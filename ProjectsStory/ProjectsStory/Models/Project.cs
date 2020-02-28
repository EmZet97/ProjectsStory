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

        [MinLength(3)]
        [Required]
        public string Title { get; set; }

        public string RepositoryLink { get; set; }

        // References
        public int UserId { get; set; }
        public User User { get; set; }

        public List<ProjectUpdate> ProjectUpdates { get; set; }


    }
}