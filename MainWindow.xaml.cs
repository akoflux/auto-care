using System;
using System.Windows;
using System.Windows.Media;
using FleetManager.Database;

namespace FleetManager
{
	public partial class MainWindow : Window
	{
		private DatabaseConnection dbConnection;

		public MainWindow()
		{
			InitializeComponent();
		}

		// �v�nement au chargement de la fen�tre
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			TestDatabaseConnection();
		}

		// Tester la connexion � la base de donn�es
		private void TestDatabaseConnection()
		{
			try
			{
				dbConnection = DatabaseConnection.GetInstance();

				if (dbConnection.TestConnection())
				{
					// Connexion r�ussie - Indicateur vert
					ConnectionIndicator.Fill = new SolidColorBrush(Colors.LimeGreen);
					ConnectionStatus.Text = "Connect�";
				}
				else
				{
					// �chec de connexion - Indicateur rouge
					ConnectionIndicator.Fill = new SolidColorBrush(Colors.Red);
					ConnectionStatus.Text = "D�connect�";
					MessageBox.Show("Impossible de se connecter � la base de donn�es.\n" +
								  "V�rifiez que MySQL est d�marr� et que la base 'fleetmanager' existe.",
								  "Erreur de connexion",
								  MessageBoxButton.OK,
								  MessageBoxImage.Error);
				}
			}
			catch (Exception ex)
			{
				ConnectionIndicator.Fill = new SolidColorBrush(Colors.Red);
				ConnectionStatus.Text = "Erreur";
				MessageBox.Show($"Erreur lors du test de connexion :\n{ex.Message}",
							  "Erreur",
							  MessageBoxButton.OK,
							  MessageBoxImage.Error);
			}
		}

		private void BtnVehicules_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Module Gestion des V�hicules\n\n" +
						  "Fonctionnalit�s :\n" +
						  "� Ajouter un v�hicule\n" +
						  "� Modifier les informations\n" +
						  "� Supprimer un v�hicule\n" +
						  "� Consulter la liste compl�te",
						  "Gestion V�hicules",
						  MessageBoxButton.OK,
						  MessageBoxImage.Information);

			// TODO: Ouvrir la fen�tre de gestion des v�hicules
			// var vehiculesWindow = new VehiculesWindow();
			// vehiculesWindow.Show();
		}

		private void BtnSuivi_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Module Suivi Carburant & Kilom�trage\n\n" +
						  "Fonctionnalit�s :\n" +
						  "� Enregistrer une consommation\n" +
						  "� Suivre le kilom�trage\n" +
						  "� Historique des trajets\n" +
						  "� Calcul de consommation moyenne",
						  "Suivi Carburant",
						  MessageBoxButton.OK,
						  MessageBoxImage.Information);

			// TODO: Ouvrir la fen�tre de suivi
			// var suiviWindow = new SuiviWindow();
			// suiviWindow.Show();
		}

		private void BtnStatistiques_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Module Statistiques & Rapports\n\n" +
						  "Fonctionnalit�s :\n" +
						  "� Graphiques de consommation\n" +
						  "� Co�ts par v�hicule\n" +
						  "� Rapports mensuels\n" +
						  "� Analyse de performance",
						  "Statistiques",
						  MessageBoxButton.OK,
						  MessageBoxImage.Information);

			// TODO: Ouvrir la fen�tre de statistiques
			// var statsWindow = new StatistiquesWindow();
			// statsWindow.Show();
		}

		private void BtnUtilisateurs_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Module Gestion des Utilisateurs\n\n" +
						  "Fonctionnalit�s :\n" +
						  "� Ajouter un utilisateur\n" +
						  "� G�rer les r�les\n" +
						  "� Modifier les informations\n" +
						  "� Gestion des acc�s",
						  "Gestion Utilisateurs",
						  MessageBoxButton.OK,
						  MessageBoxImage.Information);

			// TODO: Ouvrir la fen�tre de gestion des utilisateurs
			// var utilisateursWindow = new UtilisateursWindow();
			// utilisateursWindow.Show();
		}
	}
}