﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhotoCritic.Models
{
    public class Age
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Age")]
        public string Name { get; set; }
    }
}