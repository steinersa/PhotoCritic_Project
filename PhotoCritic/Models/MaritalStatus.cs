using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhotoCritic.Models
{
    public class MaritalStatus
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Marital Status")]
        public string Name { get; set; }
    }
}