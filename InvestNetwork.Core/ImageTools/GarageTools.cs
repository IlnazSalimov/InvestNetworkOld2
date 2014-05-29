using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
 
namespace InvestNetwork.Core
{
    /// <summary>
    /// HTTP module that allows to resize image on the fly.
    /// Syntax: IMAGE_URL @ WIDTH x HEIGHT[OPTIONS]
    /// E.g.: /img/avatar.jpg@64x64scp
    /// Options may include:
    ///  s - do not enlarge image if original is smaller then destination size
    ///  c - preserve canvas size (will fill it with white color if destination size is large than original image)
    ///  p - use PNG compression instead of JPG (default) for output image
    /// </summary>
    public class ImageResizeModule : IHttpModule
    {
        private const long JpegQuality = 80L;
        private static readonly TimeSpan ClientCacheExpiration = TimeSpan.FromDays(1);
        private static readonly string CacheDir = Path.Combine(Path.GetTempPath(), "ImageResizeCache");
        private static readonly TimeSpan CacheExpiration = TimeSpan.FromDays(7);
        private static readonly object Sync = new object();
        private static bool _cacheCleaned;
 
        private readonly Regex _regex = new Regex(@"^(?<imagepath>[^?]+\.(gif|jpg|jpeg|png|bmp))@(?<width>\d{2,4})x(?<height>\d{2,4})(?<options>[spc]*)$",
                      RegexOptions.IgnoreCase | RegexOptions.Compiled);
 
        #region IHttpModule Members
 
        public void Init(HttpApplication application)
        {
            application.BeginRequest += ApplicationBeginRequest;
        }
 
        public void Dispose() { }
 
        #endregion
 
        private void ApplicationBeginRequest(object sender, EventArgs e)
        {
            var context = HttpContext.Current;
            var request = context.Request;
            string path = request.Url.LocalPath;
 
            // abort if no match
            var m = _regex.Match(path);
            if (!m.Success) return;
 
            // abort if physical path doesn't exist
            string pathAndSettings = context.Request.PhysicalPath;
            string physicalPath = pathAndSettings.Substring(0, pathAndSettings.LastIndexOf('@'));
            var imageFile = new FileInfo(physicalPath);
            if (!imageFile.Exists) return;
 
            // resize settings
            int width = int.Parse(m.Groups["width"].Value);
            int height = int.Parse(m.Groups["height"].Value);
            string options = m.Groups["options"].Value;
 
            bool doNotEnlarge = options.Contains("s");
            bool preserveCanvasSize = options.Contains("c");
            bool usePng = options.Contains("p");
 
            // abort on invalid settings
            if (width + height == 0) return;
 
            var cacheDir = new DirectoryInfo(CacheDir);
            if (!cacheDir.Exists)
            {
                cacheDir.Create();
            }
 
            string cacheName = GetMd5String(path.ToLowerInvariant()) + (usePng ? ".png" : ".jpg");
            var cacheFile = new FileInfo(Path.Combine(CacheDir, cacheName));
 
            lock (Sync)
            {
                // clean old cached files ones upon application start
                if (!_cacheCleaned)
                {
                    var deleteFilesOlderThan = DateTime.Now - CacheExpiration;
                    var cachedFiles = cacheDir.GetFiles();
                    foreach (var file in cachedFiles)
                    {
                        if (file.LastWriteTime < deleteFilesOlderThan)
                        {
                            file.Delete();
                        }
                    }
                    _cacheCleaned = true;
                }
                if (!cacheFile.Exists || cacheFile.LastWriteTime < imageFile.LastWriteTime)
                {
                    CreateResizedImageFile(imageFile.FullName, cacheFile.FullName, width, height, doNotEnlarge, preserveCanvasSize, usePng);
                }
            }
 
            var response = context.Response;
            response.Clear();
            response.Cache.SetLastModified(imageFile.LastWriteTime);
            response.Cache.SetExpires(DateTime.Now + ClientCacheExpiration);
            response.Cache.SetCacheability(HttpCacheability.Public);
            response.ContentType = usePng ? "image/png" : "image/jpeg";
            response.WriteFile(cacheFile.FullName);
            response.End();
        }
 
        #region Resize image
 
        private static void CreateResizedImageFile(string originalFile, string cacheFile, int width, int height, bool doNotEnlarge, bool preserveCanvasSize,
                                              bool usePng)
        {
            using (var imgIn = new Bitmap(originalFile))
            {
                using (var imgOut = CreateResizedImage(imgIn, width, height, doNotEnlarge, preserveCanvasSize))
                {
                    if (usePng)
                    {
                        imgOut.Save(cacheFile, ImageFormat.Png);
                    }
                    else
                    {
                        var info = ImageCodecInfo.GetImageEncoders();
                        var encoderParameters = new EncoderParameters(1);
                        encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, JpegQuality);
                        imgOut.Save(cacheFile, info[1], encoderParameters);
                    }
                }
            }
        }
 
        private static Bitmap CreateResizedImage(Image image, int width, int height, bool doNotEnlarge, bool preserveCanvasSize)
        {
            double y = image.Height;
            double x = image.Width;
 
            double factorX = double.MaxValue;
            double factorY = double.MaxValue;
            if (width > 0)
            {
                factorX = width / x;
            }
            if (height > 0)
            {
                factorY = height / y;
            }
            double factor = Math.Min(factorX, factorY);
 
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (factor == double.MaxValue)
            // ReSharper restore CompareOfFloatsByEqualityOperator
            {
                throw new Exception("Invalid height and width parameters");
            }
 
            if (doNotEnlarge && factor > 1)
            {
                factor = 1;
            }
 
            var innerWidth = (int)(x * factor);
            var innerHeight = (int)(y * factor);
 
            int outerWidth = preserveCanvasSize ? width : innerWidth;
            int outerHeight = preserveCanvasSize ? height : innerHeight;
 
            int paddingWidth = (outerWidth - innerWidth) / 2;
            int paddingHeight = (outerHeight - innerHeight) / 2;
 
            int rectX = paddingWidth;
            int rectY = paddingHeight;
            int rectWidth = innerWidth;
            int rectHeight = innerHeight;
 
            // add extra pixels to crop semi-white spaces
            if (paddingWidth == 0)
            {
                rectX -= 2;
                rectWidth += 4;
            }
 
            if (paddingHeight == 0)
            {
                rectY -= 2;
                rectHeight += 4;
            }
 
            var destRect = new Rectangle(rectX, rectY, rectWidth, rectHeight);
            var result = new Bitmap(outerWidth, outerHeight);
            using (var g = Graphics.FromImage(result))
            {
                g.Clear(Color.White);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
 
                g.DrawImage(image, destRect, new Rectangle(0, 0, (int)x, (int)y), GraphicsUnit.Pixel);
            }
 
            return result;
        }
 
        #endregion
 
        #region Helper routines
 
        private static string GetMd5String(string value)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(value);
            MD5 md5 = new MD5CryptoServiceProvider();
            return ToHexString(md5.ComputeHash(bytes));
        }
 
        private static string ToHexString(IEnumerable<byte> array)
        {
            var builder = new StringBuilder();
            foreach (byte b in array)
            {
                builder.Append(b.ToString("X").PadLeft(2, '0'));
            }
            return builder.ToString();
        }
 
        #endregion
    }
}
