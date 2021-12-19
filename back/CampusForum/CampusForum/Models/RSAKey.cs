using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CampusForum.Models
{
    public class RSAKey
    {
        /// <summary>
        /// 创建RSA公钥私钥
        /// </summary>
        public static void createRSAKey()
        {
            string privateKeyPath = "privateKey.xml";
            string publicKeyPath = "publicKey.xml";

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            if(!File.Exists(privateKeyPath)||!File.Exists(publicKeyPath))
            {
                string privateKey = rsa.ToXmlString(true);
                string publicKey = rsa.ToXmlString(false);

                File.WriteAllText(privateKeyPath, privateKey);
                File.WriteAllText(publicKeyPath, publicKey);
            }
        }

        /// <summary>
        /// 使用RSA加密
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string RSAEncrypt(string data)
        {
            //使用公钥加密
            string publicKeyPath = "publicKey.xml";
            string publicKey = File.ReadAllText(publicKeyPath);

            RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
            rSACryptoServiceProvider.FromXmlString(publicKey);

            byte[] publicValue = rSACryptoServiceProvider.Encrypt(Encoding.UTF8.GetBytes(data), false);
            string publicStr = Convert.ToBase64String(publicValue);
            return publicStr;
        }


        public static string RSADecrypt(string data)
        {
            //使用私钥解密
            string privateKeyPath = "privateKey.xml";
            string privateKey = File.ReadAllText(privateKeyPath);

            RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
            rSACryptoServiceProvider.FromXmlString(privateKey);

            byte[] privateValue = rSACryptoServiceProvider.Decrypt(Convert.FromBase64String(data), false);
            string privateStr = Encoding.UTF8.GetString(privateValue);
            return privateStr;
        }
    }
}
