using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDEA_X.Models.ViewModels
{
    public class Note_User
    {
        public int NOTE_ID { get; set; }
        public string USERNAME { get; set; }
        public string NOTE_DATE { get; set; }
        public string NOTE_TEXT { get; set; }
        public string STATUS { get; set; }
        public string NOTE_TIME { get; set; }
        public string NOTE_IP { get; set; }
    }
}