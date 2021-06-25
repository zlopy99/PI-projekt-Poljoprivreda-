using System;
using System.Collections.Generic;

#nullable disable

namespace PI_projekt.Models
{
    public partial class NarodnaImena
    {
        public int NarodnoImeId { get; set; }
        public string Naziv { get; set; }
        public int? Biljka { get; set; }

        public virtual Biljka BiljkaNavigation { get; set; }
    }
}
