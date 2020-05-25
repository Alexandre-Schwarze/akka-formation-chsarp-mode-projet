using StarWars.Entities.Interfaces;
using StarWars.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace StarWars.Managers
{
    public class BDDManager
    {
        #region Attributs
        private static BDDManager _instance;
        static readonly object instanceLock = new object();
        public static BDDManager Instance
        {
            get
            {
                if (_instance == null) //Les locks prennent du temps, il est préférable de vérifier d'abord la nullité de l'instance.
                {
                    lock (instanceLock)
                    {
                        if (_instance == null) //on vérifie encore, au cas où l'instance aurait été créée entretemps.
                            _instance = new BDDManager();
                    }
                }
                return _instance;
            }
        }

        private static string connexionstring = "Data Source = DESKTOP-M25SQU2\\SQLEXPRESS01; Initial Catalog = StarWars; Integrated Security=true;";
        #endregion

        #region Ctor
        private BDDManager() { }
        #endregion

        #region Methods
        public Game GetSavedGame()
        {
            Game game = new Game();
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = connexionstring;

                connection.Open();

                using (SqlCommand com = new SqlCommand("Get_Saved_Game", connection) { CommandType = CommandType.StoredProcedure })
                {
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        string typePJ = reader.GetString(0).Trim();
                        int remaining_HP = reader.GetInt32(1);
                        int gridsize = reader.GetInt32(2);
                        int turnnumber = reader.GetInt32(3);

                        
                        game.Size = gridsize;
                        game.Current_turn_number = turnnumber;
                        game.PJ = (IBaseTroop)Activator.CreateInstance(Assembly.GetExecutingAssembly().GetTypes().First(t => t.Name == typePJ));
                        game.PJ.Remaining_HP = remaining_HP;
                    }
                    reader.Close();
                }
                connection.Close();
                return game;
            }
        }

        public void SaveGame(Game game) 
        {
            string type = game.PJ.GetType().Name;
            int hp = game.PJ.Remaining_HP;
            int size = game.Size;
            int turnnumber = game.Current_turn_number;

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = connexionstring;

                connection.Open();

                SqlCommand com = new SqlCommand("Set_Saved_Game", connection) { CommandType = CommandType.StoredProcedure };
                com.Parameters.AddWithValue("@type_perso", type);
                com.Parameters.AddWithValue("@HP", hp);
                com.Parameters.AddWithValue("@size", size);
                com.Parameters.AddWithValue("@turnnumber", turnnumber);

                try
                {
                    Int32 rowsAffected = com.ExecuteNonQuery();
                    Console.WriteLine("RowsAffected: {0}", rowsAffected);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void Delete(Game game) { }

        #endregion
    }
}
