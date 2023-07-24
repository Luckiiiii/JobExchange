using JobExchange.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JobExchange.Data
{
    public class JobExchangeRepository : IJobExchangeRepository
    {
        private readonly JobExchangeContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JobExchangeRepository(JobExchangeContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public void AddEntity(object model)
        {
            _context.Add(model);
        }
        /*public IEnumerable<Employer> GetAllOrders()
        {
            return _context.Employers
                .Include(o => o.Jobs)
                .ToList();
        }*/
        
        public Employer GetEmployerById(int id)
        {
            return _context.Employers
                .Include(o => o.Jobs)
                .Where(p => p.Id == id)
                .FirstOrDefault();
        }
        public void UpdateEmployer(Employer employer)
        {
            _context.Update(employer);
            _context.SaveChanges();
        }
        public IEnumerable<Employer> GetEmployer()
        {
            return _context.Employers.ToList();
        }
        public async Task<bool> HasEmployer(string userId)
        {
            return await _context.Employers.AnyAsync(e => e.User.Id == userId);
        }
        public async Task<Employer> HasEmployerByUser(string userId)
        {
            return await _context.Employers.FirstOrDefaultAsync(e => e.User.Id == userId);
        }
        public async Task<bool> HasJobInfoByEmployer(int employerId)
        {
            return await _context.JobInfos.AnyAsync(e=>e.Employer.Id == employerId);
        }
        public IEnumerable<JobInfo> GetAllJobs()
        {
            return _context.JobInfos
                .Include(o => o.TypeJob)
                .Include(o => o.Employer)
                .ToList();
        }
        public JobInfo GetJobById(int id)
        {
            return _context.JobInfos
                .Include(o => o.TypeJob)
                .Include(o => o.Employer)
                .Where(p => p.Id == id)
                .FirstOrDefault();
                
        }
        public void UpdateJobInfo(JobInfo jobInfo)
        {
            _context.Update(jobInfo);
            _context.SaveChanges();
        }
        public void DeleteJobInfo(int jobId)
        {
            var job = _context.JobInfos.Find(jobId);
            if (job != null)
            {
                _context.JobInfos.Remove(job);
                _context.SaveChanges();
            }
        }
        public void DeleteEmployer(int employerId)
        {
            var employer = _context.Employers.Find(employerId);
            if (employer != null)
            {
                _context.Employers.Remove(employer);
                _context.SaveChanges();
            }
        }
        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
        
        public string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
        public IEnumerable<TypeJob> GetTypeJobs()
        {
            return _context.TypeJobs.ToList();
        }
        
        public TypeJob GetTypeJobById(int id)
        {
            return _context.TypeJobs
                .Include(o=>o.JobInfos)
                .Where (p => p.Id == id)
                .FirstOrDefault();
        }

        public async Task DeleteJobInfoByEmployerId(int employerId)
        {
            // Truy vấn tập hợp các đối tượng JobInfo dựa trên employerId
            var jobInfosToDelete = await _context.JobInfos
                .Where(job => job.Employer.Id == employerId)
                .ToListAsync();

            // Xóa tập hợp các đối tượng JobInfo
            _context.JobInfos.RemoveRange(jobInfosToDelete);

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
        }

        public async Task<Employer> GetEmployerByUserId(String userId)
        {
            return await _context.Employers.FirstOrDefaultAsync(e => e.User.Id == userId);
        }

        public async Task <int> GetEmployerByUser(String userId)
        {
            var employer = await _context.Employers.FirstOrDefaultAsync(e => e.User.Id == userId);

            if (employer != null)
            {
                return employer.Id;
            }

            // Trả về một giá trị không hợp lệ (ví dụ: -1) nếu không tìm thấy Employer.
            return -1;
        }
        public async Task<IEnumerable<JobInfo>> GetJobInfosByUserId(string userId)
        {
            return _context.JobInfos
                .Where(job => job.Employer.User.Id == userId)
                .ToList();
        }

    }
}
