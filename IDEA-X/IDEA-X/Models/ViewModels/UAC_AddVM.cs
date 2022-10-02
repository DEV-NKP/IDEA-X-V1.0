using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Validation_Task.Models;

namespace IDEA_X.ViewModels
{
    public class UAC_AddVM
    {
        [Required]
        [EmailAddress]
        public string EMAIL { get; set; }

        [Required]
        public string USERNAME { get; set; }

        [Required]
        [PasswordValidation]
        public string PASSWORD { get; set; }
    }
}