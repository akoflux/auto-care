using FleetManager.Models;
using FleetManager.Services;
using System.Windows;
using System.Windows.Input;

namespace FleetManager.Views
{
    public partial class LoginWindow : Window
    {
        private readonly DatabaseService _dbService;

        public LoginWindow()
        {
            InitializeComponent();
            _dbService = new DatabaseService();
            TxtEmail.Focus();
        }

        private void BtnConnexion_Click(object sender, RoutedEventArgs e)
        {
            Connexion();
        }

        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Connexion();
            }
        }

        private void Connexion()
        {
            string email = TxtEmail.Text.Trim();
            string password = TxtPassword.Password;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                TxtErreur.Text = "Veuillez remplir tous les champs.";
                return;
            }

            try
            {
                var utilisateur = _dbService.Authentifier(email, password);

                if (utilisateur != null)
                {
                    SessionUtilisateur.UtilisateurConnecte = utilisateur;

                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    TxtErreur.Text = "Email ou mot de passe incorrect.";
                    TxtPassword.Clear();
                    TxtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                TxtErreur.Text = $"Erreur de connexion : {ex.Message}";
            }
        }
    }
}
