using System;
using System.Collections.Generic;

#nullable disable

namespace PI_projekt.Models
{
    public partial class VrstaTla
    {
        public VrstaTla()
        {
            Lokacijas = new HashSet<Lokacija>();
        }

        public int VrstaTlaId { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }

        public virtual ICollection<Lokacija> Lokacijas { get; set; }
    }
}
