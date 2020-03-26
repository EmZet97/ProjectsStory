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

        [MinLength(3, ErrorMessage = "At least 3 characters required in title")]
        [Required( ErrorMessage = "Title required" )]
        public string Title { get; set; }

        //Publicity
        public string ShareUrl { get; set; }
        public bool IsPublic { get; set; }
        

        //URLs
        public string RepositoryLink { get; set; }
        public string ReadyProductLink { get; set; }

        //Visit counter
        public int VisitCounter { get; set; } = 0;

        //References
        [ForeignKey("Repository")]
        public int RepositoryId { get; set; }
        [Required]
        public virtual Repository Repository { get; set; }

        public virtual List<ProjectUpdate> ProjectUpdates { get; set; }


    }
}