using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment.Entity;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;

namespace Assignment.Model
{
    class DataAccess
    {

        private static ObservableCollection<User> ListUser = new ObservableCollection<User>();
        public static void InitializeDatabase()
        {
            using (SqliteConnection db = new SqliteConnection("Filename=users_manager.db"))
            {
                db.Open();
                String tableCommand = "CREATE TABLE IF NOT EXISTS users (id INTEGER PRIMARY KEY AUTOINCREMENT," +
                                      "name NVARCHAR(50) NOT NULL," +
                                      "email VARCHAR(100)," +
                                      "phone VARCHAR(20)," +
                                      "address NVARCHAR(100)," +
                                      "avatar NVARCHAR(100)" +
                                      ")";
                SqliteCommand createTable = new SqliteCommand(tableCommand, db);
                createTable.ExecuteReader();
            }
        }

        public static void SetUsers(ObservableCollection<User> user)
        {
            ListUser = user;
        }

        public static void AddData(User user)
        {
            DataAccess.InitializeDatabase();
            using (SqliteConnection db = new SqliteConnection("Filename=users_manager.db"))
            {
                db.Open();
                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText =
                    "INSERT INTO users(name, email, phone, address, avatar) VALUES (@name, @email, @phone, @address, @avatar);";
                insertCommand.Parameters.AddWithValue("@name", user.name);
                insertCommand.Parameters.AddWithValue("@email", user.email);
                insertCommand.Parameters.AddWithValue("@phone", user.phone);
                insertCommand.Parameters.AddWithValue("@address", user.address);
                insertCommand.Parameters.AddWithValue("@avatar", user.avatar);

                insertCommand.ExecuteReader();
                db.Close();
                if (ListUser == null)
                {
                    ListUser = new ObservableCollection<User>();
                }
                ListUser.Add(user);
            }
        }

        public static ObservableCollection<User> SelectData()
        {
            DataAccess.InitializeDatabase();
            if (ListUser == null)
            {
                ListUser = new ObservableCollection<User>();
            }

            using (SqliteConnection db = new SqliteConnection("Filename=users_manager.db"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand();
                selectCommand.Connection = db;
                selectCommand.CommandText = "SELECT * FROM users";
                SqliteDataReader sqliteData = selectCommand.ExecuteReader();
                User user;
                while (sqliteData.Read())
                {
                    int id = Convert.ToInt32(sqliteData["id"]);
                    user = new User
                    {
                        name = Convert.ToString(sqliteData["name"]),
                        email = Convert.ToString(sqliteData["email"]),
                        phone = Convert.ToString(sqliteData["phone"]),
                        address = Convert.ToString(sqliteData["address"]),
                        avatar = Convert.ToString(sqliteData["avatar"])
                    };
                    ListUser.Add(user);
                }
                db.Close();
            }
            
            
            if (ListUser == null)
            {
                ListUser = new ObservableCollection<User>();
            }

            return ListUser;
        }

        public static ObservableCollection<User> FindDataPhone(string phone)
        {
            DataAccess.InitializeDatabase();
            if (ListUser != null)
            {
                ListUser = new ObservableCollection<User>();
            }

            using (SqliteConnection db = new SqliteConnection("Filename=users_manager.db"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand();
                selectCommand.Connection = db;
                selectCommand.CommandText = "SELECT * FROM users WHERE phone = '" + phone + "'";
                SqliteDataReader sqliteData = selectCommand.ExecuteReader();
                User user;
                while (sqliteData.Read())
                {
                    int id = Convert.ToInt32(sqliteData["id"]);
                    user = new User
                    {
                        name = Convert.ToString(sqliteData["name"]),
                        email = Convert.ToString(sqliteData["email"]),
                        phone = Convert.ToString(sqliteData["phone"]),
                        address = Convert.ToString(sqliteData["address"]),
                        avatar = Convert.ToString(sqliteData["avatar"])
                    };
                    ListUser.Add(user);
                }
                db.Close();
            }
            
            if (ListUser == null)
            {
                ListUser = new ObservableCollection<User>();
            }

            return ListUser;
        }

        public static ObservableCollection<User> FindDataEmail(string email)
        {
            DataAccess.InitializeDatabase();
            if (ListUser != null)
            {
                ListUser = new ObservableCollection<User>();
            }

            using (SqliteConnection db = new SqliteConnection("Filename=users_manager.db"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand();
                selectCommand.Connection = db;
                selectCommand.CommandText = "SELECT * FROM users WHERE email = '" + email + "'";
                SqliteDataReader sqliteData = selectCommand.ExecuteReader();
                User user;
                while (sqliteData.Read())
                {

                    int id = Convert.ToInt32(sqliteData["id"]);
                    user = new User
                    {
                        name = Convert.ToString(sqliteData["name"]),
                        email = Convert.ToString(sqliteData["email"]),
                        phone = Convert.ToString(sqliteData["phone"]),
                        address = Convert.ToString(sqliteData["address"]),
                        avatar = Convert.ToString(sqliteData["avatar"])
                    };
                    ListUser.Add(user);
                }
                db.Close();
            }
            if (ListUser == null)
            {
                ListUser = new ObservableCollection<User>();
            }

            return ListUser;
        }

        public static ObservableCollection<User> FindDataNameKey(string keyword)
        {
            DataAccess.InitializeDatabase();
            if (ListUser != null)
            {
                ListUser = new ObservableCollection<User>();
            }

            using (SqliteConnection db = new SqliteConnection("Filename=users_manager.db"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand();
                selectCommand.Connection = db;
                selectCommand.CommandText = "SELECT * FROM users WHERE name LIKE '%" + keyword + "%'";
                SqliteDataReader sqliteData = selectCommand.ExecuteReader();
                User user;
                while (sqliteData.Read())
                {
                    int id = Convert.ToInt32(sqliteData["id"]);
                    user = new User
                    {
                        name = Convert.ToString(sqliteData["name"]),
                        email = Convert.ToString(sqliteData["email"]),
                        phone = Convert.ToString(sqliteData["phone"]),
                        address = Convert.ToString(sqliteData["address"]),
                        avatar = Convert.ToString(sqliteData["avatar"])
                    };
                    ListUser.Add(user);
                }
                db.Close();
            }
            if (ListUser == null)
            {
                ListUser = new ObservableCollection<User>();
            }

            return ListUser;
        }
    }
}
