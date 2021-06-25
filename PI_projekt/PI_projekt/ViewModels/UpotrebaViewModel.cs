using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PI_projekt.Models;
namespace PI_projekt.ViewModels
{
    public class UpotrebaViewModel
    {
        public int BiljkaKorisnikId { get; set; }
        public int BiljkaUpotrebaId { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }
    }
}
