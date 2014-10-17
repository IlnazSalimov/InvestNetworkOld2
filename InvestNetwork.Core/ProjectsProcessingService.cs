using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Web.Mvc;

namespace InvestNetwork.Core
{
    /// <summary>
    /// Служба, которая с некоторым интервалом проверяет все проекты на предмет завершенности срока финансирования.
    /// Если срок финансирования завершился, статус проекта изменяется на "Неактивный"
    /// </summary>
    public class ProjectsProcessingService
    {
        private static System.Timers.Timer aTimer;
        private static Thread thread;

        /// <summary>
        /// Инициализирует службу и создает для него парралельный поток выполнения
        /// </summary>
        public static void Start()
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(StartProcessing);
                thread.Start();
            }
        }

        /// <summary>
        /// Запускает службу
        /// </summary>
        private static void StartProcessing()
        {
            aTimer = new System.Timers.Timer(2000);

            // Hook up the Elapsed event for the timer.
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Enabled = true;

            // If the timer is declared in a long-running method, use
            // KeepAlive to prevent garbage collection from occurring
            // before the method ends.
            // GC.KeepAlive(aTimer);
        }

        /// <summary>
        /// Метод вызывается когда срабатывает событие Elapsed. Выполняется проверка проектов.
        /// </summary>
        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            IProjectRepository projectRepository = DependencyResolver.Current.GetService<IProjectRepository>();
            IProjectStatusRepository projectStatusRepository = DependencyResolver.Current.GetService<IProjectStatusRepository>();
            List<Project> expiredProjects;
            try
            {
                expiredProjects = (
                    from p in projectRepository.GetAll()
                    join s in projectStatusRepository.GetAll()
                    on p.ProjectStatusID equals s.ProjectStatusID
                    where p.EndDate < DateTime.Now
                    where s.StatusCode == (int)ProjectStatusEnum.Active ||
                    s.StatusCode == (int)ProjectStatusEnum.OnReview
                    select p
                ).ToList();
            }
            catch
            {
                expiredProjects = new List<Project>();
            }

            foreach (Project p in expiredProjects)
            {
                p.Status = ProjectStatusEnum.Inactive;
                projectRepository.SaveChanges();
            }
        }
    }
}
