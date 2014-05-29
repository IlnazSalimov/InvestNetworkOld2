using InvestNetwork.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace InvestNetwork.Application.Core
{
    /// <summary>
    /// Представляет поддержку для визуализации элементов расширенный список управления HTML в представлении.</summary>
    public static class HtmlHelperExtension
    {
        /// <summary>
        /// Предоставляет доступ к хранилищу данных о пользователях.</summary>
        public static IUserRepository _userRepository 
        { 
            get
            {
                return DependencyResolver.Current.GetService<IUserRepository>();
            }
        }

        /// <summary>
        /// Предоставляет доступ к хранилищу данных о проектах.</summary>
        public static IProjectRepository _projectRepository
        {
            get
            {
                return DependencyResolver.Current.GetService<IProjectRepository>();
            }
        }

        private static TagBuilder GetUserLinkBuilder(int userId, object optionalHtmlAttributes = null)
        {
            User user = _userRepository.GetById(userId);
            if (user == null)
                return new TagBuilder(String.Empty);
            var builder = new TagBuilder("a");

            builder.SetInnerText(user.FullName);
            UrlHelper urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            string linkToUser = urlHelper.Action("GetProfile", "Profile", new { Id = user.Id });
            builder.MergeAttribute("href", linkToUser);
            if (optionalHtmlAttributes != null)
                builder.MergeAttributes(new RouteValueDictionary(optionalHtmlAttributes));

            return builder;
        }

        private static TagBuilder GetProjectImgBuilder(int projectId, bool wrapToLink = false, bool isLinkToAdminView = false, int? optionalHeight = null, int? optionalWidth = null, object optionalHtmlAttributes = null)
        {
            Project project = _projectRepository.GetById(projectId);
            if (project == null)
                return new TagBuilder(String.Empty);
            TagBuilder builder = new TagBuilder("img");

            builder.MergeAttribute("src", project.LinkToImg + ((optionalHeight != null && optionalWidth != null) ?
                String.Format("@{0}x{1}sc", optionalHeight, optionalWidth) : ""));

            if (optionalHtmlAttributes != null)
                builder.MergeAttributes(new RouteValueDictionary(optionalHtmlAttributes));
            
            if (wrapToLink){
                TagBuilder wrapper = GetProjectLinkBuilder(projectId, null, isLinkToAdminView);
                wrapper.InnerHtml = builder.ToString(TagRenderMode.SelfClosing);
                return wrapper;
            }

            return builder;
        }

        private static TagBuilder GetUserImgBuilder(int userId, bool wrapToLink = false, int? optionalHeight = null, int? optionalWidth = null, object optionalHtmlAttributes = null)
        {
            User user = _userRepository.GetById(userId);
            if (user == null)
                return new TagBuilder(String.Empty);
            TagBuilder builder = new TagBuilder("img");

            builder.MergeAttribute("src", (user.Avatar ?? "/UPLOAD/Custom/210x230.jpeg") + ((optionalHeight != null && optionalWidth != null) ?
                String.Format("@{0}x{1}sc", optionalHeight, optionalWidth) : ""));

            if (optionalHtmlAttributes != null)
                builder.MergeAttributes(new RouteValueDictionary(optionalHtmlAttributes));

            if (wrapToLink)
            {
                TagBuilder wrapper = GetUserLinkBuilder(userId, null);
                wrapper.InnerHtml = builder.ToString(TagRenderMode.SelfClosing);
                return wrapper;
            }

            return builder;
        }

        private static TagBuilder GetProjectLinkBuilder(int projectId, object optionalHtmlAttributes = null, bool isLinkToAdminView = false)
        {
            Project project = _projectRepository.GetById(projectId);
            if (project == null)
                return new TagBuilder(String.Empty);
            TagBuilder builder = new TagBuilder("a");

            UrlHelper urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            string linkToProject = urlHelper.Action(isLinkToAdminView ? "ReviewProject" : "View", isLinkToAdminView ? "Admin" : "Project", new { Id = project.ID });
            builder.MergeAttribute("href", linkToProject);
            builder.SetInnerText(project.Name);

            if (optionalHtmlAttributes != null)
                builder.MergeAttributes(new RouteValueDictionary(optionalHtmlAttributes));
            return builder;
        }

        /// <summary>  
        /// Создает HTML элемент, который является ссылкой на домашнюю страницу пользователя с заданным идентификатором.</summary>  
        /// <param name="helper">Объект к которому будет применен данный метод расширения</param>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>HTML элемент, который ссылается на пользователя.</returns>
        public static MvcHtmlString UserLink(this HtmlHelper helper, int userId)
        {
            return new MvcHtmlString(GetUserLinkBuilder(userId).ToString(TagRenderMode.Normal));
        }

        /// <summary>  
        /// Создает HTML элемент с заданными атрибутами, который является ссылкой на домашнюю страницу пользователя с заданным идентификатором.</summary>  
        /// <param name="helper">Объект к которому будет применен данный метод расширения</param>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="htmlAttributes">Объект, который содержит HTML атрибуты для элемента.</param>
        /// <returns>HTML элемент, который ссылается на пользователя.</returns>
        public static MvcHtmlString UserLink(this HtmlHelper helper, int userId, object htmlAttributes)
        {
            TagBuilder builder = GetUserLinkBuilder(userId, htmlAttributes);

            return new MvcHtmlString(builder.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>  
        /// Создает HTML элемент, который является изображением проекта с заданным идентификатором.</summary>  
        /// <param name="helper">Объект к которому будет применен данный метод расширения</param>
        /// <param name="projectId">Идентификатор проекта</param>
        /// <returns>HTML элемент, который визуализирует изображение проекта.</returns>
        public static MvcHtmlString ProjectImage(this HtmlHelper helper, int projectId)
        {
            TagBuilder builder = GetProjectImgBuilder(projectId);

            return new MvcHtmlString(builder.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>  
        /// Создает HTML элемент со ссылкой на проект или без, который является изображением проекта с заданным идентификатором.</summary>
        /// <param name="helper">Объект к которому будет применен данный метод расширения</param>
        /// <param name="projectId">Идентификатор проекта</param>
        /// <param name="wrapToLink">true для установки ссылки, false в противном случае</param>
        /// <param name="isLinkToAdminView">true для установки ссылки на администраторское представление, false в противном случае</param>
        /// <returns>HTML элемент, который визуализирует изображение проекта.</returns>
        public static MvcHtmlString ProjectImage(this HtmlHelper helper, int projectId, bool wrapToLink, bool isLinkToAdminView)
        {
            TagBuilder builder = GetProjectImgBuilder(projectId, wrapToLink, isLinkToAdminView);

            return new MvcHtmlString(builder.ToString(wrapToLink ? TagRenderMode.Normal : TagRenderMode.SelfClosing));
        }

        /// <summary>  
        /// Создает HTML элемент со ссылкой на проект или без, с устанокой заданных атрибутов, который является изображением проекта с заданными идентификатором.</summary>  
        /// <remarks>  
        /// Размеры и атрибуты элемента являются опциональными. Если размеры не указаны, размер изображения не 
        /// изменяется. Высота и ширина изображения должны передаваться или не передаваться одновременно.</remarks>
        /// <param name="helper">Объект к которому будет применен данный метод расширения</param>
        /// <param name="projectId">Идентификатор проекта</param>
        /// <param name="wrapToLink">true для установки ссылки, false в противном случае</param>
        /// <param name="isLinkToAdminView">true для установки ссылки на администраторское представление, false в противном случае</param>
        /// <param name="htmlAttributes">Объект, который содержит HTML атрибуты для элемента.</param>
        /// <returns>HTML элемент, который визуализирует изображение проекта.</returns>
        public static MvcHtmlString ProjectImage(this HtmlHelper helper, int projectId, bool wrapToLink, bool isLinkToAdminView, object htmlAttributes)
        {
            TagBuilder builder = GetProjectImgBuilder(projectId, wrapToLink, isLinkToAdminView, null, null, htmlAttributes);

            return new MvcHtmlString(builder.ToString(wrapToLink ? TagRenderMode.Normal : TagRenderMode.SelfClosing));
        }


        /// <summary>  
        /// Создает HTML элемент c заданными размерами со ссылкой на проект или без, который является изображением проекта с заданными идентификатором.</summary>  
        /// <param name="helper">Объект к которому будет применен данный метод расширения</param>
        /// <param name="projectId">Идентификатор проекта</param>
        /// <param name="wrapToLink">true для установки ссылки, false в противном случае</param>
        /// <param name="isLinkToAdminView">true для установки ссылки на администраторское представление, false в противном случае</param>
        /// <param name="height">Высота изображения</param>
        /// <param name="width">Ширина изображения</param>
        /// <returns>HTML элемент, который визуализирует изображение проекта.</returns>
        public static MvcHtmlString ProjectImage(this HtmlHelper helper, int projectId, bool wrapToLink, bool isLinkToAdminView, int height, int width)
        {
            TagBuilder builder = GetProjectImgBuilder(projectId, wrapToLink, isLinkToAdminView, height, width);

            return new MvcHtmlString(builder.ToString(wrapToLink ? TagRenderMode.Normal : TagRenderMode.SelfClosing));
        }

        /// <summary>  
        /// Создает HTML элемент c заданными атрибутами и размером, со ссылкой на проект или без, который является изображением проекта с заданными идентификатором.</summary>  
        /// <param name="helper">Объект к которому будет применен данный метод расширения</param>
        /// <param name="projectId">Идентификатор проекта</param>
        /// <param name="wrapToLink">true для установки ссылки, false в противном случае</param>
        /// <param name="isLinkToAdminView">true для установки ссылки на администраторское представление, false в противном случае</param>
        /// <param name="height">Высота изображения</param>
        /// <param name="width">Ширина изображения</param>
        /// <param name="htmlAttributes">Объект, который содержит HTML атрибуты для элемента.</param>
        /// <returns>HTML элемент, который визуализирует изображение проекта.</returns>
        public static MvcHtmlString ProjectImage(this HtmlHelper helper, int projectId, bool wrapToLink, bool isLinkToAdminView, int height, int width, object htmlAttributes)
        {
            TagBuilder builder = GetProjectImgBuilder(projectId, wrapToLink, isLinkToAdminView, height, width, htmlAttributes);

            return new MvcHtmlString(builder.ToString(wrapToLink ? TagRenderMode.Normal : TagRenderMode.SelfClosing));
        }

        /// <summary>  
        /// Создает HTML элемент c заданными атрибутами и размером, со ссылкой на пользователя или без, который является изображением пользователя с заданными идентификатором.</summary>  
        /// <param name="helper">Объект к которому будет применен данный метод расширения</param>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="wrapToLink">true для установки ссылки, false в противном случае</param>
        /// <param name="isLinkToAdminView">true для установки ссылки на администраторское представление, false в противном случае</param>
        /// <param name="height">Высота изображения</param>
        /// <param name="width">Ширина изображения</param>
        /// <param name="htmlAttributes">Объект, который содержит HTML атрибуты для элемента.</param>
        /// <returns>HTML элемент, который визуализирует изображение пользователя.</returns>
        public static MvcHtmlString UserImage(this HtmlHelper helper, int userId, bool wrapToLink, int height, int width, object htmlAttributes)
        {
            TagBuilder builder = GetUserImgBuilder(userId, wrapToLink, height, width, htmlAttributes);

            return new MvcHtmlString(builder.ToString(wrapToLink ? TagRenderMode.Normal : TagRenderMode.SelfClosing));
        }

        /// <summary>  
        /// Создает HTML элемент, который является ссылкой на проект с заданными идентификатором.</summary>  
        /// <param name="helper">Объект к которому будет применен данный метод расширения.</param>
        /// <param name="projectId">Идентификатор проекта.</param>
        
        /// <returns>HTML элемент, который ссылается на страницу проекта.</returns>
        public static MvcHtmlString ProjectLink(this HtmlHelper helper, int projectId, bool isLinkToAdminView)
        {
            TagBuilder builder = GetProjectLinkBuilder(projectId, null, isLinkToAdminView);

            return new MvcHtmlString(builder.ToString(TagRenderMode.Normal));
        }

        /// <summary>  
        /// Создает HTML элемент с заданными атрибутами, который является ссылкой на проект с заданными идентификатором.</summary>  
        /// <param name="helper">Объект к которому будет применен данный метод расширения.</param>
        /// <param name="htmlAttributes">Объект, который содержит HTML атрибуты для элемента.</param>
        /// <param name="projectId">Идентификатор проекта.</param>
        /// <returns>HTML элемент, который ссылается на страницу проекта.</returns>
        public static MvcHtmlString ProjectLink(this HtmlHelper helper, int projectId, bool isLinkToAdminView, object htmlAttributes)
        {
            TagBuilder builder = GetProjectLinkBuilder(projectId, htmlAttributes, isLinkToAdminView);

            return new MvcHtmlString(builder.ToString(TagRenderMode.Normal));
        }
    }
}