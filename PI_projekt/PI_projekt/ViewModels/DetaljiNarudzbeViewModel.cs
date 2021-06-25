using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PI_projekt.Models;
namespace PI_projekt.ViewModels
{
    public class DetaljiNarudzbeViewModel
    {
        public int detaljiNarudzbeId { get; set; }
        public int NarudzbaId { get; set; }
        public int BiljkaId { get; set; }
        public string NazivBiljke { get; set; }
        public string Cijena { get; set; }
        public string Kolicina { get; set; }
        public string KontaktKupca { get; set; }
        public string AdresaKupca { get; set; }
        
        public string Opis { get; set; }

    }
}
