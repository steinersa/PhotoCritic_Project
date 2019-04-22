using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PhotoCritic.Models
{
    public class OpinionatedIndividualPhoto
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Opinion")]
        public bool LikeDislike { get; set; }

        [Display(Name = "Comments")]
        public string Comment { get; set; }

        [Display(Name = "My Reason")]
        public string Reason1 { get; set; }
        
        [Display(Name = "Another Reason")]
        public string Reason2 { get; set; }

        [ForeignKey("Photo")]
        public int PhotoId { get; set; }
        public Photo Photo { get; set; }

        [ForeignKey("OpinionatedIndividual")]
        public int OpinionatedIndividualId { get; set; }
        public OpinionatedIndividual OpinionatedIndividual { get; set; }
    }
}