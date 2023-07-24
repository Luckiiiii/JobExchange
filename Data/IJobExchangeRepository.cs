using JobExchange.Models;
using System.Threading.Tasks;

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
        Task<bool> HasJobInfoByEmployer(int employerId);
        Task<Employer> HasEmployerByUser(string userId);
        Task DeleteJobInfoByEmployerId(int employerId);
        Task<int> GetEmployerByUser(String userId);
        void DeleteEmployer(int employerId);

        Task<List<JobInfo>> SearchJobInfo(double minSalary, double maxSalary, string address, string position, DateTime postTime, int typeJobId, string typeJobName);
        IEnumerable<JobInfo> SearchJobInfoByTypeJobName(string typeJobName);
        IEnumerable<JobInfo> GetJobInfosWithPagination(int pageNumber, int pageSize);
        Task<int> GetTotalJobInfo();

        IEnumerable<TypeJob> GetMostSearchedTypeJobs(int count);
    }
}