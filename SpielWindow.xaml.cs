using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WaagenSpiel.Models;
using WaagenSpiel.Spiel;

namespace WaagenSpiel
{

    public partial class SpielWindow : Window
    {
        private SpielManager spielManager;
        private Stein ausgewaehlterStein;
        private Waage  hauptwaage;
        private Waage zweitwaage;
        public SpielWindow(SpielManager manager)
        {
            InitializeComponent();

            spielManager = manager;
            
            hauptwaage = new Waage();
            zweitwaage = new Waage();

            ZeigeAktuelleGruppe();
           
        }
        private void ZeigeAktuelleGruppe()
        {
            Gruppe gruppe = spielManager.HoleAktuelleGruppe();
            GruppenText.Text = $"{gruppe.Name} ist an der Reihe";
            SteinnePanel.Children.Clear();
            foreach (Stein stein in gruppe.Steine)
            {
                if (!stein.Platziert)
                {
                    Button button = ErstelleSteinButton(stein);
                    SteinnePanel.Children.Add(button);

                }
            }
        }

        private Button ErstelleSteinButton(Stein stein)
        {
            Button button = new Button();

            button.Content = stein.Farbe;
            button.Tag = stein;
            button.Width = 90;
            button.Height = 60;
            button.Margin = new Thickness(5);
            button.FontSize = 18;
            button.Click += SteinButton_Click;

            return button;
        }

        private void SteinButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button) sender;
            ausgewaehlterStein = (Stein)button.Tag;
            StatusText.Text =
                $" {ausgewaehlterStein.Farbe} wurde ausgewählt.";
        }

        private void NaechsteGruppe_Click(object sender, RoutedEventArgs e)
        {
            if (spielManager.NaechsteGruppe())
            {
                ZeigeAktuelleGruppe();
                RundenText.Text = $"Runde: {spielManager.AktuelleGruppe + 1}";
                StatusText.Text = "Nächste Gruppe ist an der Reihe.";
                ZeigeSpielstand();
            }
            else
            {
                ZeigeEndbildschirm(
                    spielManager.IstSpielGewonnen(),
                    spielManager.IstSpielGewonnen()
                        ? "Alle Gruppen haben ihre Steine platziert."
                        : "Keine spielbereite Gruppe ist mehr vorhanden.");
            }
        }

        private void GruppeRaus_Click(object sender, RoutedEventArgs e)
        {
            Gruppe gruppe = spielManager.HoleAktuelleGruppe();
            // Gruppe als ausgeschieden markieren. SpielManager sorgt dafür,
            // dass ausgeschiedene Gruppen bei NaechsteGruppe() übersprungen werden.
            gruppe.Ausgeschieden = true;

            if (spielManager.NaechsteGruppe())
            {
                ZeigeAktuelleGruppe();
                StatusText.Text = "Gruppe " + gruppe.Nummer + " ist ausgeschieden.";
                ZeigeSpielstand();
            }
            else
            {
                ZeigeEndbildschirm(false, "Alle Gruppen sind ausgeschieden. Das Spiel ist beendet.");
            }
        }

        private void ZeigeSpielstand()
        {
            SpielstandPanel.Children.Clear();

            foreach (Gruppe gruppe in spielManager.Gruppen)
            {
                string verfuegbareSteine = string.Join(", ", gruppe.Steine
                    .Where(stein => !stein.Platziert)
                    .Select(stein => stein.Farbe));
                string gespielteSteine = string.Join(", ", gruppe.Steine
                    .Where(stein => stein.Platziert)
                    .Select(stein => stein.Farbe));

                if (string.IsNullOrEmpty(verfuegbareSteine))
                    verfuegbareSteine = "keine";
                if (string.IsNullOrEmpty(gespielteSteine))
                    gespielteSteine = "keine";

                string status = gruppe.Ausgeschieden
                    ? "ausgeschieden"
                    : gruppe.Steine.All(stein => stein.Platziert) ? "fertig" : "aktiv";

                TextBlock zeile = new TextBlock
                {
                    Text = $"{gruppe.Name}: {status}\n  Verfügbar: {verfuegbareSteine}\n  Gespielt: {gespielteSteine}",
                    FontSize = 16,
                    Margin = new Thickness(0, 0, 0, 12),
                    Foreground = new SolidColorBrush(Color.FromRgb(44, 62, 80))
                };
                SpielstandPanel.Children.Add(zeile);
            }

            SpielstandOverlay.Visibility = Visibility.Visible;
        }

        private void SpielstandWeiter_Click(object sender, RoutedEventArgs e)
        {
            SpielstandOverlay.Visibility = Visibility.Collapsed;
        }

        private void ZeigeEndbildschirm(bool gewonnen, string begruendung)
        {
            EndergebnisText.Text = gewonnen ? "GEWONNEN!" : "VERLOREN!";
            EndergebnisText.Foreground = gewonnen
                ? new SolidColorBrush(Color.FromRgb(39, 174, 96))
                : new SolidColorBrush(Color.FromRgb(192, 57, 43));
            EndebegruendungText.Text = begruendung;
            SpielstandOverlay.Visibility = Visibility.Collapsed;
            EndbildschirmOverlay.Visibility = Visibility.Visible;
        }

        private void NeuesSpiel_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Application.Current.MainWindow.Show();
        }
        private void HauptwaagenLinks_Click(object sender, RoutedEventArgs e)
        {
            PlatziereSteinAufWaage("HauptwaagenLinks");

        }
        private void HauptwaagenRechts_Click(object sender, RoutedEventArgs e)
        {
            PlatziereSteinAufWaage("HauptwaagenRechts");
        }
        private void ZweitwaagenLinks_Click(object sender, RoutedEventArgs e)
        {
            PlatziereSteinAufWaage("ZweitwaagenLinks");
        }
        private void ZweitwaagenRechts_Click(object sender, RoutedEventArgs e)
        {
            PlatziereSteinAufWaage("ZweitwaagenRechts");
        }


        private void PlatziereSteinAufWaage(string ziel)
        {
            if (ausgewaehlterStein == null)
            {
                StatusText.Text = "Bitte zuerst einen Stein auswählen.";
                return;
            }
            Stein stein = ausgewaehlterStein;
            if (ausgewaehlterStein.Platziert)
            {
                StatusText.Text = "Dieser Stein wurde bereits platziert.";
                return;
            }

            // Steine auf die gewünschte Seite legen
            if (ziel == "HauptwaagenLinks")
            {
                // Vorab-Prüfung: Führt das Platzieren zu identischen Kombinationen auf Hauptwaage?
                var linksSim = hauptwaage.LinkeSeite.Select(s => s.Farbe).OrderBy(f => f).ToList();
                var rechtsSim = hauptwaage.RechteSeite.Select(s => s.Farbe).OrderBy(f => f).ToList();
                linksSim.Add(stein.Farbe);
                linksSim = linksSim.OrderBy(f => f).ToList();
                if (linksSim.SequenceEqual(rechtsSim))
                {
                    StatusText.Text = "Warnung: Auf der Hauptwaage würden links und rechts die gleichen Steinkombinationen liegen. Stein kann nicht gesetzt werden.";
                    return;
                }

                hauptwaage.SteiLinksPlatzieren(stein);
                HauptwaageLinks.Items.Add(stein.Farbe);
            }
            else if (ziel == "HauptwaagenRechts")
            {
                // Vorab-Prüfung: Führt das Platzieren zu identischen Kombinationen auf Hauptwaage?
                var rechtsSim = hauptwaage.RechteSeite.Select(s => s.Farbe).OrderBy(f => f).ToList();
                var linksSim = hauptwaage.LinkeSeite.Select(s => s.Farbe).OrderBy(f => f).ToList();
                rechtsSim.Add(stein.Farbe);
                rechtsSim = rechtsSim.OrderBy(f => f).ToList();
                if (linksSim.SequenceEqual(rechtsSim))
                {
                    StatusText.Text = "Warnung: Auf der Hauptwaage würden links und rechts die gleichen Steinkombinationen liegen. Stein kann nicht gesetzt werden.";
                    return;
                }

                hauptwaage.SteinRechtsPlatzieren(stein);
                HauptwaageRechts.Items.Add(stein.Farbe);
            }
            else if (ziel == "ZweitwaagenLinks")
            {
                zweitwaage.SteiLinksPlatzieren(stein);
                ZweitwaageLinks.Items.Add(stein.Farbe);
            }
            else if (ziel == "ZweitwaagenRechts")
            {
                zweitwaage.SteinRechtsPlatzieren(stein);
                ZweitwaageRechts.Items.Add(stein.Farbe);
            }

            stein.Platziert = true;
            ausgewaehlterStein = null;
            ZeigeAktuelleGruppe();
            AktualisiereWaagenAnzeige();
            AktualisiereWaagenStatus();
            StatusText.Text =
                $"{stein.Farbe} wurde platziert";

        }

        private void AktualisiereWaagenAnzeige()
        {
            HauptwaageLinks.Items.Clear();
            HauptwaageRechts.Items.Clear();

            ZweitwaageLinks.Items.Clear();
            ZweitwaageRechts.Items.Clear();

            foreach (Stein stein in hauptwaage.LinkeSeite.OrderBy(s => s.Farbe))
            {
                HauptwaageLinks.Items.Add(stein.Farbe);
            }
            foreach (Stein stein in hauptwaage.RechteSeite.OrderBy(s => s.Farbe))
            {
                HauptwaageRechts.Items.Add(stein.Farbe);
            }
            foreach (Stein stein in zweitwaage.LinkeSeite.OrderBy(s => s.Farbe))
            {
                ZweitwaageLinks.Items.Add(stein.Farbe);
            }
            foreach (Stein stein in zweitwaage.RechteSeite.OrderBy(s => s.Farbe))
            {
                ZweitwaageRechts.Items.Add(stein.Farbe);
            }
        }


        private void AktualisiereWaagenStatus()
        {
            //Hauptwaage
            if (hauptwaage.GewichtLinks() > hauptwaage.GewichtRechts())
            {
                HauptwaageStatus.Text = "<= SCHWERER";
            }
            else if (hauptwaage.GewichtRechts() > hauptwaage.GewichtLinks())
            {
                HauptwaageStatus.Text = "SCHWERER =>";
            }
            else
            {
                HauptwaageStatus.Text = "<= AUSGEGLICHEN =>";
            }

            //Zeitwaage
            if (zweitwaage.GewichtLinks() > zweitwaage.GewichtRechts())
            {
                ZweitwaageStatus.Text = "<= SCHWERER";
            }
            else if (zweitwaage.GewichtRechts() > zweitwaage.GewichtLinks())
            {
                ZweitwaageStatus.Text = "SCHWERER =>";
            }
            else
            {
                ZweitwaageStatus.Text = "<= AUSGEGLICHEN =>";
            }

        }

    }
}
