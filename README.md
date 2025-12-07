# FleetManager - Auto Care

> Application de gestion de flotte automobile développée en C# WPF dans le cadre du BTS SIO (Services Informatiques aux Organisations)

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![C#](https://img.shields.io/badge/C%23-12-239120?logo=csharp)
![WPF](https://img.shields.io/badge/WPF-Windows-0078D4?logo=windows)
![MySQL](https://img.shields.io/badge/MySQL-8.0-4479A1?logo=mysql&logoColor=white)

---

## Table des matières

1. [Présentation du projet](#présentation-du-projet)
2. [Contexte BTS SIO](#contexte-bts-sio)
3. [Fonctionnalités](#fonctionnalités)
4. [Technologies utilisées](#technologies-utilisées)
5. [Architecture](#architecture)
6. [Installation](#installation)
7. [Utilisation](#utilisation)
8. [Base de données](#base-de-données)
9. [Sécurité](#sécurité)
10. [Captures d'écran](#captures-décran)
11. [Difficultés rencontrées](#difficultés-rencontrées)
12. [Perspectives d'évolution](#perspectives-dévolution)
13. [Auteur](#auteur)

---

## Présentation du projet

**FleetManager** (Auto Care) est une application desktop de gestion de flotte automobile permettant de :
- Suivre l'ensemble des véhicules d'une entreprise
- Enregistrer les pleins de carburant et calculer la consommation
- Tracer les trajets effectués par chaque véhicule
- Gérer les utilisateurs avec un système de rôles
- Visualiser des statistiques et indicateurs de performance

L'application s'adresse aux entreprises souhaitant optimiser la gestion de leur parc automobile et réduire leurs coûts d'exploitation.

---

## Contexte BTS SIO

### Projet réalisé dans le cadre du BTS SIO option SLAM

**Formation** : BTS Services Informatiques aux Organisations - Option SLAM (Solutions Logicielles et Applications Métiers)

**Objectifs pédagogiques** :
- Concevoir et développer une solution applicative complète
- Mettre en œuvre des bonnes pratiques de développement
- Gérer une base de données relationnelle
- Implémenter un système d'authentification sécurisé
- Créer une interface utilisateur ergonomique

**Compétences mobilisées** :
- Développement d'applications desktop en C# avec WPF
- Conception et gestion de bases de données MySQL
- Architecture logicielle en couches
- Sécurisation des données (hachage BCrypt)
- Tests et validation des fonctionnalités
- Documentation technique

---

## Fonctionnalités

### Authentification et Gestion des Utilisateurs

- **Système de connexion sécurisé** avec email et mot de passe
- **Trois niveaux de rôles** :
  - **Super Admin** : Gestion complète des administrateurs et utilisateurs
  - **Admin** : Gestion des employés uniquement
  - **Employé** : Consultation des données uniquement
- **Hachage des mots de passe** avec BCrypt (12 rounds)
- **Migration automatique** des anciens mots de passe en clair

### Dashboard - Tableau de Bord

- **Statistiques globales** :
  - Nombre total de véhicules
  - Kilométrage total parcouru
  - Coût total du carburant
  - Consommation moyenne (L/100km)
- **Répartition des véhicules par type de carburant** (Essence, Diesel, Hybride, Électrique)
- **Alertes de consommation élevée** (véhicules dépassant 8 L/100km)
- **Statistiques mensuelles** sur 6 mois (évolution de la consommation)

### Gestion des Véhicules

- **CRUD complet** : Ajout, modification, suppression de véhicules
- **Informations trackées** :
  - Immatriculation (unique)
  - Marque et modèle
  - Type de carburant
  - Date d'achat
- **Recherche et pagination** (20 véhicules par page)
- **Validation** : Détection des immatriculations en double

### Gestion des Pleins de Carburant

- **Enregistrement détaillé** :
  - Sélection du véhicule
  - Date du plein
  - Litres consommés
  - Coût total
- **Historique complet** avec filtrage par véhicule
- **Suppression de pleins** avec confirmation

### Suivi des Trajets

- **Enregistrement de trajets** :
  - Sélection du véhicule
  - Date du trajet
  - Kilométrage départ et arrivée
  - Calcul automatique de la distance
- **Récupération automatique** du dernier kilométrage
- **Historique des trajets** avec filtrage
- **Validation** : Vérification que KmArrivée > KmDépart

### Gestion des Utilisateurs (Admins uniquement)

- **CRUD utilisateurs** avec contrôle par rôle
- **Champs** : Nom, Prénom, Email (unique), Mot de passe, Rôle
- **Validation** : Format email, unicité, force du mot de passe
- **Restrictions** : Les admins ne peuvent pas gérer d'autres admins (sauf Super Admin)

---

## Technologies utilisées

### Frontend

| Technologie | Version | Usage |
|------------|---------|-------|
| **WPF** | .NET 8.0 | Framework d'interface utilisateur |
| **XAML** | - | Langage de définition d'interface |
| **C#** | 12 | Langage de programmation |

### Backend

| Technologie | Version | Usage |
|------------|---------|-------|
| **.NET** | 8.0 | Framework applicatif |
| **MySql.Data** | 9.5.0 | Connecteur MySQL |
| **BCrypt.Net-Next** | 4.0.3 | Hachage de mots de passe |
| **PamelloV7.Core** | 0.1.2 | Bibliothèque utilitaire |

### Base de données

- **MySQL** 8.0+ (Serveur local)
- **Schéma** : `fleetmanager`

---

## Architecture

### Structure du projet

```
FleetManager/
├── Application/
│   └── Application/
│       ├── Models/              # Modèles de données
│       │   └── Models.cs        # Utilisateur, Vehicule, Suivi, Enums
│       ├── Views/               # Pages et fenêtres XAML
│       │   ├── LoginWindow.xaml
│       │   ├── MainWindow.xaml
│       │   ├── DashboardPage.xaml
│       │   ├── VehiculesPage.xaml
│       │   ├── PleinPage.xaml
│       │   ├── TrajetPage.xaml
│       │   └── UtilisateursPage.xaml
│       ├── Services/            # Couche d'accès aux données
│       │   └── DatabaseService.cs
│       ├── Helpers/             # Classes utilitaires
│       │   ├── PasswordHelper.cs
│       │   ├── ValidationHelper.cs
│       │   ├── NotificationHelper.cs
│       │   └── SessionUtilisateur.cs
│       ├── Controls/            # Composants réutilisables
│       │   ├── ToastNotification.xaml
│       │   └── LoadingSpinner.xaml
│       ├── DataBase/            # Gestion de la connexion
│       │   └── DatabaseConnection.cs
│       ├── Images/              # Ressources graphiques
│       ├── App.xaml             # Point d'entrée
│       └── Application.csproj   # Configuration projet
└── README.md
```

### Patterns et principes

- **Architecture en couches** : Séparation UI / Services / Data
- **Singleton** : Gestion de la connexion base de données
- **Service Layer** : Centralisation de la logique métier dans DatabaseService
- **Static Session** : SessionUtilisateur pour la gestion de l'utilisateur connecté
- **Validation** : Helpers dédiés à la validation des entrées
- **Notifications** : Système de toasts réutilisables

### Flux applicatif

```
App.xaml
    ↓
LoginWindow (Authentification)
    ↓
MainWindow (Navigation principale)
    ├── DashboardPage (Tableau de bord)
    ├── VehiculesPage (Gestion véhicules)
    ├── PleinPage (Pleins carburant)
    ├── TrajetPage (Trajets)
    └── UtilisateursPage (Gestion utilisateurs - Admins)
```

---

## Installation

### Prérequis

- **Windows 10/11** (64 bits)
- **.NET 8.0 SDK** : [Télécharger ici](https://dotnet.microsoft.com/download/dotnet/8.0)
- **MySQL Server 8.0+** : [Télécharger ici](https://dev.mysql.com/downloads/mysql/)
- **Visual Studio 2022** (recommandé) avec la charge de travail "Développement .NET Desktop"

### Étapes d'installation

#### 1. Cloner le dépôt

```bash
git clone https://github.com/votre-username/fleetmanager.git
cd fleetmanager
```

#### 2. Configurer la base de données

**Créer la base de données MySQL** :

```sql
CREATE DATABASE fleetmanager CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE fleetmanager;

-- Table des utilisateurs
CREATE TABLE utilisateur (
    IdUtilisateur INT AUTO_INCREMENT PRIMARY KEY,
    Nom VARCHAR(100) NOT NULL,
    Prenom VARCHAR(100) NOT NULL,
    Email VARCHAR(255) NOT NULL UNIQUE,
    Password VARCHAR(255) NOT NULL,
    IdRole INT NOT NULL,
    INDEX idx_email (Email),
    INDEX idx_role (IdRole)
) ENGINE=InnoDB;

-- Table des véhicules
CREATE TABLE vehicule (
    IdVehicule INT AUTO_INCREMENT PRIMARY KEY,
    Immatriculation VARCHAR(20) NOT NULL UNIQUE,
    Marque VARCHAR(100) NOT NULL,
    Modele VARCHAR(100) NOT NULL,
    TypeCarburant VARCHAR(50) NOT NULL,
    DateAchat DATE NOT NULL,
    INDEX idx_immat (Immatriculation)
) ENGINE=InnoDB;

-- Table des suivis (pleins + trajets)
CREATE TABLE suivis (
    IdSuivi INT AUTO_INCREMENT PRIMARY KEY,
    IdUtilisateur INT NOT NULL,
    IdVehicule INT NOT NULL,
    Date DATETIME NOT NULL,
    Consommation DECIMAL(10,2) DEFAULT 0,
    Cout DECIMAL(10,2) DEFAULT 0,
    KmDepart INT DEFAULT 0,
    KmArrivee INT DEFAULT 0,
    FOREIGN KEY (IdUtilisateur) REFERENCES utilisateur(IdUtilisateur) ON DELETE CASCADE,
    FOREIGN KEY (IdVehicule) REFERENCES vehicule(IdVehicule) ON DELETE CASCADE,
    INDEX idx_vehicule (IdVehicule),
    INDEX idx_date (Date)
) ENGINE=InnoDB;

-- Insertion d'un Super Admin par défaut
INSERT INTO utilisateur (Nom, Prenom, Email, Password, IdRole)
VALUES ('Admin', 'Super', 'admin@autocare.fr', '$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewY5GyYPdLY9.flm', 1);
-- Mot de passe : Admin123!
```

**Configurer les identifiants** dans `DatabaseConnection.cs` :

```csharp
private string _connectionString = "Server=localhost;Database=fleetmanager;Uid=root;Pwd=votre_mot_de_passe;";
```

#### 3. Restaurer les dépendances

```bash
cd Application/Application
dotnet restore
```

#### 4. Compiler et exécuter

**Avec Visual Studio** :
- Ouvrir `Application.sln`
- Définir `Application` comme projet de démarrage
- Appuyer sur F5

**Avec la CLI** :
```bash
dotnet build
dotnet run
```

### Compte par défaut

- **Email** : `admin@client.fr`
- **Mot de passe** : `password123`
- **Rôle** : Super Admin

---

## Utilisation

### Connexion

1. Lancer l'application
2. Entrer l'email et le mot de passe
3. Cliquer sur "Se connecter"

### Navigation

Utiliser le **menu latéral gauche** pour accéder aux différentes sections :
- **Tableau de bord** : Vue d'ensemble des statistiques
- **Véhicules** : Gestion du parc automobile
- **Pleins** : Enregistrement des pleins de carburant
- **Trajets** : Suivi des trajets effectués
- **Utilisateurs** : Gestion des comptes (Admins uniquement)

### Ajouter un véhicule

1. Aller dans "Véhicules"
2. Cliquer sur "Ajouter"
3. Remplir le formulaire :
   - Immatriculation (ex: AB-123-CD)
   - Marque (ex: Renault)
   - Modèle (ex: Clio)
   - Type de carburant
   - Date d'achat
4. Cliquer sur "Enregistrer"

### Enregistrer un plein

1. Aller dans "Pleins"
2. Sélectionner un véhicule
3. Entrer la date, les litres et le coût
4. Cliquer sur "Ajouter"

### Enregistrer un trajet

1. Aller dans "Trajets"
2. Sélectionner un véhicule
3. Entrer la date et les kilométrages départ/arrivée
4. Cliquer sur "Ajouter"

### Gérer les utilisateurs (Admins)

1. Aller dans "Utilisateurs"
2. Cliquer sur "Ajouter"
3. Remplir les informations et choisir un rôle
4. Cliquer sur "Enregistrer"

---

## Base de données

### Schéma relationnel

```
┌─────────────────┐        ┌─────────────────┐
│  utilisateur    │        │    vehicule     │
├─────────────────┤        ├─────────────────┤
│ IdUtilisateur PK│◄───┐   │ IdVehicule PK   │◄───┐
│ Nom             │    │   │ Immatriculation │    │
│ Prenom          │    │   │ Marque          │    │
│ Email UNIQUE    │    │   │ Modele          │    │
│ Password        │    │   │ TypeCarburant   │    │
│ IdRole          │    │   │ DateAchat       │    │
└─────────────────┘    │   └─────────────────┘    │
                       │                           │
                       │   ┌─────────────────┐    │
                       │   │     suivis      │    │
                       │   ├─────────────────┤    │
                       └───┤ IdUtilisateur FK│    │
                           ├─────────────────┤    │
                           │ IdVehicule FK   ├────┘
                           │ Date            │
                           │ Consommation    │
                           │ Cout            │
                           │ KmDepart        │
                           │ KmArrivee       │
                           └─────────────────┘
```

### Modèles de données

**Utilisateur**
- `IdUtilisateur` : INT (PK, AUTO_INCREMENT)
- `Nom` : VARCHAR(100)
- `Prenom` : VARCHAR(100)
- `Email` : VARCHAR(255) UNIQUE
- `Password` : VARCHAR(255) (BCrypt hash)
- `IdRole` : INT (1=SuperAdmin, 2=Admin, 3=Employé)

**Vehicule**
- `IdVehicule` : INT (PK, AUTO_INCREMENT)
- `Immatriculation` : VARCHAR(20) UNIQUE
- `Marque` : VARCHAR(100)
- `Modele` : VARCHAR(100)
- `TypeCarburant` : VARCHAR(50)
- `DateAchat` : DATE

**Suivi** (Pleins + Trajets fusionnés)
- `IdSuivi` : INT (PK, AUTO_INCREMENT)
- `IdUtilisateur` : INT (FK)
- `IdVehicule` : INT (FK)
- `Date` : DATETIME
- `Consommation` : DECIMAL(10,2) - Litres (plein uniquement)
- `Cout` : DECIMAL(10,2) - Euros (plein uniquement)
- `KmDepart` : INT (trajet uniquement)
- `KmArrivee` : INT (trajet uniquement)

### Requêtes principales

**Consommation moyenne (L/100km)** :
```sql
SELECT (SUM(Consommation) / (SUM(KmArrivee - KmDepart) / 100)) AS ConsommationMoyenne
FROM suivis
WHERE IdVehicule = ?
```

**Statistiques mensuelles** :
```sql
SELECT
    DATE_FORMAT(Date, '%Y-%m') AS Mois,
    SUM(Consommation) AS TotalLitres,
    SUM(Cout) AS TotalCout,
    SUM(KmArrivee - KmDepart) AS TotalKm
FROM suivis
WHERE Date >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH)
GROUP BY Mois
ORDER BY Mois DESC
```

---

## Sécurité

### Authentification

- **Hachage BCrypt** avec work factor de 12 rounds
- **Salt automatique** généré par BCrypt
- **Migration transparente** des mots de passe legacy
- **Validation côté client** avant envoi

### Protection contre les injections SQL

- **Requêtes paramétrées** avec `MySqlParameter`
- **Échappement automatique** des entrées utilisateur
- Exemple :
  ```csharp
  cmd.CommandText = "SELECT * FROM utilisateur WHERE Email = @Email";
  cmd.Parameters.AddWithValue("@Email", email);
  ```

### Gestion des sessions

- **SessionUtilisateur statique** stockant l'utilisateur connecté
- **Contrôle d'accès basé sur les rôles** (RBAC)
- **Nettoyage de session** à la déconnexion

### Validation des données

- **Format email** validé (regex)
- **Immatriculation** format français (AA-123-AA)
- **Unicité** email et immatriculation côté client et base
- **Contraintes** : KmArrivée > KmDépart, litres > 0, etc.

### Points d'amélioration identifiés

- [ ] Configuration externalisée (fichier `appsettings.json`)
- [ ] Chiffrement de la chaîne de connexion
- [ ] Timeout de session automatique
- [ ] Journalisation des accès (logs)
- [ ] HTTPS pour une éventuelle API future

---

## Captures d'écran

### Fenêtre de connexion
![LoginWindow](Images/login_screenshot.png)

### Tableau de bord
![Dashboard](Images/dashboard_screenshot.png)

### Gestion des véhicules
![Vehicules](Images/vehicules_screenshot.png)

### Enregistrement d'un plein
![Plein](Images/plein_screenshot.png)

---

## Difficultés rencontrées

### Technique

1. **Migration des mots de passe** : Gestion de l'ancien système en clair vers BCrypt
   - Solution : Détection automatique et rehachage à la connexion

2. **Calcul de la consommation** : Division par zéro lorsque aucun trajet
   - Solution : Vérification des valeurs NULL et ajout de conditions

3. **Synchronisation Pleins/Trajets** : Table unique `suivis` pour deux types d'enregistrements
   - Solution : Champs optionnels avec validation métier

4. **DataGrid WPF** : Rafraîchissement automatique après CRUD
   - Solution : Rechargement manuel via `LoadData()` après chaque opération

### Fonctionnel

1. **Gestion des rôles** : Empêcher un Admin de modifier un SuperAdmin
   - Solution : Vérification côté service avec `SessionUtilisateur.EstSuperAdmin`

2. **Pagination** : Limitation à 20 éléments par page
   - Solution : Requêtes SQL avec `LIMIT` et `OFFSET`

3. **Validation temps réel** : Feedback utilisateur immédiat
   - Solution : Événements `TextChanged` et affichage de messages d'erreur

---

## Perspectives d'évolution

### Fonctionnalités

- [ ] **Maintenance préventive** : Rappels pour révisions, contrôles techniques
- [ ] **Assurances** : Suivi des contrats et renouvellements
- [ ] **Conducteurs** : Attribution de véhicules à des employés
- [ ] **Rapports PDF** : Export des statistiques et historiques
- [ ] **Graphiques avancés** : Charts de consommation par véhicule
- [ ] **Géolocalisation** : Intégration d'une carte pour les trajets
- [ ] **Alertes push** : Notifications pour événements critiques
- [ ] **Multi-tenant** : Gestion de plusieurs entreprises

### Technique

- [ ] **Architecture MVVM** : Refactorisation complète
- [ ] **Entity Framework Core** : Remplacement de MySql.Data
- [ ] **API REST** : Backend séparé pour évolution vers web/mobile
- [ ] **Tests unitaires** : Couverture avec xUnit
- [ ] **CI/CD** : Pipeline GitHub Actions
- [ ] **Conteneurisation** : Docker pour base de données
- [ ] **Logging avancé** : Serilog ou NLog

### UX/UI

- [ ] **Mode clair/sombre** : Thème configurable
- [ ] **Localisation** : Support multilingue (EN, ES, etc.)
- [ ] **Accessibilité** : Conformité WCAG
- [ ] **Responsive** : Adaptation pour tablettes
- [ ] **Shortcuts clavier** : Navigation au clavier

---

## Auteur

**Projet BTS SIO - Option SLAM**

Développé par : Mathis
Formation : BTS Services Informatiques aux Organisations
Année : 2024-2025
Établissement : CFA INSTA

### Contact

- Email : mathisd94550@gmail.com
- GitHub : [akoflux](https://github.com/akoflux)

---

## Licence

Ce projet est un projet pédagogique réalisé dans le cadre du BTS SIO.
Il est fourni à titre d'exemple et n'est pas destiné à un usage en production sans adaptation.

---

## Remerciements

- Mon formateur pour son accompagnement
- L'équipe pédagogique du BTS SIO
- La communauté .NET et WPF pour les ressources en ligne
- [BCrypt.Net](https://github.com/BcryptNet/bcrypt.net) pour la bibliothèque de hachage
- [MySQL](https://www.mysql.com/) pour le SGBD

---

*Dernière mise à jour : Décembre 2025*

