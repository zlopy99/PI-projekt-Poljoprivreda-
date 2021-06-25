using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PI_projekt.Models;
namespace PI_projekt.ViewModels
{
    public class BiljkaKorisnikModelZaDetalje
    {
        public int BiljkaKorisnikId { get; set; }
        public int BiljkaID { get; set; }
        public string Naziv { get; set; }
        public string LatinskiNaziv { get; set; }
        //public string NarodnaImena { get; set; }
        public string Red { get; set; }
        public string Razred { get; set; }
        public string Porodica { get; set; }
        public string Rod { get; set; }
        public string Opis { get; set; }
    }
}
