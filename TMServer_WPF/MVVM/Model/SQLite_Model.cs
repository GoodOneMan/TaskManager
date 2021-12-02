using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace TMServer_WPF.MVVM.Model
{
    class SQLite_Model
    {
        private string data_path = "";
        private string data_source = "";

        private string[] users = null;
        private string[] tasks = null;
        private string[] comments = null;

        private SQLiteConnection connection = null;
        private SQLiteCommand command = null;

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
            data_path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
            + Path.DirectorySeparatorChar
            + "Assets" + Path.DirectorySeparatorChar;

            data_source = data_path + "Storage.db";

            users = new string[] { "USERS", "id", "name", "host", "description", "guid" };
            tasks = new string[] { "TASKS", "id", "title", "description", "guid", "ischecked", "state", "hint", "user_guid" };
            comments = new string[] { "COMMENTS", "id", "user_guid", "task_guid", "message" };

            CheckedDB();
        }
        private void CheckedDB()
        {
            if (!File.Exists(data_source))
            {
                if (!Directory.Exists(data_path))
                    Directory.CreateDirectory(data_path);

                SQLiteConnection.CreateFile(data_source);
                connection = new SQLiteConnection(String.Format("Data Source={0};", data_source));
                CreateTables();
            }
        }
        private void CreateTables()
        {
            connection.Open();
            SQLiteTransaction sqliteTransaction = connection.BeginTransaction();

            //USERS
            string query = "";
            for (int i = 2; i<users.Length; i++)
            {
                query += users[i] + " TEXT,";
            }
            query = String.Format("CREATE TABLE IF NOT EXISTS '{0}' (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, {1} );", users[0], query.TrimEnd(','));
            command = new SQLiteCommand(query, connection);
            command.ExecuteNonQuery();

            //TASKS
            query = "";
            for (int i = 2; i < tasks.Length; i++)
            {
                query += tasks[i] + " TEXT,";
            }
            query = String.Format("CREATE TABLE IF NOT EXISTS '{0}' (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, {1} );", tasks[0], query.TrimEnd(','));
            command = new SQLiteCommand(query, connection);
            command.ExecuteNonQuery();

            //COMMITS
            query = "";
            for (int i = 2; i < comments.Length; i++)
            {
                query += comments[i] + " TEXT,";
            }
            query = String.Format("CREATE TABLE IF NOT EXISTS '{0}' (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, {1} );", comments[0], query.TrimEnd(','));
            command = new SQLiteCommand(query, connection);
            command.ExecuteNonQuery();


            sqliteTransaction.Commit();
            connection.Close();
        }
        #endregion

        #region CRUD

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

        /*
         * Structure database
         * 
         * table:
         *      USERS
         *          id, name, host, description, guid;
         *      TASKS
         *          id, title, description, guid, ischecked, state, hint, user_guid
         *      COMMENT
         *          id, user_guid, task_guid, message
         * 
         */


    }
}
