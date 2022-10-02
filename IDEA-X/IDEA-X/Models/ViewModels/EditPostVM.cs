using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDEA_X.Models.ViewModels
{
    public class EditPostVM
    {
public int POST_IDR { get; set; }
        public string TIMELINE_TEXTR { get; set; }
        public HttpPostedFileBase TIMELINE_IMGR { get; set; } = null;
        public string POST_TAGR { get; set; } = null;

  public byte[] TIMELINE_IMGRC { get; set; } = null;
    }
}