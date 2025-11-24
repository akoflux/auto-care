namespace FleetManager.Models
{
    // Mod�le Utilisateur
    public class Utilisateur
    {
        public int IdUtilisateur { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int IdRole { get; set; }

        public Utilisateur() { }

        public Utilisateur(int id, string nom, string prenom, string email, int role)
        {
            IdUtilisateur = id;
            Nom = nom;
            Prenom = prenom;
            Email = email;
            IdRole = role;
        }

        public string NomComplet => $"{Prenom} {Nom}";
    }

    // Mod�le V�hicule
    public class Vehicule
    {
        public int IdVehicule { get; set; }
        public string Immatriculation { get; set; }
        public string Marque { get; set; }
        public string Modele { get; set; }
        public string TypeCarburant { get; set; }
        public DateTime DateAchat { get; set; }

        public Vehicule() { }

        public Vehicule(int id, string immat, string marque, string modele, string carburant, DateTime dateAchat)
        {
            IdVehicule = id;
            Immatriculation = immat;
            Marque = marque;
            Modele = modele;
            TypeCarburant = carburant;
            DateAchat = dateAchat;
        }

        public string InfoComplete => $"{Marque} {Modele} ({Immatriculation})";

        public override string ToString() => InfoComplete;
    }

    // Mod�le Suivi
    public class Suivi
    {
        public int IdUtilisateur { get; set; }
        public int IdVehicule { get; set; }
        public DateTime Date { get; set; }
        public decimal Consommation { get; set; }
        public decimal Cout { get; set; }
        public int KmDepart { get; set; }
        public int KmArrivee { get; set; }

        public Suivi() { }

        public Suivi(int idUser, int idVehicule, DateTime date, decimal conso, decimal cout, int kmDep, int kmArr)
        {
            IdUtilisateur = idUser;
            IdVehicule = idVehicule;
            Date = date;
            Consommation = conso;
            Cout = cout;
            KmDepart = kmDep;
            KmArrivee = kmArr;
        }

        public int DistanceParcourue => KmArrivee - KmDepart;
        public decimal ConsommationMoyenne => DistanceParcourue > 0 ?
            (Consommation / DistanceParcourue) * 100 : 0;
    }

    // Enum pour les types de carburant (correspond � ta BDD)
    public enum TypeCarburant
    {
        Essence,
        Diesel,
        Hybride,
        Electrique
    }

    // Enum pour les roles utilisateurs
    public enum Role
    {
        SuperAdmin = 1,
        Admin = 2,
        Employe = 3
    }

    // Session utilisateur connecte
    public static class SessionUtilisateur
    {
        public static Utilisateur? UtilisateurConnecte { get; set; }

        public static bool EstConnecte => UtilisateurConnecte != null;

        public static bool EstSuperAdmin => UtilisateurConnecte?.IdRole == (int)Role.SuperAdmin;

        public static bool EstAdmin => UtilisateurConnecte?.IdRole == (int)Role.Admin;

        public static bool EstEmploye => UtilisateurConnecte?.IdRole == (int)Role.Employe;

        public static bool PeutGererUtilisateurs => EstSuperAdmin || EstAdmin;

        public static bool PeutGererAdmins => EstSuperAdmin;

        public static void Deconnecter()
        {
            UtilisateurConnecte = null;
        }
    }
}