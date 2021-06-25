using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace PI_projekt.Models
{
    public partial class Alat
    {
        /*public Alat()
        {
            PosloviBiljkas = new HashSet<PosloviBiljka>();
        }*/

        public int AlatId { get; set; }
        [Required(ErrorMessage = "Unesite naziv!")]
        public string Naziv { get; set; }
        [Required(ErrorMessage = "Unesite količinu!")]
        public int? Količina { get; set; }
        public int? Korisnik { get; set; }

        public virtual Korisnik KorisnikNavigation { get; set; }
       // public virtual ICollection<PosloviBiljka> PosloviBiljkas { get; set; }
    }
}
