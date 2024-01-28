using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Globalization;
using System.Data;

namespace CommonLib
{
    public class CommonFunc
    {
        public static string gKeyEncrypt_AES = "vb1fckcxjvdkcpxmz9a2sk9itybd3yop";
        public static string gKeyEncrypt_AES_Byte = "tls3asrj121sb6zf0idu3n7ryxyw2g1c";

        public static string Encrypt_MD5(string toEncrypt)
        {
            try
            {
                var md5Hasher = new MD5CryptoServiceProvider();
                byte[] hashedBytes;
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(toEncrypt));
                StringBuilder s = new StringBuilder();
                foreach (byte _hashedByte in hashedBytes)
                {
                    s.Append(_hashedByte.ToString("x2"));
                }
                return s.ToString();
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return "";
            }
        }

        public static string EncryptString_AES(string strDataToEncrypt, string strKey = "")
        {
            try
            {
                if (string.IsNullOrEmpty(strKey))
                    strKey = gKeyEncrypt_AES;

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Encoding.UTF8.GetBytes(strKey);
                    aesAlg.Mode = CipherMode.ECB; // ECB mode for simplicity, consider other modes for better security
                    aesAlg.Padding = PaddingMode.PKCS7; // PKCS7 padding for simplicity

                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                    using (var msEncrypt = new System.IO.MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (var swEncrypt = new System.IO.StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(strDataToEncrypt);
                            }
                        }
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return "";
            }
        }

        public static string DecryptString_AES(string strEncryptedString, string strKey = "")
        {
            try
            {
                if (string.IsNullOrEmpty(strKey))
                    strKey = gKeyEncrypt_AES;

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Encoding.UTF8.GetBytes(strKey);
                    aesAlg.Mode = CipherMode.ECB;
                    aesAlg.Padding = PaddingMode.PKCS7;

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (var msDecrypt = new System.IO.MemoryStream(Convert.FromBase64String(strEncryptedString)))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new System.IO.StreamReader(csDecrypt))
                            {
                                return srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return "";
            }
        }


        public static string Encrypt_Byte_ToString(string p_strdata, string strKey = "")
        {
            try
            {
                byte[] encryptedBytes = Encrypt_Byte(p_strdata, strKey);
                return BitConverter.ToString(encryptedBytes).Replace("-", "");
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return "";
            }
        }

        /// <summary>
        /// Giải mã aes
        /// </summary>
        /// <param name="p_strdata"></param>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public static string Decrypt_Byte_ToString(string p_strdata, string strKey)
        {
            try
            {
                // Giải mã
                byte[] encryptedBytes = FromHex(p_strdata);
                return Decrypt_Byte(encryptedBytes, strKey);
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return "";
            }
        }

        public static byte[] Encrypt_Byte(string p_strdata, string strKey = "")
        {
            try
            {
                if (string.IsNullOrEmpty(strKey))
                    strKey = gKeyEncrypt_AES_Byte;

                byte[] keyBytes = Encoding.UTF8.GetBytes(strKey);
                byte[] dataBytes = Encoding.UTF8.GetBytes(p_strdata);

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = keyBytes;
                    aesAlg.Mode = CipherMode.ECB;
                    aesAlg.Padding = PaddingMode.PKCS7;

                    ICryptoTransform encryptor = aesAlg.CreateEncryptor();
                    return encryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length);
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return new byte[0];
            }
        }

        public static string Decrypt_Byte(byte[] encryptedData, string key = "")
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                    key = gKeyEncrypt_AES_Byte;

                byte[] keyBytes = Encoding.UTF8.GetBytes(key);

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = keyBytes;
                    aesAlg.Mode = CipherMode.ECB;
                    aesAlg.Padding = PaddingMode.PKCS7;

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor();
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return ex.ToString();
            }
        }

        public static byte[] FromHex(string hex)
        {
            hex = hex.Replace("-", ""); // Loại bỏ dấu gạch nếu có
            byte[] data = new byte[hex.Length / 2];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return data;
        }

        public static string NumberToTextVN(decimal v_number)
        {
            string str = "";
            string s = v_number.ToString("#");
            string[] so = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] hang = new string[] { "", "nghìn", "triệu", "tỷ" };
            int i, j, donvi, chuc, tram;

            bool booAm = false;
            decimal decS = 0;
            //Tung addnew
            try
            {
                decS = Convert.ToDecimal(s.ToString());
            }
            catch
            {
            }
            if (decS < 0)
            {
                decS = -decS;
                s = decS.ToString();
                booAm = true;
            }
            i = s.Length;
            if (i == 0)
                str = so[0] + str;
            else
            {
                j = 0;
                while (i > 0)
                {
                    donvi = Convert.ToInt32(s.Substring(i - 1, 1));
                    i--;
                    if (i > 0)
                        chuc = Convert.ToInt32(s.Substring(i - 1, 1));
                    else
                        chuc = -1;
                    i--;
                    if (i > 0)
                        tram = Convert.ToInt32(s.Substring(i - 1, 1));
                    else
                        tram = -1;
                    i--;
                    if ((donvi > 0) || (chuc > 0) || (tram > 0) || (j == 3))
                        str = hang[j] + str;
                    j++;
                    if (j > 3) j = 1;
                    if ((donvi == 1) && (chuc > 1))
                        str = "một " + str;
                    else
                    {
                        if ((donvi == 5) && (chuc > 0))
                            str = "lăm " + str;
                        else if (donvi > 0)
                            str = so[donvi] + " " + str;
                    }
                    if (chuc < 0)
                        break;
                    else
                    {
                        if ((chuc == 0) && (donvi > 0)) str = "lẻ " + str;
                        if (chuc == 1) str = "mười " + str;
                        if (chuc > 1) str = so[chuc] + " mươi " + str;
                    }
                    if (tram < 0) break;
                    else
                    {
                        if ((tram > 0) || (chuc > 0) || (donvi > 0)) str = so[tram] + " trăm " + str;
                    }
                    str = " " + str;
                }
            }
            if (booAm) str = "Âm " + str;
            if (str.Length > 0)
            {
                string str1 = str.Trim().Substring(0, 1);
                string str2 = str.Trim().Substring(1);

                str = str1.ToUpper() + str2;
            }

            return str.Trim();
        }
               
    }
}
