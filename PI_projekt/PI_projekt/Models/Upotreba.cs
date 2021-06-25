using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace PI_projekt.Models
{
    public partial class Upotreba
    {
        public Upotreba()
        {
            BiljkaUpotrebas = new HashSet<BiljkaUpotreba>();
        }

        public int UpotrebaId { get; set; }
        [Required(ErrorMessage = "Unesite upotrebu!")]
        public string Naziv { get; set; }

        public virtual ICollection<BiljkaUpotreba> BiljkaUpotrebas { get; set; }
    }
}
