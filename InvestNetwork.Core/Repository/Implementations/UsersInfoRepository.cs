using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using InvestNetwork.Core;
using System.Linq.Expressions;

namespace InvestNetwork.Core
{
    public class UsersInfoRepository : IUsersInfoRepository
    {
        private IRepository<UsersInfo> usersInfoRepository;
        private IRepository<Payment> paymentRepository;
        private IRepository<Project> projectRepository;
        private IRepository<PaymentStatus> paymentStatusRepository;
        private IRepository<ProjectStatus> projectStatusRepository;

        public UsersInfoRepository(IRepository<UsersInfo> usersInfoRepository, IRepository<Payment> paymentRepository,
                                   IRepository<Project> projectRepository, IRepository<PaymentStatus> paymentStatusRepository,
                                   IRepository<ProjectStatus> projectStatusRepository)
        {
            this.usersInfoRepository = usersInfoRepository;
            this.paymentRepository = paymentRepository;
            this.projectRepository = projectRepository;
            this.paymentStatusRepository = paymentStatusRepository;
            this.projectStatusRepository = projectStatusRepository;
        }

        public UsersInfo GetById(int id)
        {
            if (id == 0)
                return null;
            return usersInfoRepository.GetById(id);
        }

        public List<PartycipationUsersInfo> GetPartycipation(int id)
        {
            List<Payment> payments = paymentRepository.GetAll().Where(e => e.UserID == id).ToList();

            List<PartycipationUsersInfo> participations = new List<PartycipationUsersInfo>();

            foreach (Payment payment in payments)
            {
                PartycipationUsersInfo partycipation = new PartycipationUsersInfo();

                partycipation.PaymentId = payment.PaymentID;
                partycipation.PaymentDate = payment.PaymentDate;
                partycipation.Sum = payment.Sum;
                partycipation.PaymentStatus = paymentStatusRepository.GetById(partycipation.PaymentId).Status;
                partycipation.ProjectId = payment.ProjectID;

                Project project = projectRepository.GetById(partycipation.ProjectId);

                partycipation.ProjectName = project.Name;
                partycipation.ProjectStatus = projectStatusRepository.GetById(project.ProjectStatusID).Status;

                participations.Add(partycipation);
            }

            return participations;
        }

        public UsersInfo GetByUserId(int id)
        {
            try
            {
                if (id == 0)
                    return null;
                return usersInfoRepository.GetAll().Where(u => u.UserID == id).Single();
            }
            catch
            {
                return null;
            }
        }

        public void Insert(UsersInfo model)
        {
            if (model == null)
                throw new ArgumentNullException("usersInfo");
            usersInfoRepository.Insert(model);
        }

        public void Update(UsersInfo model)
        {
            if (model == null)
                throw new ArgumentNullException("usersInfo");
            usersInfoRepository.Update(model);

        }

        public void Delete(UsersInfo model)
        {
            if (model == null)
                throw new ArgumentNullException("usersInfo");
            usersInfoRepository.Delete(model);
        }

        //public bool ValidateUser(string email, string password)
        //{
        //    return usersInfoRepository.GetAll().Any(user => string.Equals(user.Email, email) && string.Equals(user.Password, password));
        //}

        public void SaveChanges()
        {
            usersInfoRepository.SaveChanges();
        }
    }
}

