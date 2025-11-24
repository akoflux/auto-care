using FleetManager.Models;
using FleetManager.Services;
using System.Windows;
using System.Windows.Controls;

namespace FleetManager.Views
{
    public partial class PleinPage : Page
    {
        private readonly DatabaseService _dbService;
        private List<Vehicule> _vehicules;

        public PleinPage()
        {
            InitializeComponent();
            _dbService = new DatabaseService();
            _vehicules = new List<Vehicule>();
            DpDate.SelectedDate = DateTime.Today;
            ChargerVehicules();
            ChargerHistorique();
        }

        private void ChargerVehicules()
        {
            try
            {
                _vehicules = _dbService.GetAllVehicules();
                CmbVehicule.ItemsSource = _vehicules;
                CmbVehicule.SelectedValuePath = "IdVehicule";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChargerHistorique()
        {
            try
            {
                DgHistorique.ItemsSource = _dbService.GetHistoriquePleins();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CmbVehicule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbVehicule.SelectedItem is Vehicule vehicule)
            {
                TxtCarburant.Text = vehicule.TypeCarburant;
            }
        }

        private void BtnValider_Click(object sender, RoutedEventArgs e)
        {
            if (CmbVehicule.SelectedItem == null)
            {
                MessageBox.Show("Veuillez selectionner un vehicule.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (DpDate.SelectedDate == null)
            {
                MessageBox.Show("Veuillez selectionner une date.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(TxtLitres.Text.Replace(",", "."), System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out decimal litres) || litres <= 0)
            {
                MessageBox.Show("Veuillez entrer un nombre de litres valide.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(TxtCout.Text.Replace(",", "."), System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out decimal cout) || cout <= 0)
            {
                MessageBox.Show("Veuillez entrer un cout valide.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var vehicule = (Vehicule)CmbVehicule.SelectedItem;
                _dbService.AjouterPlein(
                    vehicule.IdVehicule,
                    SessionUtilisateur.UtilisateurConnecte!.IdUtilisateur,
                    DpDate.SelectedDate.Value,
                    litres,
                    cout
                );

                MessageBox.Show("Plein enregistre avec succes.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

                TxtLitres.Clear();
                TxtCout.Clear();
                CmbVehicule.SelectedIndex = -1;
                TxtCarburant.Clear();
                ChargerHistorique();
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

            // Selectionner le vehicule
            foreach (Vehicule v in CmbVehicule.Items)
            {
                if (v.IdVehicule == (int)item.IdVehicule)
                {
                    CmbVehicule.SelectedItem = v;
                    break;
                }
            }

            DpDate.SelectedDate = (DateTime)item.Date;
            TxtLitres.Text = ((decimal)item.Litres).ToString();
            TxtCout.Text = ((decimal)item.Cout).ToString();
        }

        private void BtnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            dynamic item = button?.Tag!;
            if (item == null) return;

            var result = MessageBox.Show("Voulez-vous vraiment supprimer ce plein ?",
                "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _dbService.SupprimerSuivi((int)item.IdVehicule, (int)item.IdUtilisateur);
                    ChargerHistorique();
                    MessageBox.Show("Plein supprime.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
