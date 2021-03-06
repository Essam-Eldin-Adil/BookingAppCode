
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Domain
{
    public static class File
    {
        //private static readonly;
        public static string Get(HttpContext httpContext, Guid id)
        {
            var fileRepository = (IRepository<Data.Models.File>)httpContext.RequestServices.GetService(typeof(IRepository<Data.Models.File>));
            var file = fileRepository.Find(id);
            if (file != null)
            {
                return "data:" + file.Type + ";base64, " + file.FileContent;// Convert.ToBase64String();

            }
            return null;
        }

        public static string GetExtension(string name)
        {
            return Path.GetExtension(name).Replace(".", "");
        }
        public static string SizeSuffix(Int64 value, int decimalPlaces = 1)
        {
            string[] SizeSuffixes = { Resource.Byte, Resource.KiloByte, Resource.MegaByte, Resource.GigaByte, "TB", "PB", "EB", "ZB", "YB" };

            if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException("decimalPlaces"); }
            if (value < 0) { return "-" + SizeSuffix(-value); }
            if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} bytes", 0); }

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            int mag = (int)Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}",
                adjustedSize,
                SizeSuffixes[mag]);
        }
        public static Guid Upload(IRepository<Data.Models.File> fileRepository, IFormFile formfile, bool secure = false)
        {
            try
            {
                if (formfile.Length > 0)
                {
                    var file = new Data.Models.File();
                    file.Name = formfile.FileName;
                    using (var ms = new MemoryStream())
                    {
                        formfile.CopyTo(ms);
                        file.FileContent = Convert.ToBase64String(ms.ToArray());
                    }
                    file.Size = formfile.Length;
                    file.Type = formfile.ContentType;
                    file.IsSecure = secure;
                    var result = fileRepository.Add(file);
                    return result;
                }
            }
            catch (Exception ex)
            {
                return new Guid();
            }

            return new Guid();
        }
        public static List<Guid> Upload(IRepository<Data.Models.File> fileRepository, IEnumerable<IFormFile> formfiles, bool secure = false)
        {
            List<Guid> guids = new List<Guid>();

            foreach (FormFile ff in formfiles)
            {
                try
                {
                    var file = new Data.Models.File();
                    file.Name = ff.FileName;
                    using (var ms = new MemoryStream())
                    {
                        ff.CopyTo(ms);
                        file.FileContent = Convert.ToBase64String(ms.ToArray());
                    }
                    file.Size = ff.Length;
                    file.Type = ff.ContentType;
                    file.IsSecure = secure;
                    var result = fileRepository.Add(file);
                    guids.Add(result);
                }
                catch
                {

                }

            }
            return guids;
        }
        public static List<Guid> Upload(string directory,IRepository<Data.Models.File> fileRepository, IEnumerable<IFormFile> formfiles, bool secure = false)
        {
            List<Guid> guids = new List<Guid>();

            foreach (FormFile ff in formfiles)
            {
                try
                {
                    var file = new Data.Models.File();
                    file.Name = ff.FileName;
                    var path = Path.Combine(
                           Directory.GetCurrentDirectory(), "wwwroot", "_uploads", directory,
                           ff.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        ff.CopyTo(stream);
                    }
                    file.FileContent = Path.Combine("_uploads", directory,ff.FileName);
                    file.Size = ff.Length;
                    file.Type = ff.ContentType;
                    file.IsSecure = secure;
                    

                    //Resize Images
                    if (directory== "ChaletImages"|| directory== "UnitImages")
                    {
                        var filePath = Path.Combine(
                           Directory.GetCurrentDirectory(), "wwwroot", "_uploads", directory, "Thumb",
                           ff.FileName);
                        Image image = Image.FromStream(ff.OpenReadStream(), true, true);
                        var newImage = new Bitmap(200, 200);
                        using (var g = Graphics.FromImage(newImage))
                        {
                            g.DrawImage(image, 0, 0, 200, 200);
                        }
                        file.FileContentMin= Path.Combine("_uploads", directory, "Thumb",
                           ff.FileName);
                        newImage.Save(filePath);
                    }
                    var result = fileRepository.Add(file);
                    guids.Add(result);
                }
                catch(Exception ex)
                {

                }

            }
            return guids;
        }
        public static string Upload(IFormFile formfile, string directory, string name)
        {
            if (formfile.Length > 0)
            {

                var path = Path.Combine(
                           Directory.GetCurrentDirectory(), "wwwroot", "_uploads",
                           formfile.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    formfile.CopyToAsync(stream);
                }

                return path;
            }

            return null;
        }
        public static bool Remove(string path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                return true;
            }

            return false;
        }
        public static bool Remove(IRepository<Data.Models.File> fileRepository, Guid id)
        {
            var file = fileRepository.Find(id);
            var filePath = Path.Combine(
                           Directory.GetCurrentDirectory(), "wwwroot", file.FileContent);
            
            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    System.IO.File.Delete(filePath);
                }
                catch
                {
                }
            }

            if (!string.IsNullOrEmpty(file.FileContentMin))
            {
                var filePathMin = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot", file.FileContentMin);
                if (System.IO.File.Exists(filePathMin))
                {
                    try
                    {
                        System.IO.File.Delete(filePathMin);
                    }
                    catch
                    {
                    }
                }
            }
            fileRepository.RemoveHard(id);
            return true;
        }
        public static string ConvertImageURLToBase64(String url)
        {
            StringBuilder _sb = new StringBuilder();
            Byte[] _byte = GetImage(url);
            _sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length));
            return _sb.ToString();
        }

        private static byte[] GetImage(string url)
        {
            Stream stream = null;
            byte[] buf;
            try
            {
                WebProxy myProxy = new WebProxy();
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                stream = response.GetResponseStream();
                using (BinaryReader br = new BinaryReader(stream))
                {
                    int len = (int)(response.ContentLength);
                    buf = br.ReadBytes(len);
                    br.Close();
                }
                stream.Close();
                response.Close();
            }
            catch (Exception)
            {
                buf = null;
            }
            return (buf);
        }


    }
}
