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
        public Game GetGame()
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

        public void SetGame(Game game) { }

        public void Delete(Game game) { }

        #endregion
    }
}
