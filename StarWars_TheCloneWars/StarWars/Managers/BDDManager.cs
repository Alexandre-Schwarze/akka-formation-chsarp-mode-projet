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

        public string connexionstring;
        #endregion

        #region Ctor
        private BDDManager() { }
        #endregion

        #region Methods
        public Game GetSavedGame()
        {
            using (SqlConnection connection = new SqlConnection())
            {
                Game game = new Game();
                connection.ConnectionString = connexionstring;
                try
                {
                    connection.Open();

                    int idgame = 0;
                    int idPJ = 0;

                    ///Récupérer GAME
                    SqlCommand com = new SqlCommand("SELECT * FROM Game", connection);

                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        idgame = reader.GetInt32(0);
                        idPJ = reader.GetInt32(1);
                        game.Size = reader.GetInt32(2);
                        game.Current_turn_number = reader.GetInt32(3);
                    }
                    reader.Close();

                    ///Récupérer GAME.PJ
                    IBaseTroop PJ;
                    com = new SqlCommand(String.Format("SELECT tt.[Type], t.Remaining_HP ,t.PositionAbsciss, t.PositionOrdinate FROM Troop t " +
                    "INNER JOIN Type_Troop tt ON T.Id_Type_Troop = tt.Id_Type_Troop WHERE Id_Troop = {0}", idPJ),connection);
                    reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        PJ = (IBaseTroop)Activator.CreateInstance(Assembly.GetExecutingAssembly().GetTypes().First(t => t.Name == reader.GetString(0)));
                        PJ.Remaining_HP = reader.GetInt32(1);
                        PJ.Position = new Tools.Position() { Absciss = reader.GetString(2), Ordinate = reader.GetInt32(3) };
                        game.PJ = PJ;
                    }
                    reader.Close();

                    
                    ///Récupérer GAME.TROOPS
                    List<IBaseTroop> troops = new List<IBaseTroop>();
                    com = new SqlCommand(String.Format("SELECT tt.[Type], t.Remaining_HP ,t.PositionAbsciss, t.PositionOrdinate FROM Troop t "+
                    "INNER JOIN Type_Troop tt ON T.Id_Type_Troop = tt.Id_Type_Troop WHERE Id_Game = {0} AND Id_Troop <> {1}", idgame, idPJ), connection);

                    reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        var troop = (IBaseTroop)Activator.CreateInstance(Assembly.GetExecutingAssembly().GetTypes().First(t => t.Name == reader.GetString(0)));
                        troop.Remaining_HP = reader.GetInt32(1);
                        troop.Position = new Tools.Position() { Absciss = reader.GetString(2), Ordinate = reader.GetInt32(3) };
                        troops.Add(troop);
                    }
                    reader.Close();
                    game.Troops = troops;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {                   
                    connection.Close();    
                }
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
