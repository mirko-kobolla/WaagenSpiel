namespace WaagenSpiel.Models
{
    public class Stein
    {
        public string Farbe {  get; set; }
        public int Gewicht { get; set; }

        public bool Platziert { get; set; }

        public bool GewichtBestimmt { get; set; }
        public Stein(string farbe, int gewicht)
        {
            Farbe = farbe;
            Gewicht = gewicht;
            Platziert = false;
            GewichtBestimmt = false;
        }
    }
}