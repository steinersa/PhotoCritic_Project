using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhotoCritic.Models
{
    public class IncomeLevel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Income Level")]
        public string Name { get; set; }
    }
}