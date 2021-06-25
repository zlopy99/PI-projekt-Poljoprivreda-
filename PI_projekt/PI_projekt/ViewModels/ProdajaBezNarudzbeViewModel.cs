using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PI_projekt.Models;
namespace PI_projekt.ViewModels
{
    public class ProdajaBezNarudzbeViewModel
    {
        public int ProdajaBezNarudzbeId { get; set; }
        public int KorisnikId { get; set; }
        public int KupacId { get; set; }
        public int biljkaKorisnikId { get; set; }
        public string NazivBiljke { get; set; }
        public string ImeKupca { get; set; }
        public string KontaktKupca { get; set; }
        public string Kolicina { get; set; }
        public string Cijena { get; set; }
        public string Opis { get; set; }

    }
}
