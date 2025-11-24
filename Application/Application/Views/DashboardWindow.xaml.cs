using FleetManager.Services;
using System.Windows;

namespace FleetManager.Views
{
    public partial class DashboardWindow : Window
    {
        private readonly DatabaseService _dbService;

        public DashboardWindow()
        {
            InitializeComponent();
            _dbService = new DatabaseService();
            ChargerStatistiques();
        }

        private void ChargerStatistiques()
        {
            try
            {
                // Statistiques generales
                TxtTotalVehicules.Text = _dbService.GetNombreTotalVehicules().ToString();
                TxtTotalKm.Text = $"{_dbService.GetTotalKmParcourus():N0} km";
                TxtTotalCout.Text = $"{_dbService.GetTotalCoutCarburant():N2} EUR";
                TxtTotalLitres.Text = $"{_dbService.GetTotalLitresConsommes():N2} L";

                // Repartition par carburant
                TxtEssence.Text = _dbService.GetNombreVehiculesParCarburant("Essence").ToString();
                TxtDiesel.Text = _dbService.GetNombreVehiculesParCarburant("Diesel").ToString();
                TxtHybride.Text = _dbService.GetNombreVehiculesParCarburant("Hybride").ToString();
                TxtElectrique.Text = _dbService.GetNombreVehiculesParCarburant("Electrique").ToString();

                // Liste des vehicules
                DgVehicules.ItemsSource = _dbService.GetAllVehicules();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des statistiques : {ex.Message}",
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnActualiser_Click(object sender, RoutedEventArgs e)
        {
            ChargerStatistiques();
        }
    }
}
