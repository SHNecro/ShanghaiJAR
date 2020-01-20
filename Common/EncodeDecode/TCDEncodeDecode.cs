using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows;

namespace Common.EncodeDecode
{
    public static class TCDEncodeDecode
    {
        private const string DefaultPassword = "sasanasi";

        private const string ResourceSuffix = "Resource.tcd";
        private const string PatternSuffix = "Pattern.tcd";

        public static string EncMapScript(string str)
        {
            var byteList = str.Split(',').Where(s => !string.IsNullOrEmpty(s)).Select(s => (byte)~int.Parse(s)).ToArray();
            return Encoding.GetEncoding("Shift_JIS").GetString(byteList);
        }

        public static string DecryptString(string sourceString, string password)
        {
            var rijndaelManaged = new RijndaelManaged();
            TCDEncodeDecode.GenerateKeyFromPassword(password, rijndaelManaged.KeySize, out byte[] key, rijndaelManaged.BlockSize, out byte[] iv);
            rijndaelManaged.Key = key;
            rijndaelManaged.IV = iv;
            var inputBuffer = Convert.FromBase64String(sourceString);
            var decryptor = rijndaelManaged.CreateDecryptor();
            var bytes = decryptor.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
            decryptor.Dispose();
            return Encoding.UTF8.GetString(bytes);
        }

        public static string EncryptString(StringBuilder sourceString, string password)
        {
            var rijndaelManaged = new RijndaelManaged();
            TCDEncodeDecode.GenerateKeyFromPassword(password, rijndaelManaged.KeySize, out byte[] key, rijndaelManaged.BlockSize, out byte[] iv);
            rijndaelManaged.Key = key;
            rijndaelManaged.IV = iv;
            var bytes = Encoding.UTF8.GetBytes(sourceString.ToString());
            var encryptor = rijndaelManaged.CreateEncryptor();
            var inArray = encryptor.TransformFinalBlock(bytes, 0, bytes.Length);
            encryptor.Dispose();
            return Convert.ToBase64String(inArray);
        }

        public static void PackTCDFile(string directoryPath, string outputPrefix, Action<string, double> progressCallback)
        {
            var fullPath = new DirectoryInfo(Path.GetFullPath(directoryPath));
            var patternFile = Path.Combine(fullPath.Parent.FullName, $"{outputPrefix}{TCDEncodeDecode.PatternSuffix}");
            var resourceFile = Path.Combine(fullPath.Parent.FullName, $"{outputPrefix}{TCDEncodeDecode.ResourceSuffix}");

            var files = Directory.EnumerateFiles(directoryPath).ToList();
            files.Sort();

            var packThread = new Thread(() =>
            {
                try
                {
                    using (var sw = new StreamWriter(new FileStream(patternFile, FileMode.Create)))
                    {
                        using (var bw = new BinaryWriter(new FileStream(resourceFile, FileMode.Create)))
                        {
                            var fileCount = files.Count;
                            var fileNumber = 0;
                            foreach (var file in files)
                            {
                                using (var br = new BinaryReader(File.OpenRead(file), Encoding.GetEncoding("Shift_JIS")))
                                {
                                    var size = br.BaseStream.Length;
                                    var fileName = Path.GetFileName(file);
                                    var pattern = $"{fileName}_{size}";

                                    progressCallback(fileName, (double)fileNumber / fileCount);

                                    var rawBuffer = br.ReadBytes((int)size);
                                    var buffer = rawBuffer.Select((b, i) => i > 1024 ? (byte)b : (byte)(~b & 0xff)).ToArray();
                                    bw.Write(buffer);

                                    var encryptedPattern = TCDEncodeDecode.EncryptString(new StringBuilder(pattern), TCDEncodeDecode.DefaultPassword);
                                    sw.Write($"{encryptedPattern}\r\n");

                                    fileNumber += 1;
                                }
                            }
                        }
                    }
                }
                finally
                {
                    progressCallback(null, 0);
                }
            });

            packThread.Start();
        }

        public static void UnpackTCDFile(string directoryPath, string inputPrefix, Action<string, double> progressCallback)
        {
            var fullPath = new DirectoryInfo(Path.GetFullPath(directoryPath));
            var patternFile = Path.Combine(fullPath.Parent.FullName, $"{inputPrefix}{TCDEncodeDecode.PatternSuffix}");
            var resourceFile = Path.Combine(fullPath.Parent.FullName, $"{inputPrefix}{TCDEncodeDecode.ResourceSuffix}");

            Directory.CreateDirectory(directoryPath);

            var unpackThread = new Thread(() =>
            {
                try
                {
                    using (var rsr = new BinaryReader(File.OpenRead(resourceFile), Encoding.GetEncoding("Shift_JIS")))
                    {
                        using (var psr = new StreamReader(File.OpenRead(patternFile), Encoding.GetEncoding("Shift_JIS")))
                        {
                            var files = psr.ReadToEnd().Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                            var fileCount = files.Length;
                            var fileNumber = 0;
                            foreach (var file in files)
                            {
                                var pattern = TCDEncodeDecode.DecryptString(file, TCDEncodeDecode.DefaultPassword);

                                var parts = pattern.Split('_');
                                var fileName = string.Join("_", parts.Take(parts.Length - 1));
                                var newFile = Path.Combine(directoryPath, fileName);
                                var size = int.Parse(parts.Last());

                                progressCallback(fileName, (double)fileNumber / fileCount);

                                var rawBuffer = rsr.ReadBytes(size);

                                var buffer = rawBuffer.Select((b, i) => i > 1024 ? (byte)b : (byte)(~b & 0xff)).ToArray();
                                using (var bw = new BinaryWriter(new FileStream(newFile, FileMode.Create)))
                                {
                                    bw.Write(buffer);
                                }

                                fileNumber += 1;
                            }
                        }
                    }
                }
                finally
                {
                    progressCallback(null, 0);
                }
            });

            unpackThread.Start();
        }

        public static string ReadTextFile(string path, bool decode)
        {
            try
            {
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    var fileReader = new StreamReader(stream);
                    var fileContents = fileReader.ReadToEnd();
                    return decode ? TCDEncodeDecode.DecodeMap(fileContents) : fileContents;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"File could not be read.\n{e.GetType().ToString()}: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public static bool SaveFile(string path, string contents)
        {
            try
            {
                File.WriteAllText(path, contents);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show($"File could not be saved.\n{e.GetType().ToString()}: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static string DecodeMap(string encodedString)
        {
            var decodedLines = new List<string>();
            using (var reader = new StringReader(encodedString))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        decodedLines.Add(string.Empty);
                    }
                    else
                    {
                        decodedLines.Add(Encoding.GetEncoding("Shift_JIS").GetString(line.TrimEnd(',').Split(',').Select(byteString => (byte)~int.Parse(byteString)).ToArray()));
                    }
                }
            }
            return string.Join("\r\n", decodedLines);
        }

        public static string EncodeMap(string plainText)
        {
            var encodedLines = new List<string>();
            using (var reader = new StringReader(plainText))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        encodedLines.Add(string.Empty);
                    }
                    else
                    {
                        encodedLines.Add(string.Join(",", Encoding.GetEncoding("Shift_JIS").GetBytes(line).Select(b => (~b).ToString())) + ",");
                    }
                }
            }
            return string.Join("\r\n", encodedLines);
        }

        private static void GenerateKeyFromPassword(
          string password,
          int keySize,
          out byte[] key,
          int blockSize,
          out byte[] iv)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(TCDEncodeDecode.DefaultPassword);
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, bytes)
            {
                IterationCount = 1000
            };
            key = rfc2898DeriveBytes.GetBytes(keySize / 8);
            iv = rfc2898DeriveBytes.GetBytes(blockSize / 8);
        }
    }
}
