using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TMStructure;

namespace TMService.MVVM.Model
{
    class DataBase
    {
        /*
         * Structure database
         * 
         * table:
         *      USERS
         *          id, name, host, description, guid;
         *      TASKS
         *          id, title, description, guid, ischecked, state, hint, user_guid, blocked_user_guid, enable, edit_enable
         *      COMMENTS
         *          id, user_guid, task_guid, guid, message
         * 
         */
        private string data_path = "";
        private string data_source = "";

        private string[] users = null;
        private string[] tasks = null;
        private string[] comments = null;

        private SQLiteConnection connection = null;
        // private SQLiteCommand command = null;
        private SQLiteTransaction transaction = null;

        private ObservableCollection<User> Users = null;
        private ObservableCollection<Task> Tasks = null;
        
        public ObservableCollection<Task> GetT()
        {
            return Tasks;
        }
        public ObservableCollection<User> GetU()
        {
            return Users;
        }

        private static DataBase instance = null;

        public static DataBase GetDB()
        {
            if (instance == null)
                instance = new DataBase();
            return instance;
        }

        private DataBase()
        {
            data_path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
            + Path.DirectorySeparatorChar
            + "Assets" + Path.DirectorySeparatorChar;

            data_source = data_path + "Storage.db";

            users = new string[] { "USERS", "id", "name", "host", "description", "guid" };
            tasks = new string[] { "TASKS", "id", "title", "description", "guid", "ischecked", "state", "hint", "user_guid", "blocked_user_guid", "enable", "edit_enable" };
            comments = new string[] { "COMMENTS", "id", "user_guid", "task_guid", "guid", "message" };
        }

        #region Start service
        public void Connection()
        {
            bool flag_new_db = false;

            if (!File.Exists(data_source))
            {
                if (!Directory.Exists(data_path))
                    Directory.CreateDirectory(data_path);

                SQLiteConnection.CreateFile(data_source);

                flag_new_db = true;
            }

            using (connection = new SQLiteConnection(String.Format("Data Source={0};", data_source)))
            {
                connection.Open();
                transaction = connection.BeginTransaction();

                // Create table
                if (flag_new_db)
                {
                    CreateTables(users);
                    CreateTables(tasks);
                    CreateTables(comments);
                }

                // Get data
                Users = GetUsers();
                Tasks = GetTasks();

                transaction.Commit();
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
       
        #region Get users
        private ObservableCollection<User> GetUsers()
        {
            ObservableCollection<User> users = new ObservableCollection<User>();

            string query = String.Format("SELECT * FROM 'USERS'");

            SQLiteCommand command = new SQLiteCommand(query, connection);
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
            command.Dispose();

            return users;
        }
        #endregion

        #region Get tasks
        private ObservableCollection<Task> GetTasks()
        {
            ObservableCollection<Task> tasks = new ObservableCollection<Task>();

            string query = String.Format("SELECT * FROM 'TASKS'");
            SQLiteCommand command = new SQLiteCommand(query, connection);
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
                // GetUser(Guid guid)
                task.User = GetUser(new Guid(reader.GetValue(7).ToString()));

                if(new Guid(reader.GetValue(8).ToString()) == new Guid())
                {
                    task.BlockedUser = null;
                }
                else
                {
                    task.BlockedUser = GetUser(new Guid(reader.GetValue(8).ToString()));
                }
                
                task.Enable = Convert.ToBoolean(reader.GetValue(9).ToString());
                task.EditEnable = Convert.ToBoolean(reader.GetValue(10).ToString());
                // GetComments(Guid taskGuid)
                task.Comments = GetComments(task.Guid);

                tasks.Add(task);
            }
            reader.Close();
            command.Dispose();

            return tasks;
        }

        private User GetUser(Guid guid)
        {
            User user = null;

            string query = String.Format("SELECT * FROM 'USERS' WHERE guid = '{0}'", guid);
            SQLiteCommand command = new SQLiteCommand(query, connection);
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
            command.Dispose();

            return user;
        }

        private ObservableCollection<Comment> GetComments(Guid taskGuid)
        {
            ObservableCollection<Comment> comments = new ObservableCollection<Comment>();

            string query = String.Format("SELECT * FROM 'COMMENTS' WHERE task_guid = '{0}'", taskGuid);
            SQLiteCommand command = new SQLiteCommand(query, connection);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Comment comment = new Comment();
                comment.User = GetUser(new Guid(reader.GetValue(1).ToString()));
                comment.TaskGuid = new Guid(reader.GetValue(2).ToString());
                comment.Guid = new Guid(reader.GetValue(3).ToString());
                comment.Message = reader.GetValue(4).ToString();

                comments.Add(comment);
            }

            reader.Close();
            command.Dispose();

            return comments;
        }
        #endregion

        #endregion

        #region Stop service
        public void Disconnection(ObservableCollection<User> _users, ObservableCollection<Task> _tasks)
        {
            if (File.Exists(data_source))
                File.Delete(data_source);

            if (!File.Exists(data_source))
            {
                if (!Directory.Exists(data_path))
                    Directory.CreateDirectory(data_path);

                SQLiteConnection.CreateFile(data_source);
            }

            using (connection = new SQLiteConnection(String.Format("Data Source={0};", data_source)))
            {
                connection.Open();
                transaction = connection.BeginTransaction();

                // Create table
                CreateTables(users);
                CreateTables(tasks);
                CreateTables(comments);

                InsertUsers(_users);
                InsertTasks(_tasks);

                transaction.Commit();
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        #region Insert users
        public void InsertUsers(ObservableCollection<User> _users)
        {
            foreach (User user in _users)
            {
                string name = "";
                for (int i = 2; i < users.Length; i++)
                    name += "'" + users[i] + "',";

                name = name.TrimEnd(',');
                string value = "'" + user.Name + "', '" + user.Host + "', '" + user.Description + "', '" + user.Guid + "'";
                string query = String.Format("INSERT INTO 'USERS' ({0}) VALUES ({1});", name, value);

                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.ExecuteNonQuery();
                command.Dispose();
            }
        }
        #endregion

        #region Insert tasks
        public void InsertTasks(ObservableCollection<Task> _tasks)
        {
            foreach (Task task in _tasks)
            {
                string name = "";
                for (int i = 2; i < tasks.Length; i++)
                    name += "'" + tasks[i] + "',";

                name = name.TrimEnd(',');

                string value = "";
                if (task.BlockedUser == null)
                    value = "'" + task.Title + "', '" + task.Description + "', '" + task.Guid + "', '" + task.IsChecked + "', '" + task.State + "', '" + task.Hint + "', '" + task.User.Guid + "', '" + new Guid() + "', '" + task.Enable + "', '" + task.EditEnable + "'";
                else
                    value = "'" + task.Title + "', '" + task.Description + "', '" + task.Guid + "', '" + task.IsChecked + "', '" + task.State + "', '" + task.Hint + "', '" + task.User.Guid + "', '" + task.BlockedUser.Guid + "', '" + task.Enable + "', '" + task.EditEnable + "'";


                string query = String.Format("INSERT INTO 'TASKS' ({0}) VALUES ({1});", name, value);

                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.ExecuteNonQuery();

                foreach (Comment comment in task.Comments)
                {
                    name = "";
                    for (int i = 2; i < comments.Length; i++)
                        name += "'" + comments[i] + "',";

                    name = name.TrimEnd(',');
                    value = "'" + comment.User.Guid + "', '" + comment.TaskGuid + "', '" + comment.Guid + "', '" + comment.Message + "'";
                    query = String.Format("INSERT INTO 'COMMENTS' ({0}) VALUES ({1});", name, value);

                    SQLiteCommand command_c = new SQLiteCommand(query, connection);
                    command_c.ExecuteNonQuery();
                    command_c.Dispose();
                }

                command.Dispose();
            }
        }

        #endregion
        #endregion

        private void CreateTables(string[] fields)
        {
            string query = "";
            for (int i = 2; i < fields.Length; i++)
            {
                query += fields[i] + " TEXT,";
            }
            query = String.Format("CREATE TABLE IF NOT EXISTS '{0}' (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, {1} );", fields[0], query.TrimEnd(','));
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.ExecuteNonQuery();
            command.Dispose();
        }
    }
}
