using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PhotoCritic.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Image Name")]
        public string ImageName { get; set; }

        [Required]
        [Display(Name = "Upload Image")]
        public string ImagePath { get; set; }

        [Required]
        [ForeignKey("Category")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public IEnumerable<Category> Categories { get; set; }

        [Display(Name = "Image Hidden")]
        public bool Hidden { get; set; }

        [Display(Name = "Enable Comments")]
        public bool CommentsEnabled { get; set; }

        [Display(Name = "Total Likes")]
        public int? TotalLikes { get; set; }

        [Display(Name = "Total Dislikes")]
        public int? TotalDislikes { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }

        [Display(Name = "Upload Date")]
        public DateTime WhenCreated { get; set; }

        public Photo()
        {
            WhenCreated = DateTime.Now;
        }
    }
}