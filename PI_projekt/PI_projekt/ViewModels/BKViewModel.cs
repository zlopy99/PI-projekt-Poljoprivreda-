using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PI_projekt.Models;
namespace PI_projekt.ViewModels
{
    public class BKViewModel
    {
       
        public IEnumerable<BiljkaKorisnikViewModel> KorisnikoveBiljke { get; set; }
        
    }
}
