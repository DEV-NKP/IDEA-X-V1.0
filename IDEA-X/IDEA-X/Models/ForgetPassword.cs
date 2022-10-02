using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Validation_Task.Models;

namespace IDEA_X.Models
{
    public class ForgetPassword
    {
        public string EMAIL { get; set; }



        [Required]
         [PasswordValidation]
        public string PASSWORD { get; set; }
        [Required]
        [Compare("PASSWORD", ErrorMessage ="Did not match")]
        public string CONFIRM_PASSWORD { get; set; }
    }
}