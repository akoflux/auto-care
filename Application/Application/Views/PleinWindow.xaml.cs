using FleetManager.Models;
using FleetManager.Services;
using System.Windows;
using System.Windows.Controls;

namespace FleetManager.Views
{
    public partial class PleinWindow : Window
    {
        private readonly DatabaseService _dbService;
        private List<Vehicule> _vehicules;

        public PleinWindow()
        {
            InitializeComponent();
            _dbService = new DatabaseService();
            _vehicules = new List<Vehicule>();
            DpDate.SelectedDate = DateTime.Today;
            ChargerVehicules();
        }

        private void ChargerVehicules()
        {
            try
            {
                _vehicules = _dbService.GetAllVehicules();
                CmbVehicule.ItemsSource = _vehicules;
                CmbVehicule.DisplayMemberPath = "InfoComplete";
                CmbVehicule.SelectedValuePath = "IdVehicule";
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
