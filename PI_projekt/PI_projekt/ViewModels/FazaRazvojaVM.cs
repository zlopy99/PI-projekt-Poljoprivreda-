using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PI_projekt.Models;
namespace PI_projekt.ViewModels
{
    public class FazaRazvojaVM
    {
        public int BiljkaFazaRazvojaId { get; set; }
        public int BiljkaKorisnikId { get; set; }
        public string Naziv { get; set; }
        public string TrajanjeFaze { get; set; }
        public string GodisnjeDobaOdvijanjaFaze { get; set; }
        public string Opis { get; set; }
    }
}
