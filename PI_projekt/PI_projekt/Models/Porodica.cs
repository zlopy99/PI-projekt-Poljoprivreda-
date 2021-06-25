using System;
using System.Collections.Generic;

#nullable disable

namespace PI_projekt.Models
{
    public partial class Porodica
    {
        public Porodica()
        {
            Biljkas = new HashSet<Biljka>();
        }

        public int PorodicaId { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }

        public virtual ICollection<Biljka> Biljkas { get; set; }
    }
}
