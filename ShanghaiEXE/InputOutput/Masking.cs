using System;
using System.Security.Cryptography;
using System.Text;

namespace NSShanghaiEXE.InputOutput
{
    internal static class Masking
    {
        public static void GenerateKeyFromPassword(
          string password,
          int keySize,
          out byte[] key,
          int blockSize,
          out byte[] iv)
        {
            byte[] bytes = Encoding.UTF8.GetBytes("sasanasi");
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, bytes)
            {
                IterationCount = 1000
            };
            key = rfc2898DeriveBytes.GetBytes(keySize / 8);
            iv = rfc2898DeriveBytes.GetBytes(blockSize / 8);
        }

        public static string DecryptString(string sourceString, string password)
        {
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            Masking.GenerateKeyFromPassword(password, rijndaelManaged.KeySize, out byte[] key, rijndaelManaged.BlockSize, out byte[] iv);
            rijndaelManaged.Key = key;
            rijndaelManaged.IV = iv;
            byte[] inputBuffer = Convert.FromBase64String(sourceString);
            ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor();
            byte[] bytes = decryptor.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
            decryptor.Dispose();
            return Encoding.UTF8.GetString(bytes);
        }

        public static byte DecryptByte(byte code, byte[] key, byte[] iv, RijndaelManaged rijndael)
        {
            rijndael.Key = key;
            rijndael.IV = iv;
            byte[] inputBuffer = new byte[1] { code };
            rijndael.Padding = PaddingMode.Zeros;
            ICryptoTransform decryptor = rijndael.CreateDecryptor();
            byte[] numArray = decryptor.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
            decryptor.Dispose();
            return numArray[0];
        }

        public static byte DecryptByte(byte code)
        {
            return (byte)(~(uint)code);
        }

        public static byte EncryptByte(byte code, byte[] key, byte[] iv, RijndaelManaged rijndael)
        {
            rijndael.Key = key;
            rijndael.IV = iv;
            byte[] inputBuffer = new byte[1] { code };
            ICryptoTransform encryptor = rijndael.CreateEncryptor();
            byte[] numArray = encryptor.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
            encryptor.Dispose();
            return numArray[0];
        }

        public static byte EncryptByte(byte code)
        {
            return (byte)(~(uint)code);
        }
    }
}
