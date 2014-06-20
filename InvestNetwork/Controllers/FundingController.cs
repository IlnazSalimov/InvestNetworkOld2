using InvestNetwork.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace InvestNetwork.Controllers
{
    /// <summary>
    /// Предоставляет методы, которые отвечают за бизнес логику инвестирования</summary>
    [Authorize]
    public class FundingController : Controller
    {
        /// <summary>
        /// Предоставляет доступ к хранилищу данных о проектах.</summary>
        private readonly IProjectRepository _projectRepository;

        /// <summary>  
        /// Инициализирует новый экземпляр FundingController с внедрением зависемостей к хранилищу проектов.</summary>  
        /// <param name="projectRepository">Экземпляр класса ProjectRepository, предоставляющий доступ к хранилищу данных о проектах.</param>
        /// <param name="projectStatusRepository">Экземпляр класса ProjectRepository, предоставляющий доступ к хранилищу данных о статусах проектах</param>
        /// <returns>Новый экземпляр AdminController.</returns>
        public FundingController(IProjectRepository projectRepository)
        {
            this._projectRepository = projectRepository;
        }


        /// <summary>  
        /// Метод отвечающий за бизнес логику на странице ввода количества инвестиций в проект, с заданным идентификатором.</summary>
        /// <param name="Id">Идентификатор проекта</param>
        /// <returns>Экземпляр ViewResult с моделью проекта, который выполняет визуализацию представления.</returns>
        public async Task<ActionResult> DetermineAmount(int Id)
        {
            return View(await _projectRepository.GetByIdAsync(Id));
        }

        /// <summary>  
        /// Метод отвечающий за бизнес логику на странице ввода платежной информации.</summary>
        /// <returns>Экземпляр ViewResult с моделью проекта, который выполняет визуализацию представления.</returns>
        public ActionResult Invest()
        {
            return View();
        }
    }
}
