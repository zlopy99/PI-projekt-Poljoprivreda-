using System;
using System.Collections.Generic;

#nullable disable

namespace PI_projekt.Models
{
    public partial class FazaRazvoja
    {
        public FazaRazvoja()
        {
            FazaRazvojaBiljkes = new HashSet<FazaRazvojaBiljke>();
        }

        public int FazaRazvojaId { get; set; }
        public string Naziv { get; set; }

        public virtual ICollection<FazaRazvojaBiljke> FazaRazvojaBiljkes { get; set; }
    }
}
