using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectsStory.Models
{
    
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        public string ShareUrl { get; set; }
        public bool IsPublic { get; set; }

        [MinLength(3)]
        [Required]
        public string Title { get; set; }
        public string RepositoryLink { get; set; }
        
        [ForeignKey("Repository")]
        public int RepositoryId { get; set; }
        [Required]
        public virtual Repository Repository { get; set; }

        public virtual List<ProjectUpdate> ProjectUpdates { get; set; }


    }
}