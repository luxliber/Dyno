using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using Prorubim.External;
using WordPressSharp;
using FileSystem = Microsoft.VisualBasic.FileIO.FileSystem;

namespace Prorubim.DynoStudio.External
{
    internal class ProrubimExternal
    {
        private static string _hwId;
        private static byte[] _hwIdBytes;
        private static string _product;

        internal static void ProcessLicenseChecking(string product)
        {
            _product = product.ToLower();
            if (_hwId == null)
                _hwId = GetHwId();

            if (_hwIdBytes == null)
                _hwIdBytes = Encoding.ASCII.GetBytes(_hwId);
            try
            {
                CheckInternetConnection();
                AutoLogin();
            }
            catch (Exception)
            {
                try
                {
                    Request(GetLogin(), GetPass());
                }
                catch (Exception e)
                {
                    ManualLogin(e.Message);
                }
            }
        }

        private static void CheckLimitDate()
        {
            var dir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DynoStudio");

            if (dir == null)
                throw new Exception("There is internal error on approoving license status process");

            var exFilepath = Path.Combine(dir, "ProrubimExternal");
            try
            {
                var exBytes = File.ReadAllBytes(exFilepath);


                var creationTime = File.GetCreationTime(exFilepath);
                var accessTime = File.GetLastAccessTime(exFilepath);

                if (creationTime == accessTime)
                {
                    var exPlainText = Desc(Convert.ToBase64String(exBytes),
                        creationTime.Date.ToString(CultureInfo.GetCultureInfo("ru-RU")), _hwIdBytes);
                    if (DateTime.TryParse(exPlainText, CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.None, out var limitDate) && limitDate.Date < DateTime.UtcNow.Date)
                    {
                        throw new Exception("There is need internet connection for approoving license status");
                    }
                }
                else
                {
                    throw new Exception("There is need internet connection for approoving license status");
                }
            }
            catch (Exception)
            {
                throw new Exception("There is need internet connection for approoving license status");
            }
        }

        private static void ManualLogin(string e)
        {
            var loginVindow = new ProrubimLoginWindow
            {
                StatusBox = {Text = e},
                Title = _product + " - Sign In"
            };
            var res = loginVindow.ShowDialog();
            if (res == null || res == false)
                throw new Exception("Login process has been canceled");
        }

        private static void AutoLogin()
        {
            try
            {
                Request(GetLogin(), GetPass());
            }
            catch (Exception e)
            {
                CheckLimitDate();
            }
        }

        private static string GetPass()
        {
            var key = GetAppSubKey();
            var names = key.GetValueNames();
            if (names.Any())
            {
                var cpass = key.GetValue(names.First()) as String;
                key.Close();
                if (cpass == null) return "";

                var pass = Unprotect(Convert.FromBase64String(cpass));
                return pass != null ? Encoding.ASCII.GetString(pass) : "";
            }

            return "";
        }

        private static RegistryKey GetAppSubKey()
        {
            var key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\\PRORUBIM\\" + _product,
                          RegistryKeyPermissionCheck.ReadWriteSubTree) ??
                      Registry.CurrentUser.CreateSubKey(@"SOFTWARE\\PRORUBIM\\" + _product,
                          RegistryKeyPermissionCheck.ReadWriteSubTree);

            return key;
        }

        private static string GetLogin()
        {
            var key = GetAppSubKey();
            var names = key.GetValueNames();
            if (names.Any())
            {
                var login = names.First();
                return login ?? "";
            }

            key.Close();
            return "";
        }

        internal static string Request(string login, string pass)
        {
            if (login == "" && pass == "")
                throw new Exception("Login or password is empty");

            string plainText;

            using (var client = new WordPressClient(new WordPressSiteConfig {BaseUrl = "https://content.prorubim.com"}))
            {
                string res;
                try
                {
                    res = client.WordPressService.ReqLicStatus(login, pass, _product, _hwId);
                }
                catch (WebException e)
                {
                    throw new Exception($"Server link error: {e.Message}");
                }
                catch (Exception e1)
                {
                    throw new Exception($"Login or password is incorrect: {e1.Message}");
                }

                var resPass = DateTime.UtcNow.Date.ToString(CultureInfo.GetCultureInfo("ru-RU"));
                var resSsalt = Encoding.ASCII.GetBytes(_hwId);
                plainText = Desc(res, resPass, resSsalt);
            }

            if (plainText == "_NO_FREE_LICS")
                throw new Exception("No free licenses for this product");
            if (plainText == "_LIC_EXPIRED")
                throw new Exception("Corresponding license has been expired");
            if (plainText.StartsWith("_LIC_ALREADY_ACTIVE"))
            {
                SaveLoginAndPassword(login, pass);
             //   try
                {
                    var limitDate = DateTime.Parse(plainText.Replace("_LIC_ALREADY_ACTIVE_", ""), CultureInfo.GetCultureInfo("ru-RU")).Date
                        .ToString(CultureInfo.GetCultureInfo("ru-RU"));


                    SaveLimitDate(limitDate);
                    return DateTime.UtcNow.Date.ToString(CultureInfo.GetCultureInfo("ru-RU"));
                }
             //   catch (Exception)
                {
             //       MessageBox.Show($"{plainText}");
                    
                }
            }

            if (plainText.StartsWith("_LIC_HAS_BEEN_ACTIVATED"))
            {
                SaveLoginAndPassword(login, pass);
                var limitDate = DateTime.Parse(plainText.Replace("_LIC_HAS_BEEN_ACTIVATED_", ""), CultureInfo.GetCultureInfo("ru-RU")).Date
                    .ToString(CultureInfo.GetCultureInfo("ru-RU"));
                SaveLimitDate(limitDate);
                MessageBox.Show("Your license has been activated succesfully!", _product);
                return DateTime.UtcNow.Date.ToString(CultureInfo.GetCultureInfo("ru-RU"));
            }

            throw new Exception($"Server has returned incorrect response \n {plainText}");
        }

        private static void SaveLoginAndPassword(string login, string pass)
        {
            try
            {
                var key = GetAppSubKey();

                foreach (var name in key.GetValueNames())
                    key.DeleteValue(name);
                var pass64 = Convert.ToBase64String(Protect(Encoding.ASCII.GetBytes(pass)));
                key.SetValue(login, pass64, RegistryValueKind.String);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error on saving login and password: " + e.Message);
            }
        }

        private static void SaveLimitDate(string limitDate)
        {
            var cpass = DateTime.UtcNow.Date.ToString(CultureInfo.GetCultureInfo("ru-RU"));
            var csalt = Encoding.ASCII.GetBytes(_hwId);

            var c = Enc(limitDate, cpass, csalt);

            var dir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DynoStudio");

            var exFilepath = Path.Combine(dir, "ProrubimExternal");

            try
            {
                if (File.Exists(exFilepath))
                    FileSystem.DeleteFile(exFilepath, UIOption.OnlyErrorDialogs, RecycleOption.DeletePermanently);
                File.WriteAllBytes(exFilepath, Convert.FromBase64String(c));
                File.SetCreationTime(exFilepath, File.GetLastAccessTime(exFilepath));
            }
            catch (Exception)
            {
                // ignored
                //       throw new Exception("Could not save license file");
            }
        }

        private static void CheckInternetConnection()
        {
            try
            {
                Dns.GetHostEntry("content.prorubim.com");
            }
            catch (Exception)
            {
                throw new Exception("No internet connection or bad connect quality");
            }
        }

        public static byte[] Pbkdf2Sha256GetBytes(int dklen, byte[] password, byte[] salt, int iterationCount)
        {
            using (var hmac = new HMACSHA256(password))
            {
                var hashLength = hmac.HashSize / 8;
                if ((hmac.HashSize & 7) != 0)
                    hashLength++;
                var keyLength = dklen / hashLength;
                if (dklen > (0xFFFFFFFFL * hashLength) || dklen < 0)
                    throw new ArgumentOutOfRangeException(nameof(dklen));
                if (dklen % hashLength != 0)
                    keyLength++;
                var extendedkey = new byte[salt.Length + 4];
                Buffer.BlockCopy(salt, 0, extendedkey, 0, salt.Length);
                using (var ms = new MemoryStream())
                {
                    for (var i = 0; i < keyLength; i++)
                    {
                        extendedkey[salt.Length] = (byte) (((i + 1) >> 24) & 0xFF);
                        extendedkey[salt.Length + 1] = (byte) (((i + 1) >> 16) & 0xFF);
                        extendedkey[salt.Length + 2] = (byte) (((i + 1) >> 8) & 0xFF);
                        extendedkey[salt.Length + 3] = (byte) (((i + 1)) & 0xFF);
                        var u = hmac.ComputeHash(extendedkey);
                        Array.Clear(extendedkey, salt.Length, 4);
                        var f = u;
                        for (var j = 1; j < iterationCount; j++)
                        {
                            u = hmac.ComputeHash(u);
                            for (int k = 0; k < f.Length; k++)
                                f[k] ^= u[k];
                        }

                        ms.Write(f, 0, f.Length);
                        Array.Clear(u, 0, u.Length);
                        Array.Clear(f, 0, f.Length);
                    }

                    var dk = new byte[dklen];
                    ms.Position = 0;
                    ms.Read(dk, 0, dklen);
                    ms.Position = 0;
                    for (long i = 0; i < ms.Length; i++)
                    {
                        ms.WriteByte(0);
                    }

                    Array.Clear(extendedkey, 0, extendedkey.Length);
                    return dk;
                }
            }
        }

        private static string GetHwId()
        {
            var str = "";
            var searcher = new ManagementObjectSearcher();

            try
            {
                foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
                    if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                        nic.OperationalStatus == OperationalStatus.Up)
                        str += nic.GetPhysicalAddress();
            }
            catch
            {
                // ignored
            }

            searcher.Query = new ObjectQuery("select * from Win32_Processor");
            foreach (var managementBaseObject in searcher.Get())
            {
                var managementObject = (ManagementObject) managementBaseObject;
                str += managementObject.Properties["ProcessorId"].Value.ToString();
            }

            searcher.Query = new ObjectQuery("select * from Win32_BaseBoard");
            foreach (var managementBaseObject in searcher.Get())
            {
                var managementObject = (ManagementObject) managementBaseObject;
                str += managementObject.Properties["Product"].Value.ToString();
            }

            return str;
        }

        public static byte[] Protect(byte[] data)
        {
            try
            {
                // Encrypt the data using DataProtectionScope.CurrentUser. The result can be decrypted
                //  only by the same current user.
                return ProtectedData.Protect(data, _hwIdBytes, DataProtectionScope.CurrentUser);
            }
            catch (CryptographicException)
            {
                return null;
            }
        }

        public static byte[] Unprotect(byte[] data)
        {
            try
            {
                //Decrypt the data using DataProtectionScope.CurrentUser.
                return ProtectedData.Unprotect(data, _hwIdBytes, DataProtectionScope.CurrentUser);
            }
            catch (CryptographicException)
            {
                return null;
            }
        }

        public static string Enc(string plainText, string keyWord, byte[] salt)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException(nameof(plainText));
            if (string.IsNullOrEmpty(keyWord))
                throw new ArgumentNullException("sharedSecret");

            string outStr;
            RijndaelManaged aesAlg = null;

            try
            {
                var bkey = Pbkdf2Sha256GetBytes(32, Encoding.ASCII.GetBytes(keyWord), salt, 1000);

                aesAlg = new RijndaelManaged
                {
                    Key = bkey,
                    Padding = PaddingMode.Zeros,
                    BlockSize = 128,
                    IV = Encoding.ASCII.GetBytes("1234567891234567")
                };

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (var msEncrypt = new MemoryStream())
                {
                    msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }

                    outStr = Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
            finally
            {
                aesAlg?.Clear();
            }

            return outStr;
        }

        public static string Desc(string cipherText, string keyWord, byte[] salt)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException(nameof(cipherText));
            if (string.IsNullOrEmpty(keyWord))
                throw new ArgumentNullException("sharedSecret");
            RijndaelManaged aesAlg = null;
            string plaintext = null;

            try
            {
                var bkey = Pbkdf2Sha256GetBytes(32, Encoding.ASCII.GetBytes(keyWord), salt, 1000);
                var bytes = Convert.FromBase64String(cipherText);

                using (var msDecrypt = new MemoryStream(bytes))
                {
                    aesAlg = new RijndaelManaged
                    {
                        Key = bkey,
                        Padding = PaddingMode.Zeros,
                        BlockSize = 128,
                        IV = ReadByteArray(msDecrypt)
                    };

                    var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                            try
                            {
                                plaintext = srDecrypt.ReadToEnd().Trim('\0');
                            }
                            catch (Exception)
                            {
                                // ignored
                            }
                    }
                }
            }
            finally
            {
                aesAlg?.Clear();
            }

            return plaintext;
        }

        private static byte[] ReadByteArray(Stream s)
        {
            var rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
            }

            var buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
            }

            return buffer;
        }
    }
}