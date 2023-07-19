using JobExchange.Models;

namespace JobExchange.Data
{
    public interface IJobExchangeRepository
    {
        void AddEntity(object model);
        //IEnumerable<Employer> GetAllOrders();
        IEnumerable<Employer> GetEmployer();
        Employer GetEmployerById(int id);
        bool SaveAll();
        void UpdateEmployer(Employer employer);
        string GetCurrentUserId();
        void SaveChanges();
        IEnumerable<TypeJob> GetTypeJobs();
        TypeJob GetTypeJobById(int id);
        Task<bool> HasEmployer(string userId);
        IEnumerable<JobInfo> GetAllJobs();
        void DeleteJobInfo(int jobId);
        Task<Employer> GetEmployerByUserId(string userId);
        JobInfo GetJobById(int id);
        void UpdateJobInfo(JobInfo jobInfo);
        Task<IEnumerable<JobInfo>> GetJobInfosByUserId(string userId);

    }
}