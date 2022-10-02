using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDEA_X.Models.ViewModels
{
    public class ActivityPostAdmin
    {
        public String AUTHOR { get; set; }
        public int POST_ID { get; set; }
        public int LIKE { get; set; }
        public int DISLIKE { get; set; }
    }
}