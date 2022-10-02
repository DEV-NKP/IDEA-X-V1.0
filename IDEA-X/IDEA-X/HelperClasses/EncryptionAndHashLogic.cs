using EasyCrypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace IDEA_X.HelperClasses
{   // TODO : added this class --adb
    public class EncryptionAndHashLogic
    {
        public static string EncryptMsg(string msg,string sessionName,string chatTime)
        {
            
            string keyString = sessionName + chatTime;
           
            return AesEncryption.EncryptWithPassword(msg, keyString);
        }

        public static string DecryptMsg(string msg,string sessionName,string chatTime)
        {
            string keyString = sessionName + chatTime;

            return AesEncryption.DecryptWithPassword(msg, keyString);

        }

        public static string HashPassword(string pass)
        {
            using(SHA256 sHA256 = SHA256.Create())
            {
                byte[] e_pass = sHA256.ComputeHash(Encoding.UTF8.GetBytes(pass));
                StringBuilder output = new StringBuilder();
                for (int i = 0; i < e_pass.Length; i++)
                {
                    output.Append(String.Format("{0:x2}",e_pass[i]));
                }
                return output.ToString();
            }
        }
    }
}