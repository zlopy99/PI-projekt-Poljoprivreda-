using System;
using System.Collections.Generic;

#nullable disable

namespace PI_projekt.Models
{
    public partial class BiljnaPutovnica
    {
        public int BiljnaPutovnicaId { get; set; }
        public string ZemljaPorijekla { get; set; }
        public int Biljka { get; set; }

        public virtual BiljkeKorisnik BiljkaNavigation { get; set; }
    }
}
