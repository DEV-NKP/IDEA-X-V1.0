using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDEA_X.Models.ViewModels
{
    public class ActivityActionAdmin
    {
        public String USERNAME { get; set; }
        public int LIKE { get; set; }
        public int DISLIKE { get; set; }

        public int LIKEtake { get; set; }
        public int DISLIKEtake { get; set; }
    }
}