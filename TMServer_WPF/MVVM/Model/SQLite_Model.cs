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
        //SQLiteDataReader reader = null;
        SQLiteTransaction sqliteTransaction = null;

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
            comments = new string[] { "COMMENTS", "id", "user_guid", "task_guid", "message" };

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
            sqliteTransaction = connection.BeginTransaction();

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
                task.Hint = reader.GetValue(6).ToString();
                task.User = GetUser(new Guid(reader.GetValue(7).ToString()));
                task.Comments = GetComments(task.Guid);

                tasks.Add(task);
            }
            reader.Close();

            sqliteTransaction.Commit();
            connection.Close();

            return tasks;
        } //Complete
        public ObservableCollection<User> GetAllUsers()
        {
            ObservableCollection<User> users = new ObservableCollection<User>();
            connection.Open();
            sqliteTransaction = connection.BeginTransaction();

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
            sqliteTransaction.Commit();
            connection.Close();
            return users;
        } //Complete
        public User GetUser(Guid guid)
        {
            User user = null;

            string query = String.Format("SELECT * FROM 'USERS' WHERE 'guid' = '" + guid + "'");
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
            return user;
        } //Complete
        public ObservableCollection<Comment> GetAllComments()
        {
            ObservableCollection<Comment> comments = new ObservableCollection<Comment>();
            string query = String.Format("SELECT * FROM 'COMMENTS'");
            command = new SQLiteCommand(query, connection);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Comment comment = new Comment();
                comment.User = GetUser(new Guid(reader.GetValue(1).ToString()));
                comment.TaskGuid = new Guid(reader.GetValue(2).ToString());
                comment.Message = reader.GetValue(3).ToString();

                comments.Add(comment);
            }
            reader.Close();
            return comments;
        } //Complete
        public ObservableCollection<Comment> GetComments(Guid guid)
        {
            ObservableCollection<Comment> comments = new ObservableCollection<Comment>();
            string query = String.Format("SELECT * FROM 'COMMENTS' WHERE 'guid' = '" + guid + "'");
            command = new SQLiteCommand(query, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Comment comment = new Comment();
                comment.User = GetUser(new Guid(reader.GetValue(1).ToString()));
                comment.TaskGuid = new Guid(reader.GetValue(2).ToString());
                comment.Message = reader.GetValue(3).ToString();

                comments.Add(comment);
            }
            reader.Close();
            return comments;
        } //Complete
        public Comment GetComment(Guid guid)
        {
            Comment comment = null;

            string query = String.Format("SELECT * FROM 'COMMENTS' WHERE 'guid' = '" + guid + "'");
            command = new SQLiteCommand(query, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                comment = new Comment();
                comment.User = GetUser(new Guid(reader.GetValue(1).ToString()));
                comment.TaskGuid = new Guid(reader.GetValue(2).ToString());
                comment.Message = reader.GetValue(3).ToString();
            }
            reader.Close();

            return comment;
        } //Complete
        public void InsertAllTask(ObservableCollection<Task> _tasks)
        {
            connection.Open();
            sqliteTransaction = connection.BeginTransaction();
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
            sqliteTransaction.Commit();
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
            sqliteTransaction = connection.BeginTransaction();

            command = new SQLiteCommand(query, connection);
            command.ExecuteNonQuery();
            sqliteTransaction.Commit();
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
            sqliteTransaction = connection.BeginTransaction();

            command = new SQLiteCommand(query, connection);
            command.ExecuteNonQuery();

            sqliteTransaction.Commit();
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
            sqliteTransaction = connection.BeginTransaction();
            command = new SQLiteCommand(query, connection);
            command.ExecuteNonQuery();
            sqliteTransaction.Commit();
            sqliteTransaction.Commit();
            connection.Close();
        } //Complete
        #endregion

        #region CRUD One
        //// Task
        //public ObservableCollection<Task> GetAllTasks()
        //{
        //    ObservableCollection<Task> tasks = new ObservableCollection<Task>() ;
        //    bool state = false;
        //    if(connection.State != System.Data.ConnectionState.Open)
        //    {
        //        connection.Open();
        //        sqliteTransaction = connection.BeginTransaction();
        //        state = true;
        //    }

        //    string query = String.Format("SELECT * FROM 'TASKS'");
        //    command = new SQLiteCommand(query, connection);
        //    reader = command.ExecuteReader();

        //    while (reader.Read())
        //    {
        //        Task task = new Task();

        //        task.Title = reader.GetValue(1).ToString();
        //        task.Description = reader.GetValue(2).ToString();
        //        task.Guid = new Guid(reader.GetValue(3).ToString());
        //        task.IsChecked = Convert.ToBoolean(reader.GetValue(4).ToString());
        //        task.State = Convert.ToBoolean(reader.GetValue(5).ToString());
        //        task.Hint = reader.GetValue(6).ToString();
        //        task.User = GetUser(new Guid(reader.GetValue(7).ToString()));
        //        task.Comments = GetComments(task.Guid);

        //        tasks.Add(task);
        //    }
        //    reader.Close();

        //    if (state)
        //    {
        //        sqliteTransaction.Commit();
        //        connection.Close();
        //    }

        //    return tasks;
        //} //Complete
        //public ObservableCollection<Task> GetTasks(Guid guid)
        //{
        //    ObservableCollection<Task> tasks = null;

        //    bool state = false;
        //    if (connection.State != System.Data.ConnectionState.Open)
        //    {
        //        connection.Open();
        //        sqliteTransaction = connection.BeginTransaction();
        //        state = true;
        //    }

        //    string query = String.Format("SELECT * FROM 'TASKS' WHERE 'guid' = '" + guid + "'");
        //    command = new SQLiteCommand(query, connection);
        //    command.ExecuteNonQuery();
        //    reader = command.ExecuteReader();

        //    while (reader.Read())
        //    {
        //        Task task = new Task();

        //        task.Title = reader.GetValue(1).ToString();
        //        task.Description = reader.GetValue(2).ToString();
        //        task.Guid = new Guid(reader.GetValue(3).ToString());
        //        task.IsChecked = Convert.ToBoolean(reader.GetValue(4).ToString());
        //        task.State = Convert.ToBoolean(reader.GetValue(5).ToString());
        //        task.Hint = reader.GetValue(6).ToString();
        //        task.User = GetUser(new Guid(reader.GetValue(7).ToString()));
        //        task.Comments = GetComments(task.Guid);

        //        tasks.Add(task);
        //    }
        //    reader.Close();

        //    if (state)
        //    {
        //        sqliteTransaction.Commit();
        //        connection.Close();
        //    }

        //    return tasks;
        //} //Complete
        //public Task GetTask(Guid guid)
        //{
        //    Task task = null;

        //    bool state = false;
        //    if (connection.State != System.Data.ConnectionState.Open)
        //    {
        //        connection.Open();
        //        sqliteTransaction = connection.BeginTransaction();
        //        state = true;
        //    }

        //    string query = String.Format("SELECT * FROM 'TASKS' WHERE 'guid' = '" + guid + "'");
        //    command = new SQLiteCommand(query, connection);
        //    command.ExecuteNonQuery();
        //    reader = command.ExecuteReader();

        //    while (reader.Read())
        //    {
        //        task = new Task();

        //        task.Title = reader.GetValue(1).ToString();
        //        task.Description = reader.GetValue(2).ToString();
        //        task.Guid = new Guid(reader.GetValue(3).ToString());
        //        task.IsChecked = Convert.ToBoolean(reader.GetValue(4).ToString());
        //        task.State = Convert.ToBoolean(reader.GetValue(5).ToString());
        //        task.Hint = reader.GetValue(6).ToString();
        //        task.User = GetUser(new Guid(reader.GetValue(7).ToString()));
        //        task.Comments = GetComments(task.Guid);
        //    }
        //    reader.Close();

        //    if (state)
        //    {
        //        sqliteTransaction.Commit();
        //        connection.Close();
        //    }

        //    return task;
        //} //Complete
        //public void InsertAllTask(ObservableCollection<Task> _tasks)
        //{
        //    bool state = false;
        //    if (connection.State != System.Data.ConnectionState.Open)
        //    {
        //        connection.Open();
        //        sqliteTransaction = connection.BeginTransaction();
        //        state = true;
        //    }

        //    foreach (Task task in _tasks)
        //    {
        //        string name = "";
        //        for (int i = 2; i < tasks.Length; i++)
        //            name += "'" + tasks[i] + "',";

        //        name = name.TrimEnd(',');
        //        string value = "'" + task.Title + "', '" + task.Description + "', '" + task.Guid + "', '" + task.IsChecked + "', '" + task.State + "', '" + task.Hint + "', '" + task.User.Guid + "'";
        //        string query = String.Format("INSERT INTO 'TASKS' ({0}) VALUES ({1});", name, value);

        //        command = new SQLiteCommand(query, connection);
        //        command.ExecuteNonQuery();
        //    }

        //    if (state)
        //    {
        //        sqliteTransaction.Commit();
        //        connection.Close();
        //    }
        //} //Complete
        //public void InsertTask(Task task)
        //{
        //    string name = "";
        //    for(int i = 2; i<tasks.Length; i++)
        //        name += "'" + tasks[i] + "',";

        //    name = name.TrimEnd(',');
        //    string value = "'" + task.Title + "', '" + task.Description + "', '" + task.Guid + "', '" + task.IsChecked + "', '" + task.State + "', '" + task.Hint + "', '" + task.User.Guid + "'";
        //    string query = String.Format("INSERT INTO 'TASKS' ({0}) VALUES ({1});", name, value);

        //    bool state = false;
        //    if (connection.State != System.Data.ConnectionState.Open)
        //    {
        //        connection.Open();
        //        sqliteTransaction = connection.BeginTransaction();
        //        state = true;
        //    }
        //    command = new SQLiteCommand(query, connection);
        //    command.ExecuteNonQuery();
        //    sqliteTransaction.Commit();

        //    if (state)
        //    {
        //        sqliteTransaction.Commit();
        //        connection.Close();
        //    }
        //} //Complete
        //public void DeleteTask(Task task)
        //{
        //    bool state = false;
        //    if (connection.State != System.Data.ConnectionState.Open)
        //    {
        //        connection.Open();
        //        sqliteTransaction = connection.BeginTransaction();
        //        state = true;
        //    }

        //    if (state)
        //    {
        //        sqliteTransaction.Commit();
        //        connection.Close();
        //    }
        //}
        //public void UpdataTask(Task task)
        //{
        //    bool state = false;
        //    if (connection.State != System.Data.ConnectionState.Open)
        //    {
        //        connection.Open();
        //        sqliteTransaction = connection.BeginTransaction();
        //        state = true;
        //    }

        //    if (state)
        //    {
        //        sqliteTransaction.Commit();
        //        connection.Close();
        //    }
        //}

        //// User
        //public ObservableCollection<User> GetAllUsers()
        //{
        //    ObservableCollection<User> users = new ObservableCollection<User>();
        //    bool state = false;
        //    if (connection.State != System.Data.ConnectionState.Open)
        //    {
        //        connection.Open();
        //        sqliteTransaction = connection.BeginTransaction();
        //        state = true;
        //    }

        //    string query = String.Format("SELECT * FROM 'USERS'");
        //    command = new SQLiteCommand(query, connection);
        //    reader = command.ExecuteReader();

        //    while (reader.Read())
        //    {
        //        User user = new User();
        //        user.Name = reader.GetValue(1).ToString();
        //        user.Host = reader.GetValue(2).ToString();
        //        user.Description = reader.GetValue(3).ToString();
        //        user.Guid = new Guid(reader.GetValue(4).ToString());

        //        users.Add(user);
        //    }
        //    reader.Close();
        //    if (state)
        //    {
        //        sqliteTransaction.Commit();
        //        connection.Close();
        //    }
        //    return users;
        //} //Complete
        //public User GetUser(Guid guid)
        //{
        //    User user = null;

        //    bool state = false;
        //    if (connection.State != System.Data.ConnectionState.Open)
        //    {
        //        connection.Open();
        //        sqliteTransaction = connection.BeginTransaction();
        //        state = true;
        //    }

        //    string query = String.Format("SELECT * FROM 'USERS' WHERE 'guid' = '" + guid + "'");
        //    command = new SQLiteCommand(query, connection);
        //    command.ExecuteNonQuery();
        //    reader = command.ExecuteReader();

        //    while (reader.Read())
        //    {
        //        user = new User();
        //        user.Name = reader.GetValue(1).ToString();
        //        user.Host = reader.GetValue(2).ToString();
        //        user.Description = reader.GetValue(3).ToString();
        //        user.Guid = new Guid(reader.GetValue(4).ToString());
        //    }
        //    reader.Close();
        //    sqliteTransaction.Commit();

        //    if (state)
        //    {
        //        sqliteTransaction.Commit();
        //        connection.Close();
        //    }

        //    return user;
        //} //Complete
        //public void InsertUser(User user)
        //{
        //    string name = "";
        //    for (int i = 2; i < users.Length; i++)
        //        name += "'" + users[i] + "',";

        //    name = name.TrimEnd(',');
        //    string value = "'" + user.Name + "', '" + user.Host + "', '" + user.Description + "', '" + user.Guid +  "'";
        //    string query = String.Format("INSERT INTO 'USERS' ({0}) VALUES ({1});", name, value);

        //    bool state = false;
        //    if (connection.State != System.Data.ConnectionState.Open)
        //    {
        //        connection.Open();
        //        sqliteTransaction = connection.BeginTransaction();
        //        state = true;
        //    }

        //    command = new SQLiteCommand(query, connection);
        //    command.ExecuteNonQuery();
        //    if (state)
        //    {
        //        sqliteTransaction.Commit();
        //        connection.Close();
        //    }
        //} //Complete
        //public void DeleteUser(User user)
        //{
        //    bool state = false;
        //    if (connection.State != System.Data.ConnectionState.Open)
        //    {
        //        connection.Open();
        //        sqliteTransaction = connection.BeginTransaction();
        //        state = true;
        //    }

        //    if (state)
        //    {
        //        sqliteTransaction.Commit();
        //        connection.Close();
        //    }
        //}
        //public void UpdataUser(User user)
        //{
        //    bool state = false;
        //    if (connection.State != System.Data.ConnectionState.Open)
        //    {
        //        connection.Open();
        //        sqliteTransaction = connection.BeginTransaction();
        //        state = true;
        //    }

        //    if (state)
        //    {
        //        sqliteTransaction.Commit();
        //        connection.Close();
        //    }
        //}

        //// Comment
        //public ObservableCollection<Comment> GetAllComments()
        //{
        //    ObservableCollection<Comment> comments = new ObservableCollection<Comment>();
        //    bool state = false;
        //    if (connection.State != System.Data.ConnectionState.Open)
        //    {
        //        connection.Open();
        //        sqliteTransaction = connection.BeginTransaction();
        //        state = true;
        //    }


        //    string query = String.Format("SELECT * FROM 'COMMENTS'");
        //    command = new SQLiteCommand(query, connection);
        //    reader = command.ExecuteReader();

        //    while (reader.Read())
        //    {
        //        Comment comment = new Comment();
        //        comment.User = GetUser(new Guid(reader.GetValue(1).ToString()));
        //        comment.TaskGuid = new Guid(reader.GetValue(2).ToString());
        //        comment.Message = reader.GetValue(3).ToString();

        //        comments.Add(comment);
        //    }
        //    reader.Close();
        //    if (state)
        //    {
        //        sqliteTransaction.Commit();
        //        connection.Close();
        //    }
        //    return comments;
        //} //Complete
        //public ObservableCollection<Comment> GetComments(Guid guid)
        //{
        //    ObservableCollection<Comment> comments = new ObservableCollection<Comment>();

        //    bool state = false;
        //    if (connection.State != System.Data.ConnectionState.Open)
        //    {
        //        connection.Open();
        //        sqliteTransaction = connection.BeginTransaction();
        //        state = true;
        //    }

        //    string query = String.Format("SELECT * FROM 'COMMENTS' WHERE 'guid' = '" + guid + "'");
        //    command = new SQLiteCommand(query, connection);
        //    reader = command.ExecuteReader();
        //    while (reader.Read())
        //    {
        //        Comment comment = new Comment();
        //        comment.User = GetUser(new Guid(reader.GetValue(1).ToString()));
        //        comment.TaskGuid = new Guid(reader.GetValue(2).ToString());
        //        comment.Message = reader.GetValue(3).ToString();

        //        comments.Add(comment);
        //    }
        //    reader.Close();

        //    if (state)
        //    {
        //        sqliteTransaction.Commit();
        //        connection.Close();
        //    }

        //    return comments;
        //} //Complete
        //public Comment GetComment(Guid guid)
        //{
        //    Comment comment = null;
        //    connection.Open();
        //    SQLiteTransaction sqliteTransaction = connection.BeginTransaction();

        //    string query = String.Format("SELECT * FROM 'COMMENTS' WHERE 'guid' = '" + guid + "'");
        //    command = new SQLiteCommand(query, connection);
        //    reader = command.ExecuteReader();
        //    while (reader.Read())
        //    {
        //        comment = new Comment();
        //        comment.User = GetUser(new Guid(reader.GetValue(1).ToString()));
        //        comment.TaskGuid = new Guid(reader.GetValue(2).ToString());
        //        comment.Message = reader.GetValue(3).ToString();
        //    }
        //    reader.Close();

        //    sqliteTransaction.Commit();
        //    connection.Close();
        //    return comment;
        //} //Complete
        //public void InsertComment(Comment comment)
        //{
        //    string name = "";
        //    for (int i = 2; i < comments.Length; i++)
        //        name += "'" + comments[i] + "',";

        //    name = name.TrimEnd(',');
        //    string value = "'" + comment.User.Guid + "', '" + comment.TaskGuid + "', '" + comment.Message + "'";
        //    string query = String.Format("INSERT INTO 'COMMENTS' ({0}) VALUES ({1});", name, value);

        //    bool state = false;
        //    if (connection.State != System.Data.ConnectionState.Open)
        //    {
        //        connection.Open();
        //        sqliteTransaction = connection.BeginTransaction();
        //        state = true;
        //    }

        //    command = new SQLiteCommand(query, connection);
        //    command.ExecuteNonQuery();

        //    if (state)
        //    {
        //        sqliteTransaction.Commit();
        //        connection.Close();
        //    }

        //} //Complete
        //public void DeleteComment(Comment comment)
        //{
        //    bool state = false;
        //    if (connection.State != System.Data.ConnectionState.Open)
        //    {
        //        connection.Open();
        //        sqliteTransaction = connection.BeginTransaction();
        //        state = true;
        //    }

        //    if (state)
        //    {
        //        sqliteTransaction.Commit();
        //        connection.Close();
        //    }
        //}
        //public void UpdataComment(Comment comment)
        //{
        //    bool state = false;
        //    if (connection.State != System.Data.ConnectionState.Open)
        //    {
        //        connection.Open();
        //        sqliteTransaction = connection.BeginTransaction();
        //        state = true;
        //    }

        //    if (state)
        //    {
        //        sqliteTransaction.Commit();
        //        connection.Close();
        //    }
        //}
        #endregion

        #region Two
        //#region Init
        //private void Init()
        //{
        //    data_path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
        //    + Path.DirectorySeparatorChar
        //    + "Assets" + Path.DirectorySeparatorChar;

        //    data_source = data_path + "Storage.db";

        //    users = new string[] { "USERS", "id", "name", "host", "description", "guid" };
        //    tasks = new string[] { "TASKS", "id", "title", "description", "guid", "ischecked", "state", "hint", "user_guid" };
        //    comments = new string[] { "COMMENTS", "id", "user_guid", "task_guid", "message" };

        //    CheckedDB();
        //}
        //private void CheckedDB()
        //{
        //    if (!File.Exists(data_source))
        //    {
        //        if (!Directory.Exists(data_path))
        //            Directory.CreateDirectory(data_path);

        //        SQLiteConnection.CreateFile(data_source);
        //        connection = new SQLiteConnection(String.Format("Data Source={0};", data_source));
        //        CreateTables();
        //    }
        //}
        //private void CreateTables()
        //{
        //    connection.Open();
        //    SQLiteTransaction sqliteTransaction = connection.BeginTransaction();

        //    //USERS
        //    string query = "";
        //    for (int i = 2; i<users.Length; i++)
        //    {
        //        query += users[i] + " TEXT,";
        //    }
        //    query = String.Format("CREATE TABLE IF NOT EXISTS '{0}' (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, {1} );", users[0], query.TrimEnd(','));
        //    command = new SQLiteCommand(query, connection);
        //    command.ExecuteNonQuery();

        //    //TASKS
        //    query = "";
        //    for (int i = 2; i < tasks.Length; i++)
        //    {
        //        query += tasks[i] + " TEXT,";
        //    }
        //    query = String.Format("CREATE TABLE IF NOT EXISTS '{0}' (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, {1} );", tasks[0], query.TrimEnd(','));
        //    command = new SQLiteCommand(query, connection);
        //    command.ExecuteNonQuery();

        //    //COMMITS
        //    query = "";
        //    for (int i = 2; i < comments.Length; i++)
        //    {
        //        query += comments[i] + " TEXT,";
        //    }
        //    query = String.Format("CREATE TABLE IF NOT EXISTS '{0}' (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, {1} );", comments[0], query.TrimEnd(','));
        //    command = new SQLiteCommand(query, connection);
        //    command.ExecuteNonQuery();


        //    sqliteTransaction.Commit();
        //    connection.Close();
        //}
        //#endregion

        //#region CRUD

        //#endregion
        #endregion

        #region One Code
        //private SQLite_Model()
        //{
        //    InitSQliteModel();
        //    CheckDB();
        //    connection = new SQLiteConnection(String.Format("Data Source={0};", data_source));

        //    //CT();

        //}

        //private void InitSQliteModel()
        //{
        //    data_path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
        //    + Path.DirectorySeparatorChar
        //    + "Assets" + Path.DirectorySeparatorChar;

        //    data_source = data_path + "Storage.db";
        //}

        //private void CheckDB() 
        //{
        //    if (!File.Exists(data_source))
        //    {
        //        if (!Directory.Exists(data_path))
        //            Directory.CreateDirectory(data_path);

        //        SQLiteConnection.CreateFile(data_source);
        //    }
        //}

        ////Test
        //private void CT()
        //{
        //    table_fields = new List<string>() { "TITLE", "DESCRIPTION", "GUID", "COMMENTS", "ISCHECKED", "STATE", "HINT", "USER" }.ToArray();
        //    CreateTable("TASK", table_fields);
        //}
        //public void ID(ObservableCollection<CORE.Task> tasks)
        //{

        //    connection.Open();
        //    SQLiteTransaction sqliteTransaction = connection.BeginTransaction();
        //    foreach (CORE.Task task in tasks)
        //    {
        //        string field = "'TITLE', 'DESCRIPTION', 'GUID', 'COMMENTS', 'ISCHECKED', 'STATE', 'HINT', 'USER'";
        //        string value = "'"+ task.Title + "', " + "'" + task.Description + "', " + "'" + task.Guid + "', " +
        //            "'" + task.Guid + "', " + "'" + task.IsChecked.ToString() + "', " + "'" + task.State.ToString() + "', "
        //             + "'" + task.Hint + "', " + "'" + task.Guid + "'";

        //        string query = String.Format("INSERT INTO 'TASK' ({0}) VALUES ({1})", field, value);

        //        //ExecuteQuery(query);
        //        command = new SQLiteCommand(query, connection);
        //        command.ExecuteNonQuery();

        //    }
        //    sqliteTransaction.Commit();
        //    connection.Close();
        //}      
        //public ObservableCollection<CORE.Task> GetTasks()
        //{
        //    ObservableCollection<CORE.Task> tasks = new ObservableCollection<CORE.Task>();
        //    string query = "SELECT * FROM 'TASK'";

        //    connection.Open();
        //    SQLiteTransaction sqliteTransaction = connection.BeginTransaction();

        //    command = new SQLiteCommand(query, connection);

        //    using (SQLiteDataReader reader = command.ExecuteReader())
        //    {
        //        while (reader.Read())
        //        {
        //            tasks.Add(
        //                new CORE.Task()
        //                {
        //                    Title = reader.GetValue(1).ToString(),
        //                    Description = reader.GetValue(2).ToString(),
        //                    Guid = new Guid(reader.GetValue(3).ToString()),
        //                    Comments = null,
        //                    IsChecked = Convert.ToBoolean(reader.GetValue(5)),
        //                    State = Convert.ToBoolean(reader.GetValue(6).ToString()),
        //                    Hint = reader.GetValue(7).ToString(),
        //                    User = null,
        //                }
        //                );
        //        }

        //    }

        //    sqliteTransaction.Commit();
        //    connection.Close();
        //    return tasks;
        //}
        ///*
        // * TASK -> ID, TITLE, DESCRIPTION, GUID, COMMENTS, ISCHECKED, STATE, HINT, USER;
        // * USER -> ID, NAME, HOST, GUID, DESCRIPTION;
        // * COMMENT -> ID, USER, MESSAGE;
        // */
        //private void CreateTable(string name, string[] fields)
        //{
        //    string query = "";

        //    for(int i=0; i<fields.Length; i++)
        //        query += fields[i] + " TEXT,";

        //    query = query.TrimEnd(',');
        //    query = String.Format("CREATE TABLE IF NOT EXISTS '{0}' (ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, {1} );", name, query);

        //    ExecuteQuery(query);
        //}
        //private void ExecuteQuery(string query)
        //{
        //    command = new SQLiteCommand(query, connection);
        //    connection.Open();
        //    command.ExecuteNonQuery();
        //    connection.Close();
        //}
        #endregion

    }
}
