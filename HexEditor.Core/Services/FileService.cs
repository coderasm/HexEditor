using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace HexEditor.Core.Services
{
    public class FileService : IFileService
    {
        public StreamWriter Append(string path)
        {
            throw new NotImplementedException();
        }

        public void AppendAll(string path, string contents)
        {
            throw new NotImplementedException();
        }

        public FileStream CreateFile(string filePath)
        {
            return File.Create(filePath);
        }

        public bool Delete(string path)
        {
            File.Delete(path);
            return !File.Exists(path);
        }

        public StreamReader Read(string path)
        {
            throw new NotImplementedException();
        }

        public string ReadAll(string path)
        {
            throw new NotImplementedException();
        }

        public void WriteAll(string path, string contents)
        {
            File.WriteAllText(path, contents);
        }

        public string GetFileNameWithoutExtension(string fileNameWithPath)
        {
            return Path.GetFileNameWithoutExtension(fileNameWithPath);
        }

        public string GetFileName(string fileNameWithPath)
        {
            return Path.GetFileName(fileNameWithPath);
        }

        public string GetDirectoryName(string fileNameWithPath)
        {
            return Path.GetDirectoryName(fileNameWithPath);
        }

        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public void ConvertToHex(string filePath)
        {
            if (FileExists(filePath))
            {
                string[] digits = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };
                using (BinaryReader br = new BinaryReader(new FileStream
                        (
                        filePath,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.None,
                        1024)
                        )
                        )
                {
                    var path = GetDirectoryName(filePath);
                    var nameOnly = GetFileNameWithoutExtension(filePath);
                    using (StreamWriter sw = new StreamWriter($"{path}/{nameOnly}.hex"))
                    {
                        byte[] inbuff = new byte[0];
                        int b = 0;
                        while ((inbuff = br.ReadBytes(50)).Length > 0)
                        {
                            for (b = 0; b < inbuff.Length - 1; b++)
                            {
                                sw.Write(digits[(inbuff[b] / 16) % 16] + digits[inbuff[b] % 16] + " ");
                            }
                            sw.WriteLine(digits[(inbuff[b] / 16) % 16] + digits[inbuff[b] % 16]);

                        }
                    }
                }
            }
        }

        public void ConvertToBinary(string filePath)
        {
            if (FileExists(filePath))
            {
                using (StreamReader sr = new StreamReader(new FileStream
                        (
                        filePath,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.None,
                        1024)
                        )
                        )
                {
                    var path = GetDirectoryName(filePath);
                    var nameOnly = GetFileNameWithoutExtension(filePath);
                    using (FileStream fs = new FileStream($"{path}/{nameOnly}.bin", FileMode.Create))
                    {
                        var line = "";
                        while ((line = sr.ReadLine()) != null)
                        {
                            var hex = line.Replace(" ", "");
                            var bytes = ConvertHexToByteArray(hex);
                            foreach (var item in bytes)
                            {
                                fs.WriteByte(item);
                            }
                        }
                    }
                }
            }
        }

        //public void ConvertToHex(string filePath)
        //{
        //    if (FileExists(filePath))
        //    {
        //        var wholeFile = File.ReadAllBytes(filePath);
        //        var hex = ConvertByteToHex(wholeFile);
        //        var path = GetDirectoryName(filePath);
        //        var nameOnly = GetFileNameWithoutExtension(filePath);
        //        File.WriteAllText($"{path}/{nameOnly}.hex", hex);
        //    }
        //}

        //public void ConvertToBinary(string filePath)
        //{
        //    if (FileExists(filePath))
        //    {
        //        var wholeFile = File.ReadAllText(filePath).Replace(" ", "").Replace($"\r\n", "");
        //        var bytes = ConvertHexToByteArray(wholeFile);
        //        var path = GetDirectoryName(filePath);
        //        var nameOnly = GetFileNameWithoutExtension(filePath);
        //        File.WriteAllBytes($"{path}/{nameOnly}.bin", bytes);
        //    }
        //}

        public string ConvertByteToHex(byte[] byteData)
        {
            string hexValues = BitConverter.ToString(byteData).Replace("-", "");

            return hexValues;
        }


        // convert hex values of file back to bytes
        public byte[] ConvertHexToByteArray(string hexString)
        {
            byte[] byteArray = new byte[hexString.Length / 2];

            for (int index = 0; index < byteArray.Length; index++)
            {
                string byteValue = hexString.Substring(index * 2, 2);
                byteArray[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return byteArray;
        }

        public bool DirectoryExists(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        public void CreateDirectory(string directoryPath)
        {
            Directory.CreateDirectory(directoryPath);
        }
    }

    public interface IFileService
    {
        FileStream CreateFile(string path);
        void CreateDirectory(string directoryPath);
        void WriteAll(string path, string contents);
        StreamWriter Append(string path);
        void AppendAll(string path, string contents);
        StreamReader Read(string path);
        string ReadAll(string path);
        string GetFileNameWithoutExtension(string fileNameWithPath);
        string GetFileName(string fileNameWithPath);
        string GetDirectoryName(string fileNameWithPath);
        bool Delete(string path);
        bool FileExists(string filePath);
        bool DirectoryExists(string directoryPath);
        void ConvertToHex(string filePath);
        void ConvertToBinary(string filePath);
    }
}
