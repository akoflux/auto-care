using FleetManager.Database;
using FleetManager.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace FleetManager.Services
{
    public class DatabaseService
    {
        private readonly DatabaseConnection _db;

        public DatabaseService()
        {
            _db = DatabaseConnection.GetInstance();
        }

        #region Authentification

        public Utilisateur? Authentifier(string email, string password)
        {
            try
            {
                var conn = _db.OpenConnection();
                string query = "SELECT * FROM utilisateur WHERE email = @email AND password = @password";
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@password", password);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Utilisateur
                    {
                        IdUtilisateur = reader.GetInt32("id_utilisateur"),
                        Nom = reader.GetString("nom"),
                        Prenom = reader.GetString("prenom"),
                        Email = reader.GetString("email"),
                        Password = reader.GetString("password"),
                        IdRole = reader.GetInt32("id_role")
                    };
                }
                return null;
            }
            finally
            {
                _db.CloseConnection();
            }
        }

        #endregion

        #region Utilisateurs

        public List<Utilisateur> GetAllUtilisateurs()
        {
            var liste = new List<Utilisateur>();
            try
            {
                var conn = _db.OpenConnection();
                string query = "SELECT * FROM utilisateur ORDER BY nom, prenom";
                using var cmd = new MySqlCommand(query, conn);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    liste.Add(new Utilisateur
                    {
                        IdUtilisateur = reader.GetInt32("id_utilisateur"),
                        Nom = reader.GetString("nom"),
                        Prenom = reader.GetString("prenom"),
                        Email = reader.GetString("email"),
                        Password = reader.GetString("password"),
                        IdRole = reader.GetInt32("id_role")
                    });
                }
            }
            finally
            {
                _db.CloseConnection();
            }
            return liste;
        }

        public bool AjouterUtilisateur(Utilisateur user)
        {
            try
            {
                var conn = _db.OpenConnection();
                string query = "INSERT INTO utilisateur (nom, prenom, email, password, id_role) VALUES (@nom, @prenom, @email, @password, @role)";
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nom", user.Nom);
                cmd.Parameters.AddWithValue("@prenom", user.Prenom);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@password", user.Password);
                cmd.Parameters.AddWithValue("@role", user.IdRole);
                return cmd.ExecuteNonQuery() > 0;
            }
            finally
            {
                _db.CloseConnection();
            }
        }

        public bool ModifierUtilisateur(Utilisateur user)
        {
            try
            {
                var conn = _db.OpenConnection();
                string query = "UPDATE utilisateur SET nom = @nom, prenom = @prenom, email = @email, password = @password, id_role = @role WHERE id_utilisateur = @id";
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", user.IdUtilisateur);
                cmd.Parameters.AddWithValue("@nom", user.Nom);
                cmd.Parameters.AddWithValue("@prenom", user.Prenom);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@password", user.Password);
                cmd.Parameters.AddWithValue("@role", user.IdRole);
                return cmd.ExecuteNonQuery() > 0;
            }
            finally
            {
                _db.CloseConnection();
            }
        }

        public bool SupprimerUtilisateur(int id)
        {
            try
            {
                var conn = _db.OpenConnection();
                string query = "DELETE FROM utilisateur WHERE id_utilisateur = @id";
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                return cmd.ExecuteNonQuery() > 0;
            }
            finally
            {
                _db.CloseConnection();
            }
        }

        public bool EmailExiste(string email, int? excludeId = null)
        {
            try
            {
                var conn = _db.OpenConnection();
                string query = excludeId.HasValue
                    ? "SELECT COUNT(*) FROM utilisateur WHERE email = @email AND id_utilisateur != @id"
                    : "SELECT COUNT(*) FROM utilisateur WHERE email = @email";
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@email", email);
                if (excludeId.HasValue)
                    cmd.Parameters.AddWithValue("@id", excludeId.Value);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
            finally
            {
                _db.CloseConnection();
            }
        }

        #endregion

        #region Vehicules

        public List<Vehicule> GetAllVehicules()
        {
            var liste = new List<Vehicule>();
            try
            {
                var conn = _db.OpenConnection();
                string query = "SELECT * FROM vehicule ORDER BY marque, modele";
                using var cmd = new MySqlCommand(query, conn);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    liste.Add(new Vehicule
                    {
                        IdVehicule = reader.GetInt32("id_vehicule"),
                        Immatriculation = reader.GetString("immatriculation"),
                        Marque = reader.GetString("marque"),
                        Modele = reader.GetString("modele"),
                        TypeCarburant = reader.GetString("type_carburant"),
                        DateAchat = reader.GetDateTime("date_achat")
                    });
                }
            }
            finally
            {
                _db.CloseConnection();
            }
            return liste;
        }

        public Vehicule? GetVehiculeById(int id)
        {
            try
            {
                var conn = _db.OpenConnection();
                string query = "SELECT * FROM vehicule WHERE id_vehicule = @id";
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new Vehicule
                    {
                        IdVehicule = reader.GetInt32("id_vehicule"),
                        Immatriculation = reader.GetString("immatriculation"),
                        Marque = reader.GetString("marque"),
                        Modele = reader.GetString("modele"),
                        TypeCarburant = reader.GetString("type_carburant"),
                        DateAchat = reader.GetDateTime("date_achat")
                    };
                }
                return null;
            }
            finally
            {
                _db.CloseConnection();
            }
        }

        public bool AjouterVehicule(Vehicule vehicule)
        {
            try
            {
                var conn = _db.OpenConnection();
                string query = "INSERT INTO vehicule (immatriculation, marque, modele, type_carburant, date_achat) VALUES (@immat, @marque, @modele, @carburant, @date)";
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@immat", vehicule.Immatriculation);
                cmd.Parameters.AddWithValue("@marque", vehicule.Marque);
                cmd.Parameters.AddWithValue("@modele", vehicule.Modele);
                cmd.Parameters.AddWithValue("@carburant", vehicule.TypeCarburant);
                cmd.Parameters.AddWithValue("@date", vehicule.DateAchat);
                return cmd.ExecuteNonQuery() > 0;
            }
            finally
            {
                _db.CloseConnection();
            }
        }

        public bool ModifierVehicule(Vehicule vehicule)
        {
            try
            {
                var conn = _db.OpenConnection();
                string query = "UPDATE vehicule SET immatriculation = @immat, marque = @marque, modele = @modele, type_carburant = @carburant, date_achat = @date WHERE id_vehicule = @id";
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", vehicule.IdVehicule);
                cmd.Parameters.AddWithValue("@immat", vehicule.Immatriculation);
                cmd.Parameters.AddWithValue("@marque", vehicule.Marque);
                cmd.Parameters.AddWithValue("@modele", vehicule.Modele);
                cmd.Parameters.AddWithValue("@carburant", vehicule.TypeCarburant);
                cmd.Parameters.AddWithValue("@date", vehicule.DateAchat);
                return cmd.ExecuteNonQuery() > 0;
            }
            finally
            {
                _db.CloseConnection();
            }
        }

        public bool SupprimerVehicule(int id)
        {
            try
            {
                var conn = _db.OpenConnection();
                string query = "DELETE FROM vehicule WHERE id_vehicule = @id";
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                return cmd.ExecuteNonQuery() > 0;
            }
            finally
            {
                _db.CloseConnection();
            }
        }

        public bool ImmatriculationExiste(string immat, int? excludeId = null)
        {
            try
            {
                var conn = _db.OpenConnection();
                string query = excludeId.HasValue
                    ? "SELECT COUNT(*) FROM vehicule WHERE immatriculation = @immat AND id_vehicule != @id"
                    : "SELECT COUNT(*) FROM vehicule WHERE immatriculation = @immat";
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@immat", immat);
                if (excludeId.HasValue)
                    cmd.Parameters.AddWithValue("@id", excludeId.Value);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
            finally
            {
                _db.CloseConnection();
            }
        }

        #endregion

        #region Suivis (Pleins et Trajets)

        public bool AjouterPlein(int idVehicule, int idUtilisateur, DateTime date, decimal litres, decimal cout)
        {
            try
            {
                var conn = _db.OpenConnection();
                string query = @"INSERT INTO suivis (id_vehicule, id_utilisateur, date, conso, cout, km_depart, km_arrive)
                                VALUES (@idVehicule, @idUser, @date, @litres, @cout, 0, 0)
                                ON DUPLICATE KEY UPDATE conso = conso + @litres, cout = cout + @cout";
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idVehicule", idVehicule);
                cmd.Parameters.AddWithValue("@idUser", idUtilisateur);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@litres", litres);
                cmd.Parameters.AddWithValue("@cout", cout);
                return cmd.ExecuteNonQuery() > 0;
            }
            finally
            {
                _db.CloseConnection();
            }
        }

        public bool AjouterTrajet(int idVehicule, int idUtilisateur, DateTime dateDepart, DateTime dateArrivee, int kmDepart, int kmArrivee)
        {
            try
            {
                var conn = _db.OpenConnection();
                string query = @"INSERT INTO suivis (id_vehicule, id_utilisateur, date, conso, cout, km_depart, km_arrive)
                                VALUES (@idVehicule, @idUser, @date, 0, 0, @kmDepart, @kmArrivee)
                                ON DUPLICATE KEY UPDATE km_depart = @kmDepart, km_arrive = @kmArrivee";
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idVehicule", idVehicule);
                cmd.Parameters.AddWithValue("@idUser", idUtilisateur);
                cmd.Parameters.AddWithValue("@date", dateArrivee);
                cmd.Parameters.AddWithValue("@kmDepart", kmDepart);
                cmd.Parameters.AddWithValue("@kmArrivee", kmArrivee);
                return cmd.ExecuteNonQuery() > 0;
            }
            finally
            {
                _db.CloseConnection();
            }
        }

        public int GetDernierKm(int idVehicule)
        {
            try
            {
                var conn = _db.OpenConnection();
                string query = "SELECT COALESCE(MAX(km_arrive), 0) FROM suivis WHERE id_vehicule = @id AND km_arrive > 0";
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", idVehicule);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            finally
            {
                _db.CloseConnection();
            }
        }

        public List<Suivi> GetSuivisByVehicule(int idVehicule)
        {
            var liste = new List<Suivi>();
            try
            {
                var conn = _db.OpenConnection();
                string query = "SELECT * FROM suivis WHERE id_vehicule = @id ORDER BY date DESC";
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", idVehicule);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    liste.Add(new Suivi
                    {
                        IdUtilisateur = reader.GetInt32("id_utilisateur"),
                        IdVehicule = reader.GetInt32("id_vehicule"),
                        Date = reader.GetDateTime("date"),
                        Consommation = reader.GetDecimal("conso"),
                        Cout = reader.GetDecimal("cout"),
                        KmDepart = reader.GetInt32("km_depart"),
                        KmArrivee = reader.GetInt32("km_arrive")
                    });
                }
            }
            finally
            {
                _db.CloseConnection();
            }
            return liste;
        }

        public List<dynamic> GetHistoriquePleins()
        {
            var liste = new List<dynamic>();
            try
            {
                var conn = _db.OpenConnection();
                string query = @"SELECT s.id_vehicule, s.id_utilisateur, s.date, s.conso as litres, s.cout, v.immatriculation, v.marque, v.modele, v.type_carburant
                                FROM suivis s
                                INNER JOIN vehicule v ON s.id_vehicule = v.id_vehicule
                                WHERE s.conso > 0
                                ORDER BY s.date DESC
                                LIMIT 50";
                using var cmd = new MySqlCommand(query, conn);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    liste.Add(new
                    {
                        IdVehicule = reader.GetInt32("id_vehicule"),
                        IdUtilisateur = reader.GetInt32("id_utilisateur"),
                        Date = reader.GetDateTime("date"),
                        Litres = reader.GetDecimal("litres"),
                        Cout = reader.GetDecimal("cout"),
                        Vehicule = $"{reader.GetString("immatriculation")} - {reader.GetString("marque")} {reader.GetString("modele")}",
                        Carburant = reader.GetString("type_carburant")
                    });
                }
            }
            finally
            {
                _db.CloseConnection();
            }
            return liste;
        }

        public List<dynamic> GetHistoriqueTrajets()
        {
            var liste = new List<dynamic>();
            try
            {
                var conn = _db.OpenConnection();
                string query = @"SELECT s.id_vehicule, s.id_utilisateur, s.date, s.km_depart, s.km_arrive, v.immatriculation, v.marque, v.modele
                                FROM suivis s
                                INNER JOIN vehicule v ON s.id_vehicule = v.id_vehicule
                                WHERE s.km_arrive > s.km_depart
                                ORDER BY s.date DESC
                                LIMIT 50";
                using var cmd = new MySqlCommand(query, conn);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int kmDepart = reader.GetInt32("km_depart");
                    int kmArrivee = reader.GetInt32("km_arrive");
                    liste.Add(new
                    {
                        IdVehicule = reader.GetInt32("id_vehicule"),
                        IdUtilisateur = reader.GetInt32("id_utilisateur"),
                        Date = reader.GetDateTime("date"),
                        KmDepart = kmDepart,
                        KmArrivee = kmArrivee,
                        Distance = kmArrivee - kmDepart,
                        Vehicule = $"{reader.GetString("immatriculation")} - {reader.GetString("marque")} {reader.GetString("modele")}"
                    });
                }
            }
            finally
            {
                _db.CloseConnection();
            }
            return liste;
        }

        public bool SupprimerSuivi(int idVehicule, int idUtilisateur)
        {
            try
            {
                var conn = _db.OpenConnection();
                string query = "DELETE FROM suivis WHERE id_vehicule = @idVehicule AND id_utilisateur = @idUser";
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idVehicule", idVehicule);
                cmd.Parameters.AddWithValue("@idUser", idUtilisateur);
                return cmd.ExecuteNonQuery() > 0;
            }
            finally
            {
                _db.CloseConnection();
            }
        }

        #endregion

        #region Statistiques

        public int GetNombreVehiculesParCarburant(string typeCarburant)
        {
            try
            {
                var conn = _db.OpenConnection();
                string query = "SELECT COUNT(*) FROM vehicule WHERE type_carburant = @type";
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@type", typeCarburant);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            finally
            {
                _db.CloseConnection();
            }
        }

        public int GetTotalKmParcourus()
        {
            try
            {
                var conn = _db.OpenConnection();
                string query = "SELECT COALESCE(SUM(km_arrive - km_depart), 0) FROM suivis WHERE km_arrive > km_depart";
                using var cmd = new MySqlCommand(query, conn);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            finally
            {
                _db.CloseConnection();
            }
        }

        public decimal GetTotalCoutCarburant()
        {
            try
            {
                var conn = _db.OpenConnection();
                string query = "SELECT COALESCE(SUM(cout), 0) FROM suivis";
                using var cmd = new MySqlCommand(query, conn);
                return Convert.ToDecimal(cmd.ExecuteScalar());
            }
            finally
            {
                _db.CloseConnection();
            }
        }

        public int GetNombreTotalVehicules()
        {
            try
            {
                var conn = _db.OpenConnection();
                string query = "SELECT COUNT(*) FROM vehicule";
                using var cmd = new MySqlCommand(query, conn);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            finally
            {
                _db.CloseConnection();
            }
        }

        public decimal GetTotalLitresConsommes()
        {
            try
            {
                var conn = _db.OpenConnection();
                string query = "SELECT COALESCE(SUM(conso), 0) FROM suivis";
                using var cmd = new MySqlCommand(query, conn);
                return Convert.ToDecimal(cmd.ExecuteScalar());
            }
            finally
            {
                _db.CloseConnection();
            }
        }

        #endregion
    }
}
