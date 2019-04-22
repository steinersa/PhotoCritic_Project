using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhotoCritic.Models
{
    public class Sex
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Sex")]
        public string Name { get; set; }
    }
}