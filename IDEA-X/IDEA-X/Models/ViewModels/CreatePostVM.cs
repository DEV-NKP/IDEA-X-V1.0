using IDEA_X.IDEAX_Class;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace IDEA_X.Models.ViewModels
{
    public class CreatePostVM
    { 
       // [Required(ErrorMessage ="Please write something for your post")]
        public string TIMELINE_TEXT { get; set; }
        public HttpPostedFileBase TIMELINE_IMG { get; set; } = null;
        public string POST_TAG { get; set; } = null;

    }
}