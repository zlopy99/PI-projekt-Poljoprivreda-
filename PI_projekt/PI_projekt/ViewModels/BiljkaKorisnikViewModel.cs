using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PI_projekt.Models;
namespace PI_projekt.ViewModels
{
    public class BiljkaKorisnikViewModel
    {
        public int BiljkaKorisnikId { get; set; }
        public string Naziv { get; set; }
        public string CijenaPoKg { get; set; }
        public string Opis { get; set; }
        /*public IEnumerable<Biljka> Biljka { get; set; }
        public IEnumerable<BiljkeKorisnik> biljkeKorisnik{ get; set; }*/
}
}
