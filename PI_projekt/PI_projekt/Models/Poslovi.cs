using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace PI_projekt.Models
{
    public partial class Poslovi
    {
        public Poslovi()
        {
            PosloviBiljkas = new HashSet<PosloviBiljka>();
        }

        public int PosloviId { get; set; }
        [Required(ErrorMessage = "Unesite naziv!")]
        public string Naziv { get; set; }

        public virtual ICollection<PosloviBiljka> PosloviBiljkas { get; set; }
    }
}
