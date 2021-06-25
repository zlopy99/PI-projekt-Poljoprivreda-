using System;
using System.Collections.Generic;

#nullable disable

namespace PI_projekt.Models
{
    public partial class Red
    {
        public Red()
        {
            Biljkas = new HashSet<Biljka>();
        }

        public int RedId { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }

        public virtual ICollection<Biljka> Biljkas { get; set; }
    }
}
