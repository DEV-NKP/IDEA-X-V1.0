using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDEA_X.Models.ViewModels
{
    public class ReportPostVM
    {


        public int POST_ID { get; set; }
        public string POST_AUTHOR { get; set; }
        public string REPORT_CATEGORY { get; set; }
        public string REPORT_DETAILS { get; set; }
        public string REPORT_TIME { get; set; }
        public string REPORT_IP { get; set; }
        public string REPORT_STATUS { get; set; }

        public string REPORTED_BY { get; set; }

    }
}