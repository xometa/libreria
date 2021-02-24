using System;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace RosaMaríaBookstore.Props
{
    public class EncryptDecrypt{
        public byte[] Clave = Encoding.ASCII.GetBytes("rmbkstr20");
        public byte[] IV = Encoding.ASCII.GetBytes("Devjoker7.37hAES");
        public EncryptDecrypt(){

        }

        //doble camino
        public string Encrypte(string Password)
        {

            byte[] inputBytes = Encoding.ASCII.GetBytes(Password);
            byte[] encripted;
            RijndaelManaged cripto = new RijndaelManaged();
            using (MemoryStream ms = new MemoryStream(inputBytes.Length))
            {
                using (CryptoStream objCryptoStream = new CryptoStream(ms, cripto.CreateEncryptor(Clave, IV), CryptoStreamMode.Write))
                {
                    objCryptoStream.Write(inputBytes, 0, inputBytes.Length);
                    objCryptoStream.FlushFinalBlock();
                    objCryptoStream.Close();
                }
                encripted = ms.ToArray();
            }
            return Convert.ToBase64String(encripted);
        }
         public string Decrypte(string Password)
        {
            byte[] inputBytes = Convert.FromBase64String(Password);
            byte[] resultBytes = new byte[inputBytes.Length];
            string textoLimpio = String.Empty;
            RijndaelManaged cripto = new RijndaelManaged();
            using (MemoryStream ms = new MemoryStream(inputBytes))
            {
                using (CryptoStream objCryptoStream = new CryptoStream(ms, cripto.CreateDecryptor(Clave, IV), CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(objCryptoStream, true))
                    {
                        textoLimpio = sr.ReadToEnd();
                    }
                }
            }
            return textoLimpio;
        }

        //único camino, es el que ocuparemos en el sistema
        public static string GetSHA256(string Password){
            SHA256 sha256=SHA256Managed.Create();
            ASCIIEncoding encoding=new ASCIIEncoding();
            byte[] stream=null;
            StringBuilder sb=new StringBuilder();
            stream=sha256.ComputeHash(encoding.GetBytes(Password));
            for (int i = 0; i < stream.Length; i++)
            {
                sb.AppendFormat("{0:x2}",stream[i]);
            }
            return sb.ToString();
        }

        public static string CreateCode(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            System.Random rnd = new System.Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }
}