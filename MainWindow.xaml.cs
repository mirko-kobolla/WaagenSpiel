
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WaagenSpiel.Spiel;

namespace WaagenSpiel
{
    public partial class MainWindow : Window
    {
        private SpielManager spielManager;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            int anzahlGruppen = 6;

            if (Gruppenauswahl.SelectedItem is ComboBoxItem ausgeweahlteGruppe)
            {
                string text = ausgeweahlteGruppe.Content.ToString();

                if (text.Contains("7"))
                {
                    anzahlGruppen = 7;
                }
            }

            MessageBox.Show(
                $"Das Spiel wurde mit {anzahlGruppen} Gruppen gestartet!",
                "Waagen-Spiel",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            spielManager = new SpielManager(anzahlGruppen);

            SpielWindow spielWindow = new SpielWindow(spielManager);

            spielWindow.Show();
            this.Hide();
        }



    }
}

