using FleetManager.Models;
using FleetManager.Services;
using System.Windows;
using System.Windows.Controls;

namespace FleetManager.Views
{
    public partial class TrajetPage : Page
    {
        private readonly DatabaseService _dbService;
        private List<Vehicule> _vehicules;

        public TrajetPage()
        {
            InitializeComponent();
            _dbService = new DatabaseService();
            _vehicules = new List<Vehicule>();
            DpDateDepart.SelectedDate = DateTime.Today;
            DpDateArrivee.SelectedDate = DateTime.Today;
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
                DgHistorique.ItemsSource = _dbService.GetHistoriqueTrajets();
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
                int dernierKm = _dbService.GetDernierKm(vehicule.IdVehicule);
                TxtKmDepart.Text = dernierKm.ToString();
            }
        }

        private void BtnValider_Click(object sender, RoutedEventArgs e)
        {
            if (CmbVehicule.SelectedItem == null)
            {
                MessageBox.Show("Veuillez selectionner un vehicule.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (DpDateDepart.SelectedDate == null || DpDateArrivee.SelectedDate == null)
            {
                MessageBox.Show("Veuillez selectionner les dates.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (DpDateArrivee.SelectedDate < DpDateDepart.SelectedDate)
            {
                MessageBox.Show("La date d'arrivee doit etre superieure ou egale a la date de depart.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(TxtKmArrivee.Text, out int kmArrivee) || kmArrivee <= 0)
            {
                MessageBox.Show("Veuillez entrer un kilometrage valide.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int kmDepart = int.Parse(TxtKmDepart.Text);
            if (kmArrivee <= kmDepart)
            {
                MessageBox.Show("Le kilometrage d'arrivee doit etre superieur au kilometrage de depart.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var vehicule = (Vehicule)CmbVehicule.SelectedItem;
                _dbService.AjouterTrajet(
                    vehicule.IdVehicule,
                    SessionUtilisateur.UtilisateurConnecte!.IdUtilisateur,
                    DpDateDepart.SelectedDate.Value,
                    DpDateArrivee.SelectedDate.Value,
                    kmDepart,
                    kmArrivee
                );

                MessageBox.Show($"Trajet enregistre avec succes.\nDistance parcourue : {kmArrivee - kmDepart} km",
                    "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

                TxtKmArrivee.Clear();
                CmbVehicule.SelectedIndex = -1;
                TxtKmDepart.Clear();
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

            DpDateDepart.SelectedDate = (DateTime)item.Date;
            DpDateArrivee.SelectedDate = (DateTime)item.Date;
            TxtKmDepart.Text = ((int)item.KmDepart).ToString();
            TxtKmArrivee.Text = ((int)item.KmArrivee).ToString();
        }

        private void BtnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            dynamic item = button?.Tag!;
            if (item == null) return;

            var result = MessageBox.Show("Voulez-vous vraiment supprimer ce trajet ?",
                "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _dbService.SupprimerSuivi((int)item.IdVehicule, (int)item.IdUtilisateur);
                    ChargerHistorique();
                    MessageBox.Show("Trajet supprime.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
