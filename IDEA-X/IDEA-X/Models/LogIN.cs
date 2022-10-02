using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Validation_Task.Models;

namespace IDEA_X.Models
{
    public class LogIN
    {
        //[Required]
        public string USERNAME { get; set; }

        // [Required]
        // [PasswordValidation]
        public string PASSWORD { get; set; }

        public string REMEMBER { get; set; }
    }
}