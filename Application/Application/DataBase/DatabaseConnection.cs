using MySql.Data.MySqlClient;

namespace FleetManager.Database
{
    public class DatabaseConnection
    {
        private static DatabaseConnection _instance = null;
        private MySqlConnection _connection;

        // Chaîne de connexion - MODIFIE selon ta configuration
        private string _connectionString = "Server=localhost;Database=fleetmanager;Uid=root;Pwd=;";

        private DatabaseConnection()
        {
            try
            {
                _connection = new MySqlConnection(_connectionString);
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la création de la connexion : " + ex.Message);
            }
        }

        // Singleton - une seule instance de connexion
        public static DatabaseConnection GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DatabaseConnection();
            }
            return _instance;
        }

        // Ouvrir la connexion
        public MySqlConnection OpenConnection()
        {
            try
            {
                if (_connection.State == System.Data.ConnectionState.Closed ||
                    _connection.State == System.Data.ConnectionState.Broken)
                {
                    _connection.Open();
                }
                return _connection;
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de l'ouverture de la connexion : " + ex.Message);
            }
        }

        // Fermer la connexion
        public void CloseConnection()
        {
            try
            {
                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la fermeture de la connexion : " + ex.Message);
            }
        }

        // Tester la connexion
        public bool TestConnection()
        {
            try
            {
                OpenConnection();
                CloseConnection();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}