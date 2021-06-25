using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PI_projekt.ViewModels
{
    public class LokacijaViewModel
    {
        public int KorisnikID { get; set; }
        public int LokacijaID { get; set; }
        public string NazivLokacije { get; set; }
        public string Povrsina { get; set; }
        public string ObradjenaPovrsina { get; set; }
        public string VrstaTla { get; set; }
    }
}
