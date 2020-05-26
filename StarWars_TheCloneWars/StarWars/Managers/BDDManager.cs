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
                int insertedid;

                try
                {
                    //Clear tables
                    SqlCommand com = new SqlCommand("ResetTables", connection) { CommandType = CommandType.StoredProcedure };
                    com.ExecuteNonQuery();

                    /// INSERT PJ
                    com = new SqlCommand(String.Format("INSERT INTO Type_Troop ([Type]) OUTPUT INSERTED.Id_Type_Troop VALUES ('{0}')", game.PJ.GetType().Name),connection);
                    insertedid = (int)com.ExecuteScalar();
                    com = new SqlCommand(String.Format("INSERT INTO Troop (Id_Type_Troop, Remaining_HP, Id_Game, PositionAbsciss, PositionOrdinate) OUTPUT INSERTED.Id_Troop VALUES ({0},{1},{2},'{3}',{4})", insertedid, game.PJ.Remaining_HP,1,game.PJ.Position.Absciss, game.PJ.Position.Ordinate), connection);
                    insertedid = (int)com.ExecuteScalar();

                    /// INSERT GAME
                    com = new SqlCommand(String.Format("INSERT INTO GAME (Id_Game, PJ, GridSize, TurnNumber) VALUES ({0},{1},{2},{3})",1, insertedid, game.Size, game.Current_turn_number ), connection);
                    com.ExecuteNonQuery();

                    /// INSERT GAME TROOPS
                    foreach (IBaseTroop troop in game.Troops)
                    {
                        com = new SqlCommand(String.Format("INSERT INTO Type_Troop ([Type]) OUTPUT INSERTED.Id_Type_Troop VALUES ('{0}')", troop.GetType().Name), connection);
                        insertedid = (int)com.ExecuteScalar();
                        com = new SqlCommand(String.Format("INSERT INTO Troop (Id_Type_Troop, Remaining_HP, Id_Game, PositionAbsciss, PositionOrdinate) VALUES ({0},{1},{2},'{3}',{4})", insertedid, troop.Remaining_HP, 1, troop.Position.Absciss, troop.Position.Ordinate), connection);
                        com.ExecuteNonQuery();
                    }
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
