using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.Models
{
    public class StanjeModel
    {
        public int ID { get; set; }
        public DateTime Vreme { get; set; }
        public int IDPuta { get; set; }
        public string BrojPuta { get; set; }
        public int BrojVozila { get; set; }
    }
}
