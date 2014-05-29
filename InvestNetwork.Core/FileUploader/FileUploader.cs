using InvestNetwork.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InvestNetwork.Core
{
    public static class FileUploader
    {
        private static string contentFolderRoot = ConfigurationManager.AppSettings["FileUploadDirectory"].ToString();
        private const string DefaultFilter = "*.png,*.gif,*.jpg,*.jpeg";

        private const int ThumbnailHeight = 80;
        private const int ThumbnailWidth = 80;

        private static IInvestContext _investContext = DependencyResolver.Current.GetService<IInvestContext>();
        private static IUserRepository _userRepository = DependencyResolver.Current.GetService<IUserRepository>();

        private static string ContentPath
        {
            get
            {
                return Path.Combine(contentFolderRoot, _investContext.CurrentUser.FilesBrowserDirectory);  
            }
        }

        private static string ToAbsolute(string virtualPath)
        {
            return VirtualPathUtility.ToAbsolute(virtualPath);
        }

        private static string CombinePaths(string basePath, string relativePath)
        {
            return VirtualPathUtility.Combine(VirtualPathUtility.AppendTrailingSlash(basePath), relativePath);
        }

        private static bool AuthorizeRead(string path)
        {
            return CanAccess(path);
        }

        private static bool CanAccess(string path)
        {
            return path.StartsWith(ToAbsolute(ContentPath), StringComparison.OrdinalIgnoreCase);
        }

        private static string NormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return ToAbsolute(ContentPath);
            }

            return CombinePaths(ToAbsolute(ContentPath), path);
        }


        private static bool AuthorizeDeleteFile(string path)
        {
            return CanAccess(path);
        }

        public static void DeleteFile(string path)
        {
            if (!AuthorizeDeleteFile(path))
            {
                throw new HttpException(403, "Forbidden");
            }

            var physicalPath = HttpContext.Current.Server.MapPath(path);

            if (System.IO.File.Exists(physicalPath))
            {
                System.IO.File.Delete(physicalPath);
            }
        }

        private static bool AuthorizeCreateDirectory(string path, string name)
        {
            return CanAccess(path);
        }


        private static bool AuthorizeUpload(string path, HttpPostedFileBase file)
        {
            return CanAccess(path) && IsValidFile(file.FileName);
        }

        private static bool IsValidFile(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            var allowedExtensions = DefaultFilter.Split(',');

            return allowedExtensions.Any(e => e.EndsWith(extension, StringComparison.InvariantCultureIgnoreCase));
        }

        public static string Upload(HttpPostedFileBase file, string path = "")
        {
            path = NormalizePath(path);
            var fileName = Path.GetFileName(file.FileName);

            if (AuthorizeUpload(path, file))
            {
                file.SaveAs(Path.Combine(HttpContext.Current.Server.MapPath(path), fileName));

                return ToAbsolute(Path.Combine(path, fileName));
            }

            throw new HttpException(403, "Forbidden");
        }

        private static bool AuthorizeImage(string path)
        {
            return CanAccess(path) && IsValidFile(Path.GetExtension(path));
        }
    }
}