using FleetManager.Models;
using FleetManager.Services;
using System.Windows;
using System.Windows.Controls;

namespace FleetManager.Views
{
    public partial class UtilisateursPage : Page
    {
        private readonly DatabaseService _dbService;
        private Utilisateur? _utilisateurEnEdition;

        public UtilisateursPage()
        {
            InitializeComponent();
            _dbService = new DatabaseService();

            // Definir le role par defaut selon les permissions
            if (SessionUtilisateur.EstSuperAdmin)
            {
                CmbRole.SelectedIndex = 2; // Employe par defaut
            }
            else
            {
                // Admin ne peut creer que des employes
                CmbRole.SelectedIndex = 2;
                ((ComboBoxItem)CmbRole.Items[0]).IsEnabled = false; // Desactiver Super Admin
                ((ComboBoxItem)CmbRole.Items[1]).IsEnabled = false; // Desactiver Admin
            }

            ChargerUtilisateurs();
        }

        private void ChargerUtilisateurs()
        {
            try
            {
                var utilisateurs = _dbService.GetAllUtilisateurs();

                // Convertir pour afficher le nom du role
                var listeAffichage = utilisateurs.Select(u => new
                {
                    u.IdUtilisateur,
                    u.Nom,
                    u.Prenom,
                    u.Email,
                    u.Password,
                    u.IdRole,
                    RoleNom = u.IdRole switch
                    {
                        1 => "Super Admin",
                        2 => "Admin",
                        3 => "Employe",
                        _ => "Inconnu"
                    }
                }).ToList();

                DgUtilisateurs.ItemsSource = listeAffichage;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAjouter_Click(object sender, RoutedEventArgs e)
        {
            if (!ValiderFormulaire()) return;

            try
            {
                var selectedRole = (ComboBoxItem)CmbRole.SelectedItem;
                int idRole = int.Parse(selectedRole.Tag.ToString()!);

                // Verifier les permissions
                if (!SessionUtilisateur.EstSuperAdmin && idRole != 3)
                {
                    MessageBox.Show("Vous ne pouvez creer que des employes.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var utilisateur = new Utilisateur
                {
                    Nom = TxtNom.Text.Trim(),
                    Prenom = TxtPrenom.Text.Trim(),
                    Email = TxtEmail.Text.Trim().ToLower(),
                    Password = TxtPassword.Password,
                    IdRole = idRole
                };

                if (_utilisateurEnEdition != null)
                {
                    utilisateur.IdUtilisateur = _utilisateurEnEdition.IdUtilisateur;

                    // Verifier qu'on ne modifie pas un SuperAdmin si on n'est pas SuperAdmin
                    if (_utilisateurEnEdition.IdRole == 1 && !SessionUtilisateur.EstSuperAdmin)
                    {
                        MessageBox.Show("Vous ne pouvez pas modifier un Super Admin.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (_dbService.EmailExiste(utilisateur.Email, utilisateur.IdUtilisateur))
                    {
                        MessageBox.Show("Cet email existe deja.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Garder l'ancien mot de passe si non modifie
                    if (string.IsNullOrEmpty(utilisateur.Password))
                    {
                        utilisateur.Password = _utilisateurEnEdition.Password;
                    }

                    _dbService.ModifierUtilisateur(utilisateur);
                    MessageBox.Show("Utilisateur modifie avec succes.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    if (string.IsNullOrEmpty(utilisateur.Password))
                    {
                        MessageBox.Show("Le mot de passe est obligatoire pour un nouvel utilisateur.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (_dbService.EmailExiste(utilisateur.Email))
                    {
                        MessageBox.Show("Cet email existe deja.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    _dbService.AjouterUtilisateur(utilisateur);
                    MessageBox.Show("Utilisateur ajoute avec succes.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                ResetFormulaire();
                ChargerUtilisateurs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            dynamic item = button?.Tag!;
            if (item == null) return;

            // Verifier les permissions
            int itemRole = (int)item.IdRole;
            if (!SessionUtilisateur.EstSuperAdmin && itemRole != 3)
            {
                MessageBox.Show("Vous ne pouvez modifier que les employes.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _utilisateurEnEdition = new Utilisateur
            {
                IdUtilisateur = (int)item.IdUtilisateur,
                Nom = (string)item.Nom,
                Prenom = (string)item.Prenom,
                Email = (string)item.Email,
                Password = (string)item.Password,
                IdRole = itemRole
            };

            TxtNom.Text = _utilisateurEnEdition.Nom;
            TxtPrenom.Text = _utilisateurEnEdition.Prenom;
            TxtEmail.Text = _utilisateurEnEdition.Email;
            TxtPassword.Clear();

            for (int i = 0; i < CmbRole.Items.Count; i++)
            {
                var roleItem = (ComboBoxItem)CmbRole.Items[i];
                if (int.Parse(roleItem.Tag.ToString()!) == _utilisateurEnEdition.IdRole)
                {
                    CmbRole.SelectedIndex = i;
                    break;
                }
            }

            BtnAjouter.Content = "Modifier";
            BtnAnnuler.Visibility = Visibility.Visible;
        }

        private void BtnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            dynamic item = button?.Tag!;
            if (item == null) return;

            int itemId = (int)item.IdUtilisateur;
            int itemRole = (int)item.IdRole;

            // Verifier les permissions
            if (!SessionUtilisateur.EstSuperAdmin && itemRole != 3)
            {
                MessageBox.Show("Vous ne pouvez supprimer que les employes.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Empecher de se supprimer soi-meme
            if (itemId == SessionUtilisateur.UtilisateurConnecte!.IdUtilisateur)
            {
                MessageBox.Show("Vous ne pouvez pas vous supprimer vous-meme.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Supprimer l'utilisateur {item.Prenom} {item.Nom} ?",
                "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _dbService.SupprimerUtilisateur(itemId);
                    ChargerUtilisateurs();
                    MessageBox.Show("Utilisateur supprime.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            ResetFormulaire();
        }

        private bool ValiderFormulaire()
        {
            if (string.IsNullOrWhiteSpace(TxtNom.Text))
            {
                MessageBox.Show("Le nom est obligatoire.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(TxtPrenom.Text))
            {
                MessageBox.Show("Le prenom est obligatoire.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(TxtEmail.Text))
            {
                MessageBox.Show("L'email est obligatoire.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (!TxtEmail.Text.Contains("@"))
            {
                MessageBox.Show("L'email n'est pas valide.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (CmbRole.SelectedItem == null)
            {
                MessageBox.Show("Le role est obligatoire.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private void ResetFormulaire()
        {
            _utilisateurEnEdition = null;
            TxtNom.Clear();
            TxtPrenom.Clear();
            TxtEmail.Clear();
            TxtPassword.Clear();

            if (SessionUtilisateur.EstSuperAdmin)
            {
                CmbRole.SelectedIndex = 2;
            }
            else
            {
                CmbRole.SelectedIndex = 2;
            }

            BtnAjouter.Content = "Ajouter";
            BtnAnnuler.Visibility = Visibility.Collapsed;
        }
    }
}
