using System;
using System.Collections.Generic;

#nullable disable

namespace PI_projekt.Models
{
    public partial class BiljkaUpotreba
    {
        public int BiljkaUpotrebaId { get; set; }
        public int? Biljka { get; set; }
        public int? Upotreba { get; set; }
        public string Opis { get; set; }

        public virtual Biljka BiljkaNavigation { get; set; }
        public virtual Upotreba UpotrebaNavigation { get; set; }
    }
}
