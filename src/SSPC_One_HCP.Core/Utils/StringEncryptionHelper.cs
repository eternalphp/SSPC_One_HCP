using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Utils
{
    /// <summary>
    /// 字符串加密帮助
    /// </summary>
    public static class StringEncryptionHelper
    {
        /// <summary>
        /// 进行HASH编码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetHash(this string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();

            byte[] byteValue = Encoding.UTF8.GetBytes(input);

            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }

        /// <summary>
        /// 获取MD5字符串
        /// </summary>
        /// <param name="source">加密源</param>
        /// <returns>加密结果</returns>
        public static string GetMD5(this string source)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                string hash = GetMd5Hash(md5Hash, source);
                return hash;
            }
        }

        /// <summary>
        /// 获取MD5哈希串
        /// </summary>
        /// <param name="md5Hash">md5加密对象</param>
        /// <param name="source">加密源</param>
        /// <returns>加密结果</returns>
        private static string GetMd5Hash(MD5 md5Hash, string source)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(source));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        /// <summary>
        /// SHA1编码
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string SHA1_Hash(this string source)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] bytes_sha1_in = Encoding.Default.GetBytes(source);
            byte[] bytes_sha1_out = sha1.ComputeHash(bytes_sha1_in);
            string str_sha1_out = BitConverter.ToString(bytes_sha1_out);
            return str_sha1_out;
        }

        /// <summary>
        /// Base64编码
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Base64Encoding(this string source)
        {
            var encode = string.Empty;
            byte[] bytes = Encoding.UTF8.GetBytes(source);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = source;
            }
            return encode;
        }
    }
}
