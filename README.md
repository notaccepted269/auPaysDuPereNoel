# Au Pays du Père Noël

## Présentation

Ce projet propose une simulation textuelle de la logistique du Père Noël. Les lettres d’enfants sont dépilées, les lutins fabriquent les jouets, les nains les emballent, les elfes chargent les traîneaux et des entrepôts reçoivent les livraisons par continent. L’objectif est d’équilibrer la charge de travail et les coûts horaires jusqu’à ce que toutes les lettres soient traitées.

## Prérequis

- [.NET SDK 9.0](https://dotnet.microsoft.com/download) ou version supérieure compatible
- Un terminal capable d’afficher l’Unicode pour profiter des cadres et des emojis utilisés par l’interface console

## Démarrage rapide

1. Installer le SDK .NET 9.0.
2. Depuis la racine du dépôt (où se trouve `AuPaysDuPereNoel.csproj`), restaurer les dépendances :
   ```bash
   dotnet restore
   ```
3. Lancer la simulation :
   ```bash
   dotnet run
   ```
4. Saisir les paramètres demandés (nombre de lutins, nains, lettres par heure, etc.) pour initialiser une partie.

## Paramètres configurables

| Paramètre            | Description                                      |
| -------------------- | ------------------------------------------------ |
| `NbLutins`           | Lutins disponibles pour fabriquer les jouets.    |
| `NbNains`            | Nains disponibles pour emballer les cadeaux.     |
| `NbEnfants`          | Nombre total de lettres générées aléatoirement.  |
| `NbLettresParHeure`  | Capacités de lecture du Père Noël à chaque tour. |
| `NbJouetParTraineau` | Capacité des traîneaux (un par continent).       |

Une fois la simulation lancée, des menus permettent de modifier temporairement le nombre de lutins/nains actifs (avec délais de refroidissement de 12h et 24h) et de consulter les statistiques.

## Cycle d’une heure de simulation

Chaque appel à `Simulation.AvancerUneHeure()` effectue les étapes suivantes :

1. Le Père Noël lit jusqu’à `NbLettresParHeure` lettres et les place dans la file de fabrication.
2. Les lutins disponibles prennent des lettres et entament la fabrication (durée calculée selon l’âge).
3. Les lutins en cours de travail avancent d’une heure; les cadeaux terminés rejoignent la file d’emballage.
4. Les nains libres récupèrent des cadeaux à emballer et travaillent deux heures chacun.
5. Les cadeaux emballés sont routés vers la file continentale correspondante.
6. Les elfes chargent leurs traîneaux et partent en livraison dès que la capacité est atteinte.
7. Les traîneaux en route décrémentent leur temps de voyage; à l’arrivée les cadeaux sont stockés dans l’entrepôt du continent.
8. Les coûts horaires (salaires lutins/nains/elfes) sont cumulés pour produire un bilan.

Le menu principal offre aussi un raccourci pour avancer directement jusqu’au prochain matin (12 heures).

## Interface console

Le programme (`Program` dans `auPaysDuPereNoel.cs`) propose :

- **Menu d’initialisation** : saisie des paramètres de départ.
- **Menu principal** : avancer d’une heure, passer au jour suivant, afficher les indicateurs, gérer lutins/nains, visualiser les entrepôts ou afficher le bilan.
- **Menus de gestion** : activer/désactiver du personnel en respectant les délais minimaux.
- **Tableaux de bord** : `AfficherIndicateurs` et `AfficherBilan` détaillent l’état des files, des traîneaux et des coûts.

## Entités clés

- `Lettre` : encapsule l’enfant, son continent et détermine automatiquement le type de cadeau ainsi que la durée de fabrication.
- `Lutin` / `Nain` : employés avec statut (`EnTravail`, `EnAttente`, `EnRepos`) qui consomment les files de production.
- `Elfe` & `Traineau` : un elfe par continent, responsable du chargement et de la livraison vers l’entrepôt correspondant.
- `Entrepot` : stocke les cadeaux livrés par continent et renvoie des statistiques par type de cadeau.
- `Simulation` : orchestre l’ensemble du flux horaire, calcule les coûts et vérifie si toutes les lettres ont été traitées.

## Conseils et améliorations possibles

- Ajuster progressivement `NbLettresParHeure`, le nombre de lutins ou de nains pour éviter les goulots d’étranglement dans les files.
- Ajouter une persistance des résultats si vous souhaitez comparer plusieurs scénarios.
- Étendre `Lettre` pour charger des listes de lettres depuis un fichier CSV ou JSON plutôt que de les générer aléatoirement.
