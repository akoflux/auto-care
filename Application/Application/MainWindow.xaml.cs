using FleetManager.Database;
using FleetManager.Models;
using FleetManager.Views;
using System.Windows;
using System.Windows.Media;

namespace FleetManager
{
    public partial class MainWindow : Window
    {
        private DatabaseConnection? dbConnection;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TestDatabaseConnection();
            ConfigurerInterface();
            // Charger le dashboard par défaut
            NavigateTo(new DashboardPage(), "Tableau de bord", "Vue d'ensemble de votre flotte");
        }

        private void ConfigurerInterface()
        {
            if (SessionUtilisateur.EstConnecte)
            {
                var user = SessionUtilisateur.UtilisateurConnecte!;
                TxtUserName.Text = user.NomComplet;

                string roleNom = user.IdRole switch
                {
                    1 => "Super Admin",
                    2 => "Admin",
                    3 => "Employe",
                    _ => "Inconnu"
                };
                TxtUserRole.Text = roleNom;

                // Masquer le bouton Utilisateurs pour les employes
                if (SessionUtilisateur.EstEmploye)
                {
                    BtnUtilisateurs.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void TestDatabaseConnection()
        {
            try
            {
                dbConnection = DatabaseConnection.GetInstance();

                if (dbConnection.TestConnection())
                {
                    ConnectionIndicator.Fill = new SolidColorBrush(Color.FromRgb(16, 185, 129)); // #10B981
                    ConnectionStatus.Text = "Connecte";
                }
                else
                {
                    ConnectionIndicator.Fill = new SolidColorBrush(Color.FromRgb(239, 68, 68)); // #EF4444
                    ConnectionStatus.Text = "Deconnecte";
                    MessageBox.Show("Impossible de se connecter a la base de donnees.\n" +
                                  "Verifiez que MySQL est demarre et que la base 'fleetmanager' existe.",
                                  "Erreur de connexion",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                ConnectionIndicator.Fill = new SolidColorBrush(Color.FromRgb(239, 68, 68));
                ConnectionStatus.Text = "Erreur";
                MessageBox.Show($"Erreur lors du test de connexion :\n{ex.Message}",
                              "Erreur",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void NavigateTo(System.Windows.Controls.Page page, string title, string subtitle)
        {
            MainFrame.Navigate(page);
            TxtPageTitle.Text = title;
            TxtPageSubtitle.Text = subtitle;
        }

        private void BtnDashboard_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo(new DashboardPage(), "Tableau de bord", "Vue d'ensemble de votre flotte");
        }

        private void BtnVehicules_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo(new VehiculesPage(), "Gestion des Vehicules", "Ajouter, modifier et supprimer des vehicules");
        }

        private void BtnPlein_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo(new PleinPage(), "Ajouter un plein", "Enregistrer un ravitaillement en carburant");
        }

        private void BtnTrajet_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo(new TrajetPage(), "Suivi Kilometrage", "Enregistrer les trajets effectues");
        }

        private void BtnUtilisateurs_Click(object sender, RoutedEventArgs e)
        {
            if (!SessionUtilisateur.PeutGererUtilisateurs)
            {
                MessageBox.Show("Vous n'avez pas les droits pour acceder a cette fonctionnalite.",
                    "Acces refuse", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            NavigateTo(new UtilisateursPage(), "Gestion des Utilisateurs", "Gerer les comptes utilisateurs");
        }

        private void BtnDeconnexion_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Voulez-vous vraiment vous deconnecter ?",
                "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                SessionUtilisateur.Deconnecter();
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
        }
    }
}
