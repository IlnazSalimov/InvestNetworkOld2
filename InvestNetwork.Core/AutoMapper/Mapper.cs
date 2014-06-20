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
        /// <summary>
        /// Предоставляет доступ к хранилищу данных о статусах проекта.</summary>
        private static IProjectStatusRepository _projectStatusRepository = DependencyResolver.Current.GetService<IProjectStatusRepository>();

        /// <summary>
        /// Предоставляет доступ к хранилищу данных о списке сфер деятельности.</summary>
        private static IScopeRepository _scopeRepository = DependencyResolver.Current.GetService<IScopeRepository>();

        /// <summary>
        /// Инициализирует новый экземпляр CommonMapper.</summary>
        static CommonMapper()
        {
            Mapper.CreateMap<Project, ProjectDTO>().
                ForMember(dto => dto.LocationCityID, mpe => mpe.MapFrom(p => p.LocationCityID.HasValue ? p.LocationCityID.Value : 0)).
                ForMember(dto => dto.Scope, mpe => mpe.MapFrom(p => _scopeRepository.GetById(p.ScopeID).Title)).
                ForMember(dto => dto.Status, mpe => mpe.MapFrom(p => _projectStatusRepository.GetByCode((int)p.Status).Status)).
                ForMember(dto => dto.NecessaryFunding, mpe => mpe.MapFrom(p => p.NecessaryFunding.HasValue ? p.NecessaryFunding.Value : 0)).
                ForMember(dto => dto.FundingDuration, mpe => mpe.MapFrom(p => p.FundingDuration.HasValue ? p.FundingDuration.Value : 0));
        }

        /// <summary>
        /// Выполняет отображение исходного объекта в новый объект назначения.</summary>
        /// <param name="source">Объект-источник данных для отображения</param>
        /// <param name="sourceType">Тип объекта-источника(использование)</param>
        /// <param name="destinationType">Тип объекта-назначения(создание)</param>
        /// <returns>Отображенный объект назначения</returns>
        public object Map(object source, Type sourceType, Type destinationType)
        {
            return Mapper.Map(source, sourceType, destinationType);
        }
    }
}