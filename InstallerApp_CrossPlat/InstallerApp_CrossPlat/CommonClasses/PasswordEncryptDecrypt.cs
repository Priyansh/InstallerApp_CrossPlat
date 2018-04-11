using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace InstallerApp_CrossPlat
{
    static internal class PasswordEncryptDecrypt
    {
        //NOTE : This Encypt-Decrypt Class is no longer in use, instead we are doing in Database:
        //DB Code in sql server
        /* USE [KPDataSQL]
            GO

            SET ANSI_NULLS ON
            GO
            SET QUOTED_IDENTIFIER ON
            GO
            ALTER PROCEDURE [dbo].[InsKP_Login]
	            (

		            @UserID nvarchar(50),
	
		            @Pwd varchar(MAX)
	            )
 
            AS

            DECLARE @ExistingPwdMatch varbinary(200)

            DECLARE @InstallerIdValue int

            IF EXISTS (SELECT UserName From tblInstallerUSERS WHERE UserName = lower(@UserID))

	            BEGIN

		            SET @ExistingPwdMatch = (SELECT Pwd FROM tblInstallerUSERS WHERE UserName= lower(@UserID))
		            IF @Pwd = convert(varchar(MAX),DecryptByPassPhrase('key', @ExistingPwdMatch ))

		            BEGIN

			            SELECT ISNULL(InstallerId,'0') AS InstallerId

			            FROM tblInstallerUSERS
			            WHERE UserName =  lower(@UserID)

		            END
	
		            ELSE
		            BEGIN

		            SET @InstallerIdValue = 0

		            SELECT @InstallerIdValue AS InstallerId

		            END

	            END    */



        // Define the secret salt value for encrypting data
        private static readonly byte[] salt = Encoding.ASCII.GetBytes("Xamarin.Android Version: 8.0.2.1");
        //private static readonly byte[] salt = Encoding.ASCII.GetBytes("Xamarin.iOS Version: 11.3.0.47");
        internal static string Encrypt(string textToEncrypt, string encryptionPassword)
        {
            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            byte[] clearBytes = Encoding.Unicode.GetBytes(textToEncrypt);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
                    0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    textToEncrypt = Convert.ToBase64String(ms.ToArray());
                }
            }
            return textToEncrypt;

            //var algorithm = GetAlgorithm(encryptionPassword);

            ////Anything to process?
            //if (textToEncrypt == null || textToEncrypt == "") return "";

            //byte[] encryptedBytes;
            //using (ICryptoTransform encryptor = algorithm.CreateEncryptor(algorithm.Key, algorithm.IV))
            //{
            //    byte[] bytesToEncrypt = Encoding.Unicode.GetBytes(textToEncrypt);
            //    encryptedBytes = InMemoryCrypt(bytesToEncrypt, encryptor);
            //}
            //return Convert.ToBase64String(encryptedBytes);
        }

        internal static string Decrypt(string encryptedText, string encryptionPassword)
        {
            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            encryptedText = encryptedText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(encryptedText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
                    0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    encryptedText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return encryptedText;

            //var algorithm = GetAlgorithm(encryptionPassword);

            ////Anything to process?
            //if (encryptedText == null || encryptedText == "") return "";

            //byte[] descryptedBytes;
            //using (ICryptoTransform decryptor = algorithm.CreateDecryptor(algorithm.Key, algorithm.IV))
            //{
            //    byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            //    descryptedBytes = InMemoryCrypt(encryptedBytes, decryptor);
            //}
            //return Encoding.Unicode.GetString(descryptedBytes);
        }

        private static byte[] InMemoryCrypt(byte[] data, ICryptoTransform transform)
        {
            MemoryStream memory = new MemoryStream();
            using (Stream stream = new CryptoStream(memory, transform, CryptoStreamMode.Write))
            {
                stream.Write(data, 0, data.Length);
            }
            return memory.ToArray();
        }

        private static RijndaelManaged GetAlgorithm(string encryptionPassword)
        {
            // Create an encryption key from the encryptionPassword and salt.
            var key = new Rfc2898DeriveBytes(encryptionPassword, salt);

            // Declare that we are going to use the Rijndael algorithm with the key that we've just got.
            var algorithm = new RijndaelManaged();
            int bytesForKey = algorithm.KeySize / 8;
            int bytesForIV = algorithm.BlockSize / 8;
            algorithm.Key = key.GetBytes(bytesForKey);
            algorithm.IV = key.GetBytes(bytesForIV);
            return algorithm;
        }
    }
}
