using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectsStory.Models
{
    public class Repository
    {
        [Key, ForeignKey("Owner")]
        public int OwnerId { get; set; }

        public string ShareUrl { get; set; }
        public bool IsPublic { get; set; }        
        
        [Required]
        public virtual User Owner { get; set; }

        public virtual List<Project> Projects { get; set; }


    }
}