using FleetManager.Models;
using FleetManager.Services;
using System.Windows;
using System.Windows.Controls;

namespace FleetManager.Views
{
    public partial class VehiculesPage : Page
    {
        private readonly DatabaseService _dbService;
        private Vehicule? _vehiculeEnEdition;

        public VehiculesPage()
        {
            InitializeComponent();
            _dbService = new DatabaseService();
            DpDateAchat.SelectedDate = DateTime.Today;
            CmbCarburant.SelectedIndex = 0;
            ChargerVehicules();
        }

        private void ChargerVehicules()
        {
            try
            {
                var vehicules = _dbService.GetAllVehicules();
                DgVehicules.ItemsSource = vehicules;
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
                var vehicule = new Vehicule
                {
                    Immatriculation = TxtImmat.Text.Trim().ToUpper(),
                    Marque = TxtMarque.Text.Trim(),
                    Modele = TxtModele.Text.Trim(),
                    TypeCarburant = ((ComboBoxItem)CmbCarburant.SelectedItem).Content.ToString()!,
                    DateAchat = DpDateAchat.SelectedDate ?? DateTime.Today
                };

                if (_vehiculeEnEdition != null)
                {
                    vehicule.IdVehicule = _vehiculeEnEdition.IdVehicule;
                    if (_dbService.ImmatriculationExiste(vehicule.Immatriculation, vehicule.IdVehicule))
                    {
                        MessageBox.Show("Cette immatriculation existe deja.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    _dbService.ModifierVehicule(vehicule);
                    MessageBox.Show("Vehicule modifie avec succes.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    if (_dbService.ImmatriculationExiste(vehicule.Immatriculation))
                    {
                        MessageBox.Show("Cette immatriculation existe deja.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    _dbService.AjouterVehicule(vehicule);
                    MessageBox.Show("Vehicule ajoute avec succes.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                ResetFormulaire();
                ChargerVehicules();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var vehicule = button?.Tag as Vehicule;
            if (vehicule == null) return;

            _vehiculeEnEdition = vehicule;
            TxtImmat.Text = vehicule.Immatriculation;
            TxtMarque.Text = vehicule.Marque;
            TxtModele.Text = vehicule.Modele;
            DpDateAchat.SelectedDate = vehicule.DateAchat;

            for (int i = 0; i < CmbCarburant.Items.Count; i++)
            {
                var item = (ComboBoxItem)CmbCarburant.Items[i];
                if (item.Content.ToString() == vehicule.TypeCarburant)
                {
                    CmbCarburant.SelectedIndex = i;
                    break;
                }
            }

            BtnAjouter.Content = "Modifier";
            BtnAnnuler.Visibility = Visibility.Visible;
        }

        private void BtnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var vehicule = button?.Tag as Vehicule;
            if (vehicule == null) return;

            var result = MessageBox.Show($"Supprimer le vehicule {vehicule.InfoComplete} ?",
                "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _dbService.SupprimerVehicule(vehicule.IdVehicule);
                    ChargerVehicules();
                    MessageBox.Show("Vehicule supprime.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
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
            if (string.IsNullOrWhiteSpace(TxtImmat.Text))
            {
                MessageBox.Show("L'immatriculation est obligatoire.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(TxtMarque.Text))
            {
                MessageBox.Show("La marque est obligatoire.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(TxtModele.Text))
            {
                MessageBox.Show("Le modele est obligatoire.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (CmbCarburant.SelectedItem == null)
            {
                MessageBox.Show("Le type de carburant est obligatoire.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (DpDateAchat.SelectedDate == null)
            {
                MessageBox.Show("La date d'achat est obligatoire.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private void ResetFormulaire()
        {
            _vehiculeEnEdition = null;
            TxtImmat.Clear();
            TxtMarque.Clear();
            TxtModele.Clear();
            CmbCarburant.SelectedIndex = 0;
            DpDateAchat.SelectedDate = DateTime.Today;
            BtnAjouter.Content = "Ajouter";
            BtnAnnuler.Visibility = Visibility.Collapsed;
        }
    }
}
