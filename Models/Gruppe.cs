using System.Collections.Generic;


namespace WaagenSpiel.Models
{
    public class Gruppe
    {
        public int Nummer { get; set; }

        public string Name { get; set; }

        public List<Stein> Steine {  get; set; }

        public bool Ausgeschieden { get; set; }

        public Gruppe(int nummer, string? name = null)
        {
            Nummer = nummer;
            Name = string.IsNullOrWhiteSpace(name) ? $"Gruppe {nummer}" : name;
            Ausgeschieden = false;

            Steine = new List<Stein>
            {
                new Stein("Rot",2),
                new Stein("Rot",2),

                new Stein("Lila",7),
                new Stein("Lila",7),

                new Stein("Gelb",10),
                new Stein("Gelb",10),

                new Stein("Blau",16),
                new Stein("Blau",16),

                new Stein("Grün",19),
                new Stein("Grün",19)
            };
        }
    }
}
