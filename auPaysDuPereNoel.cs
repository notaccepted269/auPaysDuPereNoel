using System;

namespace AuPaysDuPereNoel
{
    // ============= ENUMS ET STRUCTURES =============
    public enum TypeCadeau
    {
        Nounours,
        Tricycle,
        Jumelles,
        AbonnementGeekyJunior,
        Ordinateur
    }

    public enum Continent
    {
        Afrique,
        Amerique,
        Asie,
        Europe,
        Oceanie
    }

    public enum StatutEmploye
    {
        EnTravail,
        EnAttente,
        EnRepos
    }

    // ============= CLASSE LETTRE =============

        public class Lettre
    {
        public string NomEnfant { get; set; }
        public string Adresse { get; set; }
        public Continent Continent { get; set; }
        public int Age { get; set; }

        public Lettre(string nom, string adresse, Continent continent, int age)
        {
            NomEnfant = nom;
            Adresse = adresse;
            Continent = continent;
            Age = age;
        }

        // Méthodes statiques utilitaires
        public static TypeCadeau DeterminerTypeCadeau(int age)
        {
            if (age >= 0 && age <= 2) return TypeCadeau.Nounours;
            if (age >= 3 && age <= 5) return TypeCadeau.Tricycle;
            if (age >= 6 && age <= 10) return TypeCadeau.Jumelles;
            if (age >= 11 && age <= 15) return TypeCadeau.AbonnementGeekyJunior;
            if (age >= 16 && age <= 18) return TypeCadeau.Ordinateur;

            throw new ArgumentOutOfRangeException(nameof(age), "Age non valide");
        }

        public static int ObtenirDureeFabrication(int age)
        {
            if (age >= 0 && age <= 2) return 3;
            if (age >= 3 && age <= 5) return 4;
            if (age >= 6 && age <= 10) return 6;
            if (age >= 11 && age <= 15) return 1;
            if (age >= 16 && age <= 18) return 10;

            throw new ArgumentOutOfRangeException(nameof(age), "Âge non supporté.");
        }

        // Propriétés d'instance pour faciliter l'accès
        public TypeCadeau TypeCadeau
        {
            get
            {
                return DeterminerTypeCadeau(Age);
            }
        }

        public int DureeFabrication
        {
            get
            {
                return ObtenirDureeFabrication(Age);
            }
        }
    }

    // ============= CLASSE LUTIN =============

    public class Lutin
    {
        public int Id { get; set; }
        public StatutEmploye Statut { get; set; }
        public Lettre LettreEnCours { get; set; }
        public int HeuresRestantes { get; set; }

        public Lutin(int id)
        {
            Id = id;
            Statut = StatutEmploye.EnAttente;
            LettreEnCours = null;
            HeuresRestantes = 0;

        }

        public void CommencerFabrication(Lettre lettre)
        {
            LettreEnCours = lettre;
            Statut = StatutEmploye.EnTravail;
            HeuresRestantes = lettre.DureeFabrication;
        }

        public Lettre Travailler()
        {
            if (Statut != StatutEmploye.EnTravail || LettreEnCours == null)
                return null;

            HeuresRestantes--;

            if (HeuresRestantes <= 0)
            {
                Lettre lettreTerminee = LettreEnCours;
                LettreEnCours = null;
                Statut = StatutEmploye.EnAttente;
                return lettreTerminee;
            }

            return null;
        }
    }

    // ============= CLASSE NAIN =============

        public class Nain
    {
        private const int DureeEmballageHeures = 2; // durée fixe pour emballer un cadeau
        public int Id { get; set; }
        public StatutEmploye Statut { get; set; }
        public Lettre LettreEnCours { get; set; }
        public int HeuresRestantes { get; set; }

        public Nain(int id)
        {
            Id = id;
            Statut = StatutEmploye.EnAttente;
            LettreEnCours = null;
            HeuresRestantes = 0;
        }

        public void CommencerEmballage(Lettre lettre)
        {
            LettreEnCours = lettre;
            Statut = StatutEmploye.EnTravail;
            HeuresRestantes = DureeEmballageHeures;
        }

        public Lettre? Travailler()
        {
            if (Statut != StatutEmploye.EnTravail || LettreEnCours == null)
                return null;

            HeuresRestantes--;

            if (HeuresRestantes <= 0)
            {
                Lettre lettreTerminee = LettreEnCours;
                LettreEnCours = null;
                Statut = StatutEmploye.EnAttente;
                return lettreTerminee;
            }

            return null;
        }
    }

    // ============= CLASSE TRAINEAU =============

    public class Traineau
    {
        public Continent Continent { get; set; }
        public Stack<Lettre> Lettres { get; set; }
        public int CapaciteMax { get; set; }
        public bool EnVoyage { get; set; }
        public int HeuresVoyageRestantes { get; set; }

        public Traineau(Continent continent, int capacite)
        {
            Continent = continent;
            CapaciteMax = capacite;
            Lettres = new Stack<Lettre>();
            EnVoyage = false;
            HeuresVoyageRestantes = 0;
        }

        public bool PeutCharger()   //Est-ce que l'on peut charger le traineau ?
        {
            return !EnVoyage && Lettres.Count < CapaciteMax;
        }

        public void ChargerLettre(Lettre lettre)    // charger le traineau si c'est possible
        {
            if (PeutCharger())
            {
                Lettres.Push(lettre);
            }
        }

        public bool EstPlein()  // Le traineau est-il plein ?
        {
            return Lettres.Count >= CapaciteMax;
        }

        public void PartirEnVoyage()    // Le départ du traineau.
        {
            if (Lettres.Count > 0)
            {
                EnVoyage = true;
                HeuresVoyageRestantes = 6;
            }
        }

        public List<Lettre> AvancerVoyage()
        {
            if (!EnVoyage) return null;

            HeuresVoyageRestantes--;

            if (HeuresVoyageRestantes <= 0)
            {
                // Retour de voyage, décharger les lettres
                List<Lettre> lettresLivrees = Lettres.ToList(); // Transforme une pile en Liste
                Lettres.Clear();
                EnVoyage = false;
                return lettresLivrees;
            }

            return null;
        }
    }
    // ============= CLASSE ELFE =============

    public class Elfe
    {
        public int Id { get; set; }
        public Continent Continent { get; set; }
        public Traineau Traineau { get; set; }

        public Elfe(int id, Continent continent, int capaciteTraineau)
        {
            Id = id;
            Continent = continent;
            Traineau = new Traineau(continent, capaciteTraineau);
        }

        public bool PeutRecevoirLettre()
        {
            return Traineau.PeutCharger();
        }

        public void RecevoirLettre(Lettre lettre)
        {
            Traineau.ChargerLettre(lettre);

            // Si le traîneau est plein, partir en voyage
            if (Traineau.EstPlein())
            {
                Traineau.PartirEnVoyage();
            }
        }
    }

    // ============= CLASSE ENTREPOT =============

    public class Entrepot
    {
        public Continent Continent { get; set; }
        public Stack<Lettre> Lettres { get; set; }

        public Entrepot(Continent continent)
        {
            Continent = continent;
            Lettres = new Stack<Lettre>();
        }

        public void AjouterLettres(List<Lettre> lettres)
        {
            foreach (Lettre lettre in lettres)
            {
                Lettres.Push(lettre);
            }
        }

        public int[] GetStatistiques()
        {
            // Tableau indexé par TypeCadeau (0=Nounours, 1=Tricycle, etc.)
            int[] stats = new int[5];

            foreach (Lettre lettre in Lettres)
            {
                stats[(int)lettre.TypeCadeau]++;
            }

            return stats;
        }
    }

    // ============= CLASSE SIMULATION =============

    public class Simulation // La classe qui gère la simulation.
    {
        // Paramètres configurables
        public int NbLutins { get; set; }
        public int NbNains { get; set; }
        public int NbEnfants { get; set; }
        public int NbLettresParHeure { get; set; }
        public int NbJouetParTraineau { get; set; }

        // Personnel
        public List<Lutin> Lutins { get; set; }
        public List<Nain> Nains { get; set; }
        public List<Elfe> Elfes { get; set; }

        // Files et piles
        public Stack<Lettre> BureauPereNoel { get; set; }
        public Queue<Lettre> FileAttenteFabrication { get; set; }
        public Queue<Lettre> FileAttenteEmballage { get; set; }
        public Queue<Lettre>[] FilesAttenteContinents { get; set; }
        // Entrepôts (un par continent, indexé par l'enum Continent)
        public Entrepot[] Entrepots { get; set; }

        // Temps et coûts
        public int HeureActuelle { get; set; }
        public int JourActuel { get; set; }
        public double CoutTotal { get; set; }
        public List<double> CoutsParHeure { get; set; }

        // Gestion du personnel
        public int DerniereModificationLutins { get; set; }
        public int DerniereModificationNains { get; set; }

        // Générateur aléatoire
        private Random random;

        // La mise en route de la simulation avec la mise à jour de tous les paramètres.
        public Simulation(int nbLutins, int nbNains, int nbEnfants, int nbLettresParHeure, int nbJouetParTraineau)
        {
            NbLutins = nbLutins;
            NbNains = nbNains;
            NbEnfants = nbEnfants;
            NbLettresParHeure = nbLettresParHeure;
            NbJouetParTraineau = nbJouetParTraineau;

            random = new Random();
            HeureActuelle = 0;
            JourActuel = 1;
            CoutTotal = 0;
            CoutsParHeure = new List<double>();
            DerniereModificationLutins = -12;   // Pour bien gérer le fait qu'on ne peut recruter des lutins pendant 12 heures si on en a mis en repos.
            DerniereModificationNains = -24;    // Pour bien gérer le fait qu'on ne peut recruter des nains pendant 12 heures si on en a mis en repos.

            // Initialiser les lutins
            Lutins = new List<Lutin>();
            for (int i = 0; i < nbLutins; i++)
            {
                Lutins.Add(new Lutin(i + 1));
            }

            // Initialiser les nains
            Nains = new List<Nain>();
            for (int i = 0; i < nbNains; i++)
            {
                Nains.Add(new Nain(i + 1));
            }

            // Initialiser les elfes (5, un par continent)
            Elfes = new List<Elfe>();
            int elfId = 1;
            foreach (Continent continent in Enum.GetValues(typeof(Continent)))
            {
                Elfes.Add(new Elfe(elfId++, continent, nbJouetParTraineau));
            }

            // Initialiser les structures de données
            BureauPereNoel = new Stack<Lettre>();
            FileAttenteFabrication = new Queue<Lettre>();
            FileAttenteEmballage = new Queue<Lettre>();

            FilesAttenteContinents = new Queue<Lettre>[5];
            for (int i = 0; i < 5; i++)
            {
                FilesAttenteContinents[i] = new Queue<Lettre>();
            }

            Entrepots = new Entrepot[5];
            for (int i = 0; i < 5; i++)
            {
                Entrepots[i] = new Entrepot((Continent)i);
            }

            // Générer toutes les lettres
            GenererLettres();
        }

        private void GenererLettres()
        {
            string[] prenoms = { "Alice", "Bob", "Charlie", "Diana", "Emma", "Frank", "Grace", "Hugo",
                               "Iris", "Jack", "Kate", "Leo", "Marie", "Nathan", "Olivia", "Paul" };
            string[] villes = { "Paris", "Londres", "Tokyo", "New York", "Sydney", "Le Caire",
                              "Berlin", "Rome", "Pékin", "Rio", "Moscou", "Toronto" };

            List<Lettre> toutesLesLettres = new List<Lettre>();

            for (int i = 0; i < NbEnfants; i++)
            {
                string nom = prenoms[random.Next(prenoms.Length)] + i;
                string adresse = villes[random.Next(villes.Length)];
                Continent continent = (Continent)random.Next(5);
                int age = random.Next(0, 19);

                toutesLesLettres.Add(new Lettre(nom, adresse, continent, age));
            }

            // Mélanger les lettres (algorithme Fisher-Yates)
            for (int i = toutesLesLettres.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                Lettre temp = toutesLesLettres[i];
                toutesLesLettres[i] = toutesLesLettres[j];
                toutesLesLettres[j] = temp;
            }

            foreach (Lettre lettre in toutesLesLettres)
            {
                BureauPereNoel.Push(lettre);
            }
        }

        public void AvancerUneHeure()
        {
            // Arrivée des lettres au bureau
            if (BureauPereNoel.Count > 0)
            {
                int nbLettreQuiArrivent = random.Next(0, NbLettresParHeure + 1);

                for (int i = 0; i < nbLettreQuiArrivent; i++)
                {
                    Lettre lettreATraiter = BureauPereNoel.Pop();
                    FileAttenteFabrication.Enqueue(lettreATraiter);
                }
            }

            // Travail des lutins
            foreach (Lutin lutin in Lutins)
            {
                if (lutin.Statut == StatutEmploye.EnAttente && FileAttenteFabrication.Count > 0)
                {
                    Lettre lettreATraiter = FileAttenteFabrication.Dequeue();
                    lutin.CommencerFabrication(lettreATraiter);
                }

                if (lutin.Statut == StatutEmploye.EnTravail)
                {
                    Lettre lettreTraitee = lutin.Travailler();
                    if (lettreTraitee != null)
                    {
                        FileAttenteEmballage.Enqueue(lettreTraitee);
                    }
                }
            }

            // Travail des nains

            foreach (Nain nain in Nains)
            {
                if (nain.Statut == StatutEmploye.EnAttente && FileAttenteEmballage.Count > 0)
                {
                    Lettre lettreATraiter = FileAttenteEmballage.Dequeue();
                    nain.CommencerEmballage(lettreATraiter);
                }

                if (nain.Statut == StatutEmploye.EnTravail)
                {
                    Lettre lettreTraitee = nain.Travailler();
                    if (lettreTraitee != null)
                    {
                        FilesAttenteContinents[(int)lettreTraitee.Continent].Enqueue(lettreTraitee);
                    }
                }
            }

            // Travail des elfes
            foreach (Elfe elfe in Elfes)
            {
                if (!elfe.Traineau.EnVoyage)
                {
                    Queue<Lettre> fileContinent = FilesAttenteContinents[(int)elfe.Continent];
                    while (fileContinent.Count > 0 && elfe.PeutRecevoirLettre())
                    {
                        Lettre lettreTraitee = fileContinent.Dequeue();
                        elfe.RecevoirLettre(lettreTraitee);
                    }
                }
                List<Lettre> lettresLivrees = elfe.Traineau.AvancerVoyage();
                if (lettresLivrees != null && lettresLivrees.Count > 0)
                {
                    Entrepots[(int)elfe.Continent].AjouterLettres(lettresLivrees);
                }
            }
            // Salaires
            double salaireHeure = 0.0;

            //Salaire lutins
            foreach(Lutin lutin in Lutins)
            {
                if (lutin.Statut == StatutEmploye.EnTravail)
                {
                    salaireHeure += 1.5;
                } else if (lutin.Statut == StatutEmploye.EnAttente)
                {
                    salaireHeure += 1.0;
                }
            }

            //Salaire nains
            foreach (Nain nain in Nains)
            {
                if (nain.Statut == StatutEmploye.EnTravail)
                {
                    salaireHeure += 1.0;
                } else if (nain.Statut == StatutEmploye.EnAttente)
                {
                    salaireHeure += 0.5;
                }
            }

            //Salaire elfes
            foreach (Elfe elfe in Elfes)
            {
                if (elfe.Traineau.EnVoyage)
                {
                    salaireHeure += 2.0;
                }
                else if (elfe.Traineau.Lettres.Count > 0)
                {
                    salaireHeure += 1.5;
                }
            }
            // Mettre à jour les valeurs
            CoutTotal += salaireHeure;
            CoutsParHeure.Add(salaireHeure);
            HeureActuelle++;
            // Journée de 12h
            if (HeureActuelle % 12 == 0)
            {
                JourActuel++;
            }
        }

        public bool ToutesLettresTraitees()
        {
            if (BureauPereNoel.Count > 0) return false;
            if (FileAttenteFabrication.Count > 0) return false;
            if (FileAttenteEmballage.Count > 0) return false;

            // Vérifier si des lutins travaillent
            foreach (Lutin lutin in Lutins)
            {
                if (lutin.Statut == StatutEmploye.EnTravail)
                    return false;
            }

            // Vérifier si des nains travaillent
            foreach (Nain nain in Nains)
            {
                if (nain.Statut == StatutEmploye.EnTravail)
                    return false;
            }

            // Vérifier les files d'attente des continents
            foreach (Queue<Lettre> file in FilesAttenteContinents)
            {
                if (file.Count > 0)
                    return false;
            }

            // Vérifier les elfes et traîneaux
            foreach (Elfe elfe in Elfes)
            {
                if (elfe.Traineau.EnVoyage || elfe.Traineau.Lettres.Count > 0)
                    return false;
            }

            return true;
        }

        public void AvancerJusquaJourSuivant()
        {
            int heuresAAjouter = 12 - (HeureActuelle % 12);
            if (heuresAAjouter == 0)
            {
                heuresAAjouter = 12;
            }

            int cibleHeure = HeureActuelle + heuresAAjouter;

            while (HeureActuelle < cibleHeure)
            {
                int heureAvant = HeureActuelle;
                AvancerUneHeure();

                // Spour eviter une boucle infini
                if (HeureActuelle == heureAvant)
                {
                    break;
                }
            }
        }

        public void ModifierNombreLutins(int nouveauNombre)
        {
            int total = Lutins.Count;
            if (nouveauNombre < 0 || nouveauNombre > total)
            {
                Console.WriteLine("Nombre de lutins invalide.");
                return;
            }

            const int delai = 12;
            int heuresDepuis = HeureActuelle - DerniereModificationLutins;
            if (heuresDepuis < delai)
            {
                int heurePossible = delai - heuresDepuis;
                Console.WriteLine($"Modification impossible avant {heurePossible} heure(s).");
                return;
            }

            int actifs = Lutins.Count(l => l.Statut != StatutEmploye.EnRepos);
            if (nouveauNombre == actifs)
            {
                Console.WriteLine(" Aucun changement nécessaire.");
                return;
            }

            if (nouveauNombre < actifs)
            {
                int aMettreEnRepos = actifs - nouveauNombre;
                List<Lutin> disponibles = Lutins.Where(l => l.Statut == StatutEmploye.EnAttente)
                                                .Take(aMettreEnRepos)
                                                .ToList();

                if (disponibles.Count < aMettreEnRepos)
                {
                    Console.WriteLine("Impossible de mettre autant de lutins en repos (certains travaillent encore).");
                    return;
                }

                foreach (Lutin lutin in disponibles)
                {
                    lutin.Statut = StatutEmploye.EnRepos;
                    lutin.LettreEnCours = null;
                    lutin.HeuresRestantes = 0;
                }

                Console.WriteLine($"{aMettreEnRepos} lutin(s) mis en repos.");
            }
            else
            {
                int aReactiv = nouveauNombre - actifs;
                List<Lutin> repos = Lutins.Where(l => l.Statut == StatutEmploye.EnRepos)
                                          .Take(aReactiv)
                                          .ToList();

                if (repos.Count < aReactiv)
                {
                    Console.WriteLine(" Aucun lutin supplémentaire n'est disponible pour reprendre le travail.");
                    return;
                }

                foreach (Lutin lutin in repos)
                {
                    lutin.Statut = StatutEmploye.EnAttente;
                    lutin.LettreEnCours = null;
                    lutin.HeuresRestantes = 0;
                }

                Console.WriteLine($"{aReactiv} lutin(s) réaffecté(s) au travail.");
            }

            DerniereModificationLutins = HeureActuelle;
        }

        public void ModifierNombreNains(int nouveauNombre)
        {
            int total = Nains.Count;
            if (nouveauNombre < 0 || nouveauNombre > total)
            {
                Console.WriteLine(" Nombre de nains invalide.");
                return;
            }

            const int delai = 24;
            int heuresDepuis = HeureActuelle - DerniereModificationNains;
            if (heuresDepuis < delai)
            {
                int heurePossible = delai - heuresDepuis;
                Console.WriteLine($" Modification impossible avant {heurePossible} heure(s).");
                return;
            }

            int actifs = Nains.Count(n => n.Statut != StatutEmploye.EnRepos);
            if (nouveauNombre == actifs)
            {
                Console.WriteLine(" Aucun changement nécessaire.");
                return;
            }

            if (nouveauNombre < actifs)
            {
                int aMettreEnRepos = actifs - nouveauNombre;
                List<Nain> disponibles = Nains.Where(n => n.Statut == StatutEmploye.EnAttente)
                                              .Take(aMettreEnRepos)
                                              .ToList();

                if (disponibles.Count < aMettreEnRepos)
                {
                    Console.WriteLine(" Impossible de mettre autant de nains en repos (certains emballent encore).");
                    return;
                }

                foreach (Nain nain in disponibles)
                {
                    nain.Statut = StatutEmploye.EnRepos;
                    nain.LettreEnCours = null;
                    nain.HeuresRestantes = 0;
                }

                Console.WriteLine($"{aMettreEnRepos} nain(s) mis en repos.");
            }
            else
            {
                int aReactiv = nouveauNombre - actifs;
                List<Nain> repos = Nains.Where(n => n.Statut == StatutEmploye.EnRepos)
                                        .Take(aReactiv)
                                        .ToList();

                if (repos.Count < aReactiv)
                {
                    Console.WriteLine(" Aucun nain supplémentaire n'est disponible pour reprendre le travail.");
                    return;
                }

                foreach (Nain nain in repos)
                {
                    nain.Statut = StatutEmploye.EnAttente;
                    nain.LettreEnCours = null;
                    nain.HeuresRestantes = 0;
                }

                Console.WriteLine($"{aReactiv} nain(s) réaffecté(s) au travail.");
            }

            DerniereModificationNains = HeureActuelle;
        }

        public void AfficherIndicateurs()
        {
            Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"║  INDICATEURS - Jour {JourActuel}, Heure {HeureActuelle % 12 + 1}/12");
            Console.WriteLine("╠════════════════════════════════════════════════════════════╣");

            // Lutins - compter manuellement
            int lutinsEnTravail = 0;
            int lutinsEnAttente = 0;
            int lutinsEnRepos = 0;
            foreach (Lutin lutin in Lutins)
            {
                if (lutin.Statut == StatutEmploye.EnTravail)
                    lutinsEnTravail++;
                else if (lutin.Statut == StatutEmploye.EnAttente)
                    lutinsEnAttente++;
                else if (lutin.Statut == StatutEmploye.EnRepos)
                    lutinsEnRepos++;
            }

            Console.WriteLine("║ LUTINS:");
            Console.WriteLine($"║   • En travail: {lutinsEnTravail}");
            Console.WriteLine($"║   • En attente: {lutinsEnAttente}");
            Console.WriteLine($"║   • En repos: {lutinsEnRepos}");
            Console.WriteLine($"║   • Cadeaux en fabrication: {lutinsEnTravail}");
            Console.WriteLine($"║   • Lettres en attente: {FileAttenteFabrication.Count}");

            // Nains - compter manuellement
            int nainsEnTravail = 0;
            int nainsEnAttente = 0;
            int nainsEnRepos = 0;
            foreach (Nain nain in Nains)
            {
                if (nain.Statut == StatutEmploye.EnTravail)
                    nainsEnTravail++;
                else if (nain.Statut == StatutEmploye.EnAttente)
                    nainsEnAttente++;
                else if (nain.Statut == StatutEmploye.EnRepos)
                    nainsEnRepos++;
            }

            Console.WriteLine("║ NAINS:");
            Console.WriteLine($"║   • En travail: {nainsEnTravail}");
            Console.WriteLine($"║   • En attente: {nainsEnAttente}");
            Console.WriteLine($"║   • En repos: {nainsEnRepos}");
            Console.WriteLine($"║   • Cadeaux en emballage: {nainsEnTravail}");
            Console.WriteLine($"║   • Cadeaux en attente d'emballage: {FileAttenteEmballage.Count}");

            // Elfes et traîneaux
            Console.WriteLine("║ ELFES ET TRAÎNEAUX:");
            foreach (Elfe elfe in Elfes)
            {
                string statut = elfe.Traineau.EnVoyage ?
                    $"En voyage ({elfe.Traineau.HeuresVoyageRestantes}h restantes)" :
                    $"Au chargement ({elfe.Traineau.Lettres.Count}/{NbJouetParTraineau})";
                int enAttente = FilesAttenteContinents[(int)elfe.Continent].Count;
                Console.WriteLine($"║   • {elfe.Continent}: {statut}, En attente: {enAttente}");
            }

            // Coûts - calculer la moyenne manuellement
            Console.WriteLine("║ COÛTS:");
            Console.WriteLine($"║   • Coût total: {CoutTotal:F2} pièces d'or");

            double coutMoyen = 0;
            if (CoutsParHeure.Count > 0)
            {
                double somme = 0;
                foreach (double cout in CoutsParHeure)
                {
                    somme += cout;
                }
                coutMoyen = somme / CoutsParHeure.Count;
            }
            Console.WriteLine($"║   • Coût moyen/heure: {coutMoyen:F2} pièces d'or");

            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");
        }

        public void AfficherBilan()
        {
            Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                     BILAN FINAL");
            Console.WriteLine("╠════════════════════════════════════════════════════════════╣");
            Console.WriteLine($"║ Durée totale: {JourActuel - 1} jours et {HeureActuelle % 12} heures");
            Console.WriteLine($"║ Coût total: {CoutTotal:F2} pièces d'or");

            // Calculer la moyenne manuellement
            double coutMoyen = 0;
            if (CoutsParHeure.Count > 0)
            {
                double somme = 0;
                foreach (double cout in CoutsParHeure)
                {
                    somme += cout;
                }
                coutMoyen = somme / CoutsParHeure.Count;
            }
            Console.WriteLine($"║ Coût moyen par heure: {coutMoyen:F2} pièces d'or");

            Console.WriteLine("╠════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ ENTREPÔTS CONTINENTAUX:");

            for (int i = 0; i < Entrepots.Length; i++)
            {
                Entrepot entrepot = Entrepots[i];
                Console.WriteLine($"║ {entrepot.Continent}:");
                Console.WriteLine($"║   Total: {entrepot.Lettres.Count} jouets");
                int[] stats = entrepot.GetStatistiques();

                // Parcourir chaque type de cadeau
                for (int j = 0; j < stats.Length; j++)
                {
                    if (stats[j] > 0)
                    {
                        TypeCadeau type = (TypeCadeau)j;
                        Console.WriteLine($"║   • {type}: {stats[j]}");
                    }
                }
            }

            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");
        }
    }

    // ============= PROGRAMME PRINCIPAL =============

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Simulation simulation = null;
            bool continuer = true;

            while (continuer)
            {
                if (simulation == null)
                {
                    simulation = MenuInitialisation();
                }
                else
                {
                    continuer = MenuPrincipal(simulation);

                    if (continuer && simulation.ToutesLettresTraitees())
                    {
                        Console.WriteLine("\n🎄 Toutes les lettres ont été traitées! 🎄");
                        simulation.AfficherBilan();

                        Console.Write("\nVoulez-vous lancer une nouvelle simulation? (o/n): ");
                        string choix = Console.ReadLine().ToLower();
                        if (choix == "o" || choix == "oui")
                        {
                            simulation = null;
                        }
                        else
                        {
                            continuer = false;
                        }
                    }
                }
            }

            Console.WriteLine("\n🎅 Joyeux Noël! 🎄");
        }

        static Simulation MenuInitialisation()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║          BIENVENUE AU PAYS DU PÈRE NOËL                    ║");
            Console.WriteLine("║              Initialisation de la simulation               ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

            Console.Write("Nombre de lutins disponibles: ");
            int nbLutins = int.Parse(Console.ReadLine());

            Console.Write("Nombre de nains disponibles: ");
            int nbNains = int.Parse(Console.ReadLine());

            Console.Write("Nombre d'enfants: ");
            int nbEnfants = int.Parse(Console.ReadLine());

            Console.Write("Nombre maximum de lettres par heure: ");
            int nbLettresParHeure = int.Parse(Console.ReadLine());

            Console.Write("Capacité de chaque traîneau: ");
            int nbJouetParTraineau = int.Parse(Console.ReadLine());

            Console.WriteLine("\n✅ Simulation initialisée avec succès!");
            Console.WriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadKey();

            return new Simulation(nbLutins, nbNains, nbEnfants, nbLettresParHeure, nbJouetParTraineau);
        }

        static bool MenuPrincipal(Simulation simulation)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                 GESTION DU PÈRE NOËL                       ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ 1. Avancer d'une heure                                     ║");
            Console.WriteLine("║ 2. Avancer jusqu'au début de la journée suivante           ║");
            Console.WriteLine("║ 3. Afficher les indicateurs                                ║");
            Console.WriteLine("║ 4. Gérer les lutins                                        ║");
            Console.WriteLine("║ 5. Gérer les nains                                         ║");
            Console.WriteLine("║ 6. Visualiser les entrepôts                                ║");
            Console.WriteLine("║ 7. Afficher le bilan et continuer                          ║");
            Console.WriteLine("║ 8. Afficher le bilan et arrêter                            ║");
            Console.WriteLine("║ 9. Afficher le bilan et relancer                           ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
            Console.Write("\nVotre choix: ");

            string choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    simulation.AvancerUneHeure();   // Cette Méthode est à écrire.
                    Console.WriteLine($"\n✅ Avancé à: Jour {simulation.JourActuel}, Heure {simulation.HeureActuelle % 12 + 1}");
                    Console.WriteLine("Appuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return true;

                case "2":
                    int heuresAvant = simulation.HeureActuelle;
                    simulation.AvancerJusquaJourSuivant();  // Cette Méthode est à écrire.
                    Console.WriteLine($"\n✅ Avancé jusqu'à: Jour {simulation.JourActuel}, Heure 1");
                    Console.WriteLine($"   ({simulation.HeureActuelle - heuresAvant} heures écoulées)");
                    Console.WriteLine("Appuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return true;

                case "3":
                    simulation.AfficherIndicateurs();
                    Console.WriteLine("Appuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return true;

                case "4":
                    MenuGestionLutins(simulation);
                    return true;

                case "5":
                    MenuGestionNains(simulation);
                    return true;

                case "6":
                    AfficherEntrepots(simulation);
                    Console.WriteLine("Appuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return true;

                case "7":
                    simulation.AfficherBilan();
                    Console.WriteLine("Appuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return true;

                case "8":
                    simulation.AfficherBilan();
                    return false;

                case "9":
                    simulation.AfficherBilan();
                    Console.WriteLine("\n🔄 Redémarrage de la simulation...");
                    Console.WriteLine("Appuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return false;

                default:
                    Console.WriteLine("❌ Choix invalide.");
                    Console.WriteLine("Appuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return true;
            }
        }

        static void MenuGestionLutins(Simulation simulation)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                   GESTION DES LUTINS                       ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

            // Compter les lutins actifs
            int actifs = 0;
            foreach (Lutin lutin in simulation.Lutins)
            {
                if (lutin.Statut != StatutEmploye.EnRepos)
                    actifs++;
            }

            Console.WriteLine($"Lutins actuellement actifs: {actifs}/{simulation.NbLutins}");
            Console.WriteLine($"Dernière modification: il y a {simulation.HeureActuelle - simulation.DerniereModificationLutins} heures");
            Console.WriteLine($"(Modification possible après 12 heures)\n");

            Console.Write("Nouveau nombre de lutins actifs (ou 'c' pour annuler): ");
            string input = Console.ReadLine();

            if (input.ToLower() != "c")
            {
                if (int.TryParse(input, out int nouveau))
                {
                    simulation.ModifierNombreLutins(nouveau);   // Cette Méthode est à écrire.
                }
                else
                {
                    Console.WriteLine("❌ Nombre invalide.");
                }
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        static void MenuGestionNains(Simulation simulation)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                   GESTION DES NAINS                        ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

            // Compter les nains actifs
            int actifs = 0;
            foreach (Nain nain in simulation.Nains)
            {
                if (nain.Statut != StatutEmploye.EnRepos)
                    actifs++;
            }

            Console.WriteLine($"Nains actuellement actifs: {actifs}/{simulation.NbNains}");
            Console.WriteLine($"Dernière modification: il y a {simulation.HeureActuelle - simulation.DerniereModificationNains} heures");
            Console.WriteLine($"(Modification possible après 24 heures)\n");

            Console.Write("Nouveau nombre de nains actifs (ou 'c' pour annuler): ");
            string input = Console.ReadLine();

            if (input.ToLower() != "c")
            {
                if (int.TryParse(input, out int nouveau))
                {
                    simulation.ModifierNombreNains(nouveau);    // Cette Méthode est à écrire.
                }
                else
                {
                    Console.WriteLine("❌ Nombre invalide.");
                }
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        static void AfficherEntrepots(Simulation simulation)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║              ÉTAT DES ENTREPÔTS CONTINENTAUX               ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

            for (int i = 0; i < simulation.Entrepots.Length; i++)
            {
                Entrepot entrepot = simulation.Entrepots[i];
                Console.WriteLine($"═══ {entrepot.Continent} ═══");
                Console.WriteLine($"Total de jouets stockés: {entrepot.Lettres.Count}");

                int[] stats = entrepot.GetStatistiques();
                Console.WriteLine("Répartition par catégorie d'âge:");

                // Parcourir les types de cadeaux dans l'ordre de l'enum
                foreach (TypeCadeau type in Enum.GetValues(typeof(TypeCadeau)))
                {
                    int index = (int)type;
                    if (stats[index] > 0)
                    {
                        string categorie;
                        switch (type)
                        {
                            case TypeCadeau.Nounours:
                                categorie = "0-2 ans (Nounours)";
                                break;
                            case TypeCadeau.Tricycle:
                                categorie = "3-5 ans (Tricycle)";
                                break;
                            case TypeCadeau.Jumelles:
                                categorie = "6-10 ans (Jumelles)";
                                break;
                            case TypeCadeau.AbonnementGeekyJunior:
                                categorie = "11-15 ans (Abonnement)";
                                break;
                            case TypeCadeau.Ordinateur:
                                categorie = "16-18 ans (Ordinateur)";
                                break;
                            default:
                                categorie = "Inconnu";
                                break;
                        }
                        Console.WriteLine($"  • {categorie}: {stats[index]}");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
