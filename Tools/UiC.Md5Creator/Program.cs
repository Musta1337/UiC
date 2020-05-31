using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Md5Creator
{
    class Program
    {
        static void Main(string[] args)
        {
            LocalMD5 = new List<FileRecord>();
            ScanLocal("./");
            File.WriteAllText(".\\UiC\\UiC.hashs", JsonConvert.SerializeObject(LocalMD5, Formatting.Indented));
        }

        public static List<FileRecord> LocalMD5;

        private static void ScanLocal(string dossier)
        {
            foreach (var item in Directory.EnumerateFiles(dossier, "*", SearchOption.AllDirectories))
            {
                if (item.EndsWith("UiC.hashs"))
                    continue;

                using (FileStream fs = File.OpenRead(item))
                {
                    MD5 md5Algorithm = MD5.Create();
                    byte[] hashBytes = md5Algorithm.ComputeHash(fs);
                    string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                    LocalMD5.Add(new FileRecord() {
                        Name = item.Replace(dossier, "").Replace(@"\", "/"),
                        Hash = hash
                    });

                    Console.WriteLine(item.Replace(@"\", "/") + "=" + hash);
                }
            }
        }
    }
}
