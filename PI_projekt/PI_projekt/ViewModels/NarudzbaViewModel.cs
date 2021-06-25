using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PI_projekt.Models;

namespace PI_projekt.ViewModels
{
    public class NarudzbaViewModel
    {
        public int NarudzbaId { get; set; }
        public int KorisnikId { get; set; }
        public int KupacId { get; set; }
        public string ImeKupca { get; set; }
        public DateTime datumNarudzbe { get; set; }
        public DateTime datumIsporuke { get; set; }
    }
}
