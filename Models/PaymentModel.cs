using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RazorPayWeb.Models
{
    public class PaymentModel
    {
        [Required]
        public string name { get; set; }
        [Required]
        public string email { get; set; }
        public string contactNumber { get; set; }
        public string address { get; set; }
        [Required]
        public int amount { get; set; }
    }
}
