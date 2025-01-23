using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kutse_App_Vsevolod.Models
{
    public class Puhkus
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Sisesta peo nimi")]
        public string Nimetus { get; set; } // Название праздника

        [Required(ErrorMessage = "Sisesta kuupäev")]
        public DateTime Kuupaev { get; set; } // Дата проведения
    }
}