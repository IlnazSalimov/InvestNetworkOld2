using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using InvestNetwork.Core;
using Ninject;
using System.Web.Mvc;

namespace InvestNetwork.Core
{
    public class CommonMapper : IMapper
    {
        private static IProjectStatusRepository _projectStatusRepository = DependencyResolver.Current.GetService<IProjectStatusRepository>();
        private static IScopeRepository _scopeRepository = DependencyResolver.Current.GetService<IScopeRepository>();

        static CommonMapper()
        {
            Mapper.CreateMap<Project, ProjectDTO>().
                ForMember(dto => dto.LocationCityID, mpe => mpe.MapFrom(p => p.LocationCityID.HasValue ? p.LocationCityID.Value : 0)).
                ForMember(dto => dto.Scope, mpe => mpe.MapFrom(p => _scopeRepository.GetById(p.ScopeID).Title)).
                ForMember(dto => dto.Status, mpe => mpe.MapFrom(p => _projectStatusRepository.GetByCode((int)p.Status).Status)).
                ForMember(dto => dto.NecessaryFunding, mpe => mpe.MapFrom(p => p.NecessaryFunding.HasValue ? p.NecessaryFunding.Value : 0)).
                ForMember(dto => dto.FundingDuration, mpe => mpe.MapFrom(p => p.FundingDuration.HasValue ? p.FundingDuration.Value : 0));
        }

        public object Map(object source, Type sourceType, Type destinationType)
        {
            return Mapper.Map(source, sourceType, destinationType);
        }
    }
}