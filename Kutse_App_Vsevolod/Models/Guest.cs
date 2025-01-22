using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Kutse_App_Vsevolod.Models
{
    public class Guest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Sisesta nimi")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Sisesta email")]
        [RegularExpression(@".+\@.+\..+", ErrorMessage = "Valesti sisestatud email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Sisesta telefoni number")]
        [RegularExpression(@"\+372.+", ErrorMessage = "Numbri alguses peal olema +372")]
        public string Phone {  get; set; }
        [Required(ErrorMessage = "Sisesta oma valik")]
        
        public bool? WillAttend { get; set; }
    }
}