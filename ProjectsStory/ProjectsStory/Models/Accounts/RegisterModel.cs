using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectsStory.Models
{
    public class RegisterModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [MinLength(3)]
        [MaxLength(30)]
        public string Name { get; set; }

        [MinLength(3)]
        [MaxLength(30)]
        public string Surname { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Nick { get; set; }

        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords must be equal")]
        public string RepeatedPassword { get; set; }
    }
}