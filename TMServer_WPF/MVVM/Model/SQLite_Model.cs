using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Collections.ObjectModel;
using TMServer_WPF.CORE;

namespace TMServer_WPF.MVVM.Model
{
    class SQLite_Model
    {

        /*
         * Structure database
         * 
         * table:
         *      USERS
         *          id, name, host, description, guid;
         *      TASKS
         *          id, title, description, guid, ischecked, state, hint, user_guid
         *      COMMENTS
         *          id, user_guid, task_guid, message
         * 
         */

        private string data_path = "";
        private string data_source = "";

        private string[] users = null;
        private string[] tasks = null;
        private string[] comments = null;

        private SQLiteConnection connection = null;
        private SQLiteCommand command = null;
        SQLiteTransaction transaction = null;

        private static SQLite_Model instance = null;

        public static SQLite_Model GetDB()
        {
            if (instance == null)
                instance = new SQLite_Model();
            return instance;
        }

        private SQLite_Model()
        {
            Init();
        }

        #region Init
        private void Init()
        {
            bool newDB = false;

            data_path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
            + Path.DirectorySeparatorChar
            + "Assets" + Path.DirectorySeparatorChar;

            data_source = data_path + "Storage.db";

            users = new string[] { "USERS", "id", "name", "host", "description", "guid" };
            tasks = new string[] { "TASKS", "id", "title", "description", "guid", "ischecked", "state", "hint", "user_guid" };
            comments = new string[] { "COMMENTS", "id", "user_guid", "task_guid", "guid", "message" };

            if (!File.Exists(data_source))
            {
                if (!Directory.Exists(data_path))
                    Directory.CreateDirectory(data_path);

                SQLiteConnection.CreateFile(data_source);

                newDB = true;
            }

            connection = new SQLiteConnection(String.Format("Data Source={0};", data_source));

            if (newDB)
            {
                CreateTables(users);
                CreateTables(tasks);
                CreateTables(comments);
            } 
        }
        private void CreateTables(string[] fields)
        {
            connection.Open();
            SQLiteTransaction sqliteTransaction = connection.BeginTransaction();
            string query = "";
            for (int i = 2; i < fields.Length; i++)
            {
                query += fields[i] + " TEXT,";
            }
            query = String.Format("CREATE TABLE IF NOT EXISTS '{0}' (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, {1} );", fields[0], query.TrimEnd(','));
            command = new SQLiteCommand(query, connection);
            command.ExecuteNonQuery();
            sqliteTransaction.Commit();
            connection.Close();
        }
        #endregion
        
        #region CRUD
        public ObservableCollection<Task> GetAllTasks()
        {
            ObservableCollection<Task> tasks = new ObservableCollection<Task>();
            connection.Open();
            transaction = connection.BeginTransaction();

            string query = String.Format("SELECT * FROM 'TASKS'");
            command = new SQLiteCommand(query, connection);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Task task = new Task();

                task.Title = reader.GetValue(1).ToString();
                task.Description = reader.GetValue(2).ToString();
                task.Guid = new Guid(reader.GetValue(3).ToString());
                task.IsChecked = Convert.ToBoolean(reader.GetValue(4).ToString());
                task.State = Convert.ToBoolean(reader.GetValue(5).ToString());
                //task.Hint = reader.GetValue(6).ToString();
                task.Hint = GetHint(task);
                task.User = GetUser(new Guid(reader.GetValue(7).ToString()), false);
                task.Comments = GetComments(task.Guid, false);

                tasks.Add(task);
            }
            reader.Close();
            transaction.Commit();
            connection.Close();
            return tasks;
        } //Complete
        private string GetHint(Task task)
        {
            string str = task.Title + Environment.NewLine;
            foreach(Comment comment in GetComments(task.Guid, false))
            {
                str += comment.User.Name + Environment.NewLine + comment.Message + Environment.NewLine;
            }
            return str;
        }
        public ObservableCollection<User> GetAllUsers()
        {
            ObservableCollection<User> users = new ObservableCollection<User>();
            connection.Open();
            transaction = connection.BeginTransaction();

            string query = String.Format("SELECT * FROM 'USERS'");
            command = new SQLiteCommand(query, connection);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                User user = new User();
                user.Name = reader.GetValue(1).ToString();
                user.Host = reader.GetValue(2).ToString();
                user.Description = reader.GetValue(3).ToString();
                user.Guid = new Guid(reader.GetValue(4).ToString());

                users.Add(user);
            }
            reader.Close();
            transaction.Commit();
            connection.Close();
            return users;
        } //Complete
        public ObservableCollection<Comment> GetAllComments()
        {
            ObservableCollection<Comment> comments = new ObservableCollection<Comment>();
            connection.Open();
            transaction = connection.BeginTransaction();
            string query = String.Format("SELECT * FROM 'COMMENTS'");
            command = new SQLiteCommand(query, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Comment comment = new Comment();
                comment.User = GetUser(new Guid(reader.GetValue(1).ToString()),false);
                comment.TaskGuid = new Guid(reader.GetValue(2).ToString());
                comment.Message = reader.GetValue(3).ToString();

                comments.Add(comment);
            }
            transaction.Commit();
            connection.Close();
            reader.Close();
            return comments;
        } //Complete
        
        public ObservableCollection<Comment> GetComments(Guid taskGuid, bool flag)
        {
            ObservableCollection<Comment> comments = new ObservableCollection<Comment>();
            if (flag)
            {
                connection.Open();
                transaction = connection.BeginTransaction();
            }
            string query = String.Format("SELECT * FROM 'COMMENTS' WHERE task_guid = '{0}'", taskGuid);
            command = new SQLiteCommand(query, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Comment comment = new Comment();
                comment.User = GetUser(new Guid(reader.GetValue(1).ToString()),false);
                comment.TaskGuid = new Guid(reader.GetValue(2).ToString());
                comment.Message = reader.GetValue(3).ToString();

                comments.Add(comment);
            }
            reader.Close();
            if (flag)
            {
                transaction.Commit();
                connection.Close();
            }
            return comments;
        } //Complete
        public ObservableCollection<Task> GetTasks(Guid userGuid, bool flag)
        {
            ObservableCollection<Task> tasks = new ObservableCollection<Task>();
            if (flag)
            {
                connection.Open();
                transaction = connection.BeginTransaction();
            }
            string query = String.Format("SELECT * FROM 'TASKS' WHERE 'user_guid' = '" + userGuid + "'"); 
            command = new SQLiteCommand(query, connection);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Task task = new Task();

                task.Title = reader.GetValue(1).ToString();
                task.Description = reader.GetValue(2).ToString();
                task.Guid = new Guid(reader.GetValue(3).ToString());
                task.IsChecked = Convert.ToBoolean(reader.GetValue(4).ToString());
                task.State = Convert.ToBoolean(reader.GetValue(5).ToString());
                task.Hint = reader.GetValue(6).ToString();
                task.User = GetUser(new Guid(reader.GetValue(7).ToString()),false);
                task.Comments = GetComments(task.Guid,false);

                tasks.Add(task);
            }
            reader.Close();
            if (flag)
            {
                transaction.Commit();
                connection.Close();
            }
            return tasks;
        } //Complete

        public User GetUser(Guid guid, bool flag)
        {
            User user = null;
            if (flag)
            {
                connection.Open();
                transaction = connection.BeginTransaction();
            }
            string query = String.Format("SELECT * FROM 'USERS' WHERE guid = '{0}'", guid);
            command = new SQLiteCommand(query, connection);
            command.ExecuteNonQuery();
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                user = new User();
                user.Name = reader.GetValue(1).ToString();
                user.Host = reader.GetValue(2).ToString();
                user.Description = reader.GetValue(3).ToString();
                user.Guid = new Guid(reader.GetValue(4).ToString());
            }
            reader.Close();
            if (flag)
            {
                transaction.Commit();
                connection.Close();
            }
            return user;
        } //Complete
        public Comment GetComment(Guid guid, bool flag)
        {
            Comment comment = null;
            if (flag)
            {
                connection.Open();
                transaction = connection.BeginTransaction();
            }
            string query = String.Format("SELECT * FROM 'COMMENTS' WHERE 'guid' = '" + guid + "'");
            command = new SQLiteCommand(query, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                comment = new Comment();
                comment.User = GetUser(new Guid(reader.GetValue(1).ToString()),false);
                comment.TaskGuid = new Guid(reader.GetValue(2).ToString());
                comment.Message = reader.GetValue(3).ToString();
            }
            reader.Close();
            if (flag)
            {
                transaction.Commit();
                connection.Close();
            }
            return comment;
        } //Complete
        public Task GetTask(Guid guid, bool flag)
        {
            Task task = null;
            if (flag)
            {
                connection.Open();
                transaction = connection.BeginTransaction();
            }
            string query = String.Format("SELECT * FROM 'TASKS' WHERE 'guid' = '" + guid + "'");
            command = new SQLiteCommand(query, connection);
            command.ExecuteNonQuery();
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                task = new Task();

                task.Title = reader.GetValue(1).ToString();
                task.Description = reader.GetValue(2).ToString();
                task.Guid = new Guid(reader.GetValue(3).ToString());
                task.IsChecked = Convert.ToBoolean(reader.GetValue(4).ToString());
                task.State = Convert.ToBoolean(reader.GetValue(5).ToString());
                task.Hint = reader.GetValue(6).ToString();
                task.User = GetUser(new Guid(reader.GetValue(7).ToString()),false);
                task.Comments = GetComments(task.Guid,false);
            }
            reader.Close();
            if (flag)
            {
                transaction.Commit();
                connection.Close();
            }
            return task;
        } //Complete
        
        public void InsertAllTasks(ObservableCollection<Task> _tasks)
        {
            connection.Open();
            transaction = connection.BeginTransaction();
            foreach (Task task in _tasks)
            {
                string name = "";
                for (int i = 2; i < tasks.Length; i++)
                    name += "'" + tasks[i] + "',";

                name = name.TrimEnd(',');
                string value = "'" + task.Title + "', '" + task.Description + "', '" + task.Guid + "', '" + task.IsChecked + "', '" + task.State + "', '" + task.Hint + "', '" + task.User.Guid + "'";
                string query = String.Format("INSERT INTO 'TASKS' ({0}) VALUES ({1});", name, value);

                command = new SQLiteCommand(query, connection);
                command.ExecuteNonQuery();
            }
            transaction.Commit();
            connection.Close();
        } //Complete
        public void InsertAllUsers(ObservableCollection<User> _users)
        {
            connection.Open();
            transaction = connection.BeginTransaction();
            foreach (User user in _users)
            {
                string name = "";
                for (int i = 2; i < users.Length; i++)
                    name += "'" + users[i] + "',";

                name = name.TrimEnd(',');
                string value = "'" + user.Name + "', '" + user.Host + "', '" + user.Description + "', '" + user.Guid + "'";
                string query = String.Format("INSERT INTO 'USERS' ({0}) VALUES ({1});", name, value);

                command = new SQLiteCommand(query, connection);
                command.ExecuteNonQuery();
            }
            transaction.Commit();
            connection.Close();
        } //Complete
        public void InsertAllComments(ObservableCollection<Comment> _comments)
        {
            connection.Open();
            transaction = connection.BeginTransaction();
            foreach (Comment comment in _comments)
            {
                string name = "";
                for (int i = 2; i < comments.Length; i++)
                    name += "'" + comments[i] + "',";

                name = name.TrimEnd(',');
                string value = "'" + comment.User.Guid + "', '" + comment.TaskGuid + "', '" + comment.Guid + "', '" + comment.Message +  "'";
                string query = String.Format("INSERT INTO 'COMMENTS' ({0}) VALUES ({1});", name, value);

                command = new SQLiteCommand(query, connection);
                command.ExecuteNonQuery();
            }
            transaction.Commit();
            connection.Close();
        } //Complete
        
        public void InsertUser(User user)
        {
            string name = "";
            for (int i = 2; i < users.Length; i++)
                name += "'" + users[i] + "',";

            name = name.TrimEnd(',');
            string value = "'" + user.Name + "', '" + user.Host + "', '" + user.Description + "', '" + user.Guid + "'";
            string query = String.Format("INSERT INTO 'USERS' ({0}) VALUES ({1});", name, value);

            connection.Open();
            transaction = connection.BeginTransaction();

            command = new SQLiteCommand(query, connection);
            command.ExecuteNonQuery();
            transaction.Commit();
            connection.Close();
        } //Complete
        public void InsertComment(Comment comment)
        {
            string name = "";
            for (int i = 2; i < comments.Length; i++)
                name += "'" + comments[i] + "',";

            name = name.TrimEnd(',');
            string value = "'" + comment.User.Guid + "', '" + comment.TaskGuid + "', '" + comment.Message + "'";
            string query = String.Format("INSERT INTO 'COMMENTS' ({0}) VALUES ({1});", name, value);

            connection.Open();
            transaction = connection.BeginTransaction();

            command = new SQLiteCommand(query, connection);
            command.ExecuteNonQuery();

            transaction.Commit();
            connection.Close();

        } //Complete
        public void InsertTask(Task task)
        {
            string name = "";
            for (int i = 2; i < tasks.Length; i++)
                name += "'" + tasks[i] + "',";

            name = name.TrimEnd(',');
            string value = "'" + task.Title + "', '" + task.Description + "', '" + task.Guid + "', '" + task.IsChecked + "', '" + task.State + "', '" + task.Hint + "', '" + task.User.Guid + "'";
            string query = String.Format("INSERT INTO 'TASKS' ({0}) VALUES ({1});", name, value);

            connection.Open();
            transaction = connection.BeginTransaction();
            command = new SQLiteCommand(query, connection);
            command.ExecuteNonQuery();
            transaction.Commit();
            connection.Close();
        } //Complete
        #endregion
        
    }
}
