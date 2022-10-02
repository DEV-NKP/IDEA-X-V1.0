using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDEA_X.Models
{
    public class POST_TIMELINE
    {
        public int POST_ID { get; set; }
        public string AUTHOR { get; set; }
        public int POST_LIKE { get; set; }
        public int POST_DISLIKE { get; set; }
        public string POST_TAG { get; set; }
        public string TIMELINE_TEXT { get; set; }
        public string POST_STATUS { get; set; }
        public string POSTING_STATUS { get; set; }
        public byte[] PROFILE_IMG { get; set; }
        public byte[] TIMELINE_IMAGE { get; set; }
        public string REPORTED_POST { get; set; }
    }
}