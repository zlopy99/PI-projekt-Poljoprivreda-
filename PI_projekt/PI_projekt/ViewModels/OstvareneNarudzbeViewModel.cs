using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PI_projekt.Models;
namespace PI_projekt.ViewModels
{
    public class OstvareneNarudzbeViewModel
    {
        public int OstvareneNarudzbeId { get; set; }
        public int? Biljka { get; set; }
        public string NazivBiljke { get; set; }
        public DateTime? Datum { get; set; }
        public int Kupac { get; set; }
        public string ImeKupca { get; set; }
        public string KontaktKupca { get; set; }
        public string Kolicina { get; set; }
        public string Zarada { get; set; }
        public int? Korisnik { get; set; }
        public string Opis { get; set; }
    }
}
