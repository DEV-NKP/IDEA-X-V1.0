using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDEA_X.Models
{
    public class User
    {

        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public string NEW_PASSWORD { get; set; }
        public string EMAIL { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string HEADLINE { get; set; }
        public string DATE_OF_BIRTH { get; set; }
        public string GENDER { get; set; }
        public string MOBILE { get; set; }
        public string USER_ADDRESS { get; set; }
        public string USER_STATE { get; set; }
        public string ZIP_CODE { get; set; }
        public string COUNTRY { get; set; }
        public string INDUSTRY { get; set; }
        public string EDUCATIONAL_INSTITUTION { get; set; }
        public string DEPARTMENT { get; set; }
        public string CONTACT_URL { get; set; }
        //public byte[] PROFILE_PICTURE { get; set; }
     
        public string USER_STATUS { get; set; }
     


    }
}