using System.Collections.Generic;
using System.Linq;

namespace WaagenSpiel.Models
{
    public class Waage
    {
        public List<Stein> LinkeSeite {  get; set; }
        public List<Stein> RechteSeite { get; set; }
        public Waage()
        {
            LinkeSeite = new List<Stein>();
            RechteSeite = new List<Stein>();
        }
        public int GewichtLinks()
        {
            return LinkeSeite.Sum(stein =>  stein.Gewicht);
        }
        public int GewichtRechts()
        {
            return RechteSeite.Sum(stein => stein.Gewicht);
        }
        public bool IstAusbalanciert()
        {
            return GewichtLinks() == GewichtRechts()
                && !SindKombinationenGleich();
        }
        public bool SindKombinationenGleich()
        {
            var links = LinkeSeite
                .Select(stein => stein.Farbe)
                .OrderBy(farbe => farbe)
                .ToList();

            var rechts = RechteSeite
                .Select(stein => stein.Farbe)
                .OrderBy(farbe => farbe)
                .ToList();

            return links.SequenceEqual(rechts);
        }
        public void SteiLinksPlatzieren(Stein stein)
        {
            LinkeSeite.Add(stein);
            stein.Platziert = true;
        }
        public void SteinRechtsPlatzieren(Stein stein)
        {
            RechteSeite.Add(stein);
            stein.Platziert = true;
        }
    }
}
