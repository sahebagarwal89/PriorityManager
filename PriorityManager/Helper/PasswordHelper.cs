using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace PriorityManager.Helper
{
    public class PasswordHelper
    {
        public static string EncryptData(string data)
        {
            //create new instance of sha1
            SHA1 sha1 = SHA1.Create();

            //convert the input text to array of bytes
            byte[] hashData = sha1.ComputeHash(Encoding.Default.GetBytes(data));

            //create new instance of StringBuilder to save hashed data
            StringBuilder returnValue = new StringBuilder();

            //loop for each byte and add it to StringBuilder
            foreach (byte b in hashData)
            {
                returnValue.Append(b.ToString("X2"));
            }

            // return hexadecimal string
            return returnValue.ToString();
        }
    }
}