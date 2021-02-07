using System.Windows;

namespace PZ3_NetworkService.Models
{
    public class PutModel
    {
        public int ID { get; set; }
        public string Broj { get; set; }
        public TipPuta Tip { get; set; }
        public bool NadgledaSe { get; set; }
        public int Index { get; set; }
        public int BrojVozila { get; set; }
        public int GranicnoStanjeVozila { get { return Tip.ID == 1 ? 15000 : 7000; } }
        public bool StanjeKriticno { get { return BrojVozila > GranicnoStanjeVozila; } }
    }
}
