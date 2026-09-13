using System.Collections.Generic;
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

            AktuelleGruppe = (AktuelleGruppe + 1) % Gruppen.Count;
            return true;
        }
    }
}
