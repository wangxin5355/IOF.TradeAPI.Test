using System;
using System.IO;
using Microsoft.Extensions.FileProviders;
using SharpCifs.Smb;

namespace IQF.Framework.Util
{
    public class SharedFoldersHelper
    {
        private static readonly string SharedFolderUser = "serviceuser";
        private static readonly string SharedFolderPassword = "!QAZ2wsx";
        private static readonly string SharedFolderIp = "192.168.88.80";
        private static readonly string MidPath = Path.Combine("www", "inquantimg");
        private static readonly string BaseSmbFilePath;
        private static string UrlPrefix = "https://img.inquant.cn";
        static SharedFoldersHelper()
        {
            BaseSmbFilePath = $"smb://{SharedFolderUser}:{SharedFolderPassword}@{SharedFolderIp}/{MidPath}";
        }

        public static bool UpLoadFile(string serverDir, string serverFileName, string localFilePath, out string url)
        {
            var space = '/';
            if (localFilePath.Contains("\\")) // linux 路径间隔使用/ windows 使用\
            {
                space = '\\';
            }
            var root = localFilePath.Substring(0, localFilePath.LastIndexOf(space) + 1);
            var provider = new PhysicalFileProvider(root);
            var localFileName = localFilePath.Substring(localFilePath.LastIndexOf(space) + 1);
            var info = provider.GetFileInfo(localFileName);
            var stream = info.CreateReadStream();
            return UpLoadFile(serverDir, serverFileName, stream, out url);
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="serverDir">服务器目录 如 live/img</param>
        /// <param name="serverFileName">上传文件名</param>
        /// <param name="stream"></param>
        /// <param name="url">返回服务器路径 https://img.inquant.cn/** </param>
        /// <returns></returns>
        public static bool UpLoadFile(string serverDir, string serverFileName, Stream stream, out string url)
        {

            byte[] bytes = null;
            using (var binaryReader = new BinaryReader(stream))
            {
                bytes = binaryReader.ReadBytes((int)stream.Length);
            }
            return UpLoadFile(serverDir, serverFileName, bytes, out url);
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="serverDir"></param>
        /// <param name="serverFileName"></param>
        /// <param name="bufBytes"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool UpLoadFile(string serverDir, string serverFileName, byte[] bufBytes, out string url)
        {
            if (string.IsNullOrWhiteSpace(serverDir)
                || string.IsNullOrWhiteSpace(serverFileName)
                || bufBytes == null || bufBytes.Length <= 0)
            {
                throw new Exception("文件上传参数异常");
            }
            var folder = new SmbFile($"{BaseSmbFilePath}/{serverDir}");
            if (!folder.Exists())
            {
                throw new Exception($"服务器不存在当前目录{serverDir}");
            }
            var file = new SmbFile($"{BaseSmbFilePath}/{serverDir}/{serverFileName}");
            if (file.Exists())
            {
                throw new Exception($"服务器已存在此文件:/{serverDir}/{serverFileName}");
            }
            file.CreateNewFile();
            //Get writable stream.
            var writeStream = file.GetOutputStream();
            //Write bytes.
            writeStream.Write(bufBytes);
            //Dispose writable stream.
            writeStream.Dispose();
            url = String.Format(@"{0}/{1}/{2}", UrlPrefix, serverDir, serverFileName);
            return true;
        }


    }
}