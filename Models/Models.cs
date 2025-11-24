using System;

namespace FleetManager.Models
{
    // Modèle Utilisateur
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

    // Modèle Véhicule
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
    }

    // Modèle Suivi
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

    // Enum pour les types de carburant (correspond à ta BDD)
    public enum TypeCarburant
    {
        Essence,
        Diesel,
        Hybride,
        Electrique
    }

    // Enum pour les rôles utilisateurs
    public enum Role
    {
        Administrateur = 1,
        Utilisateur = 2
    }
}