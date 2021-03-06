﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ToolMgt.Common
{
   public  static  class MD5Encrypt
    {
       public static string GetMD5(string myString)
       {
           MD5 md5 = new MD5CryptoServiceProvider();
           byte[] fromData = System.Text.Encoding.Unicode.GetBytes(myString);
           byte[] targetData = md5.ComputeHash(fromData);
           string byte2String = null;
           for (int i = 0; i < targetData.Length; i++)
           {
               byte2String += targetData[i].ToString("x").PadLeft(2, '0');
           }
           return byte2String;
       } 
    }
}
