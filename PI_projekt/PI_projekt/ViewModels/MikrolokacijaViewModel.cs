using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PI_projekt.Models;
namespace PI_projekt.ViewModels
{
    public class MikrolokacijaViewModel
    {
        public int mikrolokacijaId { get; set; }
        public int korisnikId { get; set; }
        public int lokacijaId { get; set; }
        public int korisnikBiljkaId { get; set; }
        public int tipUzgojaId { get; set; }
        public string nazivTipaUzgoja { get; set; }
        public string nazivBiljke { get; set; }
        public string povrsina { get; set; }
        public string ocekivaniUrod { get; set; }
        public string opis { get; set; }

    }
}
