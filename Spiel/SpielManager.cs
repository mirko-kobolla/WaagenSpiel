using System.Collections.Generic;
using System.Linq;
using WaagenSpiel.Models;

namespace WaagenSpiel.Spiel
{
    public class SpielManager
    {
        public List<Gruppe> Gruppen {  get; set; }

        public int AktuelleGruppe { get; set; }

        public SpielManager(int anzahlGruppen) 
        {
            Gruppen=new List<Gruppe>();
            AktuelleGruppe = 0;

            for (int i = 1; i <= anzahlGruppen; i++)
            {
                Gruppen.Add(new Gruppe(i));
            }
        }

        public Gruppe HoleAktuelleGruppe() 
        {
        return Gruppen[AktuelleGruppe];
        }

        public bool NaechsteGruppe()
        {
            if (Gruppen.Count == 0)
                return false;

            for (int offset = 1; offset <= Gruppen.Count; offset++)
            {
                int naechsterIndex = (AktuelleGruppe + offset) % Gruppen.Count;
                if (!Gruppen[naechsterIndex].Ausgeschieden)
                {
                    AktuelleGruppe = naechsterIndex;
                    return true;
                }
            }

            return false;
        }

        public bool IstSpielGewonnen()
        {
            return Gruppen.Any(gruppe =>
                !gruppe.Ausgeschieden && gruppe.Steine.All(stein => stein.Platziert));
        }

        public bool IstSpielVerloren()
        {
            return Gruppen.Count > 0 && Gruppen.All(gruppe => gruppe.Ausgeschieden);
        }
    }
}
