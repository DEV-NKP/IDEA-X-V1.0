//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IDEA_X.Models.EntityFramework
{
    using System;
    using System.Collections.Generic;
    
    public partial class POST_REPORT
    {
        public int REPORT_ID { get; set; }
        public Nullable<int> POST_ID { get; set; }
        public string POST_AUTHOR { get; set; }
        public string REPORTED_BY { get; set; }
        public string REPORT_CATEGORY { get; set; }
        public string REPORT_DETAILS { get; set; }
        public string REPORT_TIME { get; set; }
        public string REPORT_IP { get; set; }
        public string REPORT_STATUS { get; set; }
    }
}
