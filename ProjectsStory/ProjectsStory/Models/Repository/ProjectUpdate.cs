using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectsStory.Models
{
    public class ProjectUpdate
    {
        [Key]
        public int UpdateId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublicationDate { get; set; }
        public string ImageUrl { get; set; }

        //References
        [ForeignKey("Project")]
        public int ProjectId { get; set; }
        [Required]
        public virtual Project Project { get; set; }
        
    }
}