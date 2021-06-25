using System;
using System.Collections.Generic;

#nullable disable

namespace PI_projekt.Models
{
    public partial class TipUzgoja
    {
        public TipUzgoja()
        {
            Mikrolokacijas = new HashSet<Mikrolokacija>();
        }

        public int TipUzgojaId { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }

        public virtual ICollection<Mikrolokacija> Mikrolokacijas { get; set; }
    }
}
