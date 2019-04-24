using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PhotoCritic.Models
{
    public class OpinionatedIndividual
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Age")]
        [Display(Name = "Age")]
        public int AgeId { get; set; }
        public Age Age { get; set; }
        public IEnumerable<Age> Ages { get; set; }

        [Required]
        [ForeignKey("Sex")]
        [Display(Name = "Sex")]
        public int SexId { get; set; }
        public Sex Sex { get; set; }
        public IEnumerable<Sex> Sexes { get; set; }

        [Required]
        [ForeignKey("Race")]
        [Display(Name = "Race")]
        public int RaceId { get; set; }
        public Race Race { get; set; }
        public IEnumerable<Race> Races { get; set; }

        [Required]
        [ForeignKey("Location")]
        [Display(Name = "Location")]
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public IEnumerable<Location> Locations { get; set; }

        [Required]
        [ForeignKey("Education")]
        [Display(Name = "Education")]
        public int EducationId { get; set; }
        public Education Education { get; set; }
        public IEnumerable<Education> Educations { get; set; }

        [Required]
        [ForeignKey("Profession")]
        [Display(Name = "Profession")]
        public int ProfessionId { get; set; }
        public Profession Profession { get; set; }
        public IEnumerable<Profession> Professions { get; set; }

        [Required]
        [ForeignKey("MaritalStatus")]
        [Display(Name = "Marital Status")]
        public int MaritalStatusId { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public IEnumerable<MaritalStatus> MaritalStatuses { get; set; }

        [Required]
        [ForeignKey("IncomeLevel")]
        [Display(Name = "Income Level")]
        public int IncomeLevelId { get; set; }
        public IncomeLevel IncomeLevel { get; set; }
        public IEnumerable<IncomeLevel> IncomeLevels { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}