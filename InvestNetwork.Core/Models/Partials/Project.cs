using InvestNetwork.Core;
using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace InvestNetwork.Core
{
    public enum ProjectStatusEnum
    {
        OnReview = 1,
        Active = 5,
        Inactive = -99,
        Blocked = -1,
        Uncreated = -5
    }

    public enum FundingPeriod
    {
        _30Days = 30
    }

    [MetadataType(typeof(ProjectMetaData))]
    public partial class Project : IEntity
    {
        private IUserRepository _userRepository = DependencyResolver.Current.GetService<IUserRepository>();
        private IUsersInfoRepository _userInfoRepository = DependencyResolver.Current.GetService<IUsersInfoRepository>();
        private IProjectStatusRepository _projectStatusRepository = DependencyResolver.Current.GetService<IProjectStatusRepository>();

        public string AuthorFullName
        {
            get
            {
                UsersInfo authorInfo = _userInfoRepository.GetByUserId(this.AuthorID);
                return String.Format("{0} {1}", authorInfo.Name, authorInfo.Family);
            }
        }

        public ProjectStatusEnum Status
        {
            get
            {
                if (this.ProjectStatusID > 0)
                    return (ProjectStatusEnum)_projectStatusRepository.GetById(this.ProjectStatusID).StatusCode;
                else return 0;
            }
            set
            {
                this.ProjectStatusID = _projectStatusRepository.GetByCode((int)value).ProjectStatusID;
            }
        }

        public int ID
        {
            get { return this.ProjectID; }
        }
    }
}