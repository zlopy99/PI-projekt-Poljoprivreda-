using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PI_projekt.Models;

namespace PI_projekt.ViewModels
{
    public class PosloviViewModel
    {
        public int PosloviId { get; set; }
        public int BiljkaId { get; set; }
        public string NazivPosla { get; set; }
        public string NazivBiljke { get; set; }
        public string Opis { get; set; }
        

    }
}
