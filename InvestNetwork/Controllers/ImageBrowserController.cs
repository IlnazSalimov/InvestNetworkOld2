﻿using InvestNetwork.Core;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InvestNetwork.Controllers
{
    [Authorize]
    public class ImageBrowserController : Controller
    {
        private string contentFolderRoot = ConfigurationManager.AppSettings["FileUploadDirectory"].ToString();
        private const string DefaultFilter = "*.png,*.gif,*.jpg,*.jpeg";

        private const int ThumbnailHeight = 80;
        private const int ThumbnailWidth = 80;

        private readonly IInvestContext _investContext;
        private readonly IUserRepository _userRepository;
        private readonly DirectoryBrowser directoryBrowser;
        private readonly ThumbnailCreator thumbnailCreator;

        public ImageBrowserController(IInvestContext investContext, IUserRepository userRepository)
        {
            this.directoryBrowser = new DirectoryBrowser();
            this.thumbnailCreator = new ThumbnailCreator();
            this._investContext = investContext;
            this._userRepository = userRepository;
        }

        public string ContentPath
        {
            get
            {
                return Path.Combine(contentFolderRoot, _investContext.CurrentUser.FilesBrowserDirectory);  
            }
        }

        private string ToAbsolute(string virtualPath)
        {
            return VirtualPathUtility.ToAbsolute(virtualPath);
        }

        private string CombinePaths(string basePath, string relativePath)
        {
            return VirtualPathUtility.Combine(VirtualPathUtility.AppendTrailingSlash(basePath), relativePath);
        }

        public virtual bool AuthorizeRead(string path)
        {
            return CanAccess(path);
        }

        protected virtual bool CanAccess(string path)
        {
            return path.StartsWith(ToAbsolute(ContentPath), StringComparison.OrdinalIgnoreCase);
        }

        private string NormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return ToAbsolute(ContentPath);
            }

            return CombinePaths(ToAbsolute(ContentPath), path);
        }

        public virtual JsonResult Read(string path, string rootDirectory)
        {
            path = NormalizePath(Path.Combine(rootDirectory, path));

            if (AuthorizeRead(path))
            {
                try
                {
                    directoryBrowser.Server = Server;

                    var result = directoryBrowser
                        .GetContent(path, DefaultFilter)
                        .Select(f => new
                        {
                            name = f.Name,
                            type = f.Type == EntryType.File ? "f" : "d",
                            size = f.Size
                        });

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                catch (DirectoryNotFoundException)
                {
                    throw new HttpException(404, "File Not Found");
                }
            }

            throw new HttpException(403, "Forbidden");
        }


        public virtual bool AuthorizeThumbnail(string path)
        {
            return CanAccess(path);
        }

        [OutputCache(Duration = 3600, VaryByParam = "path")]
        public virtual ActionResult Thumbnail(string path, string rootDirectory)
        {
            path = NormalizePath(Path.Combine(rootDirectory, path));

            if (AuthorizeThumbnail(path))
            {
                var physicalPath = Server.MapPath(path);

                if (System.IO.File.Exists(physicalPath))
                {
                    Response.AddFileDependency(physicalPath);

                    return CreateThumbnail(physicalPath);
                }
                else
                {
                    throw new HttpException(404, "File Not Found");
                }
            }
            else
            {
                throw new HttpException(403, "Forbidden");
            }
        }

        private FileContentResult CreateThumbnail(string physicalPath)
        {
            using (var fileStream = System.IO.File.OpenRead(physicalPath))
            {
                var desiredSize = new ImageSize
                {
                    Width = ThumbnailWidth,
                    Height = ThumbnailHeight
                };

                const string contentType = "image/png";

                return File(thumbnailCreator.Create(fileStream, desiredSize, contentType), contentType);
            }
        }

        [System.Web.Mvc.AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Destroy(string path, string name, string type, string rootDirectory)
        {
            path = NormalizePath(Path.Combine(rootDirectory, path));

            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(type))
            {
                path = CombinePaths(path, name);
                if (type.ToLowerInvariant() == "f")
                {
                    DeleteFile(path);
                }
                else
                {
                    DeleteDirectory(path);
                }

                return Json(null);
            }
            throw new HttpException(404, "File Not Found");
        }

        public virtual bool AuthorizeDeleteFile(string path)
        {
            return CanAccess(path);
        }

        public virtual bool AuthorizeDeleteDirectory(string path)
        {
            return CanAccess(path);
        }

        protected virtual void DeleteFile(string path)
        {
            if (!AuthorizeDeleteFile(path))
            {
                throw new HttpException(403, "Forbidden");
            }

            var physicalPath = Server.MapPath(path);

            if (System.IO.File.Exists(physicalPath))
            {
                System.IO.File.Delete(physicalPath);
            }
        }

        protected virtual void DeleteDirectory(string path)
        {
            if (!AuthorizeDeleteDirectory(path))
            {
                throw new HttpException(403, "Forbidden");
            }

            var physicalPath = Server.MapPath(path);

            if (Directory.Exists(physicalPath))
            {
                Directory.Delete(physicalPath, true);
            }
        }

        public virtual bool AuthorizeCreateDirectory(string path, string name)
        {
            return CanAccess(path);
        }

        [System.Web.Mvc.AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Create(string path, FileBrowserEntry entry, string rootDirectory)
        {
            path = NormalizePath(Path.Combine(rootDirectory, path));
            var name = entry.Name;

            if (!string.IsNullOrEmpty(name) && AuthorizeCreateDirectory(path, name))
            {
                var physicalPath = Path.Combine(Server.MapPath(path), name);

                if (!Directory.Exists(physicalPath))
                {
                    Directory.CreateDirectory(physicalPath);
                }

                return Json(null);
            }

            throw new HttpException(403, "Forbidden");
        }


        public virtual bool AuthorizeUpload(string path, HttpPostedFileBase file)
        {
            return CanAccess(path) && IsValidFile(file.FileName);
        }

        private bool IsValidFile(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            var allowedExtensions = DefaultFilter.Split(',');

            return allowedExtensions.Any(e => e.EndsWith(extension, StringComparison.InvariantCultureIgnoreCase));
        }

        [System.Web.Mvc.AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Upload(string path, HttpPostedFileBase file, string rootDirectory)
        {
            path = NormalizePath(Path.Combine(rootDirectory, path));
            var fileName = Path.GetFileName(file.FileName);

            if (AuthorizeUpload(path, file))
            {
                file.SaveAs(Path.Combine(Server.MapPath(path), fileName));

                return Json(new
                {
                    size = file.ContentLength,
                    name = fileName,
                    type = "f"
                }, "text/plain");
            }

            throw new HttpException(403, "Forbidden");
        }

        [OutputCache(Duration = 360, VaryByParam = "path")]
        public ActionResult Image(string path, string rootDirectory)
        {
            path = NormalizePath(Path.Combine(rootDirectory, path));

            if (AuthorizeImage(path))
            {
                var physicalPath = Server.MapPath(path);

                if (System.IO.File.Exists(physicalPath))
                {
                    const string contentType = "image/png";
                    return File(System.IO.File.OpenRead(physicalPath), contentType);
                }
            }

            throw new HttpException(403, "Forbidden");
        }

        public virtual bool AuthorizeImage(string path)
        {
            return CanAccess(path) && IsValidFile(Path.GetExtension(path));
        }
    }
}
