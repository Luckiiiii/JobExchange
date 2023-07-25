using JobExchange.Data;
using JobExchange.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JobExchange.Controllers
{
    public class AppController : Controller
    {
        private readonly IJobExchangeRepository _repository;
        private readonly UserManager<StoreUser> _userManager;
        private readonly ILogger<AppController> _logger;

        public AppController(IJobExchangeRepository repository, UserManager<StoreUser> userManager, ILogger<AppController> logger)
        {
            _repository = repository;
            _userManager = userManager;
            _logger = logger;
        }
        //them truoc controller can phai co quyen
        //[Authorize(Roles ="admin")] quyen admin
        //[Authorize] quyen nguoi dang nhap
        public IActionResult Index()
        {

            return View();
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UserInfo()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        [Authorize]
        public async Task<IActionResult> EmployerView(Employer model)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var infoEmployer = await _repository.GetEmployerByUserId(user.Id);

            return View(infoEmployer);
        }

        public IActionResult SearchTypeJobInfo(string typeJobName)
        {
            if (typeJobName != null)
            {
                var searchTypeJob = _repository.SearchJobInfoByTypeJobName(typeJobName);
                return View("ShowJobInfo", searchTypeJob);
            }
            return View("ShowJobInfo");
        }
        /*public IActionResult SearchTypeJobInfo(string typeJobName)
        {
            if (typeJobName != null)
            {
                var searchTypeJob = _repository.SearchJobInfoByTypeJobName(typeJobName);
                var mostSearchedTypeJobs = _repository.GetMostSearchedTypeJobNames(); // Lấy 5 TypeJob có lượt tìm kiếm nhiều nhất
                ViewBag.MostSearchedTypeJobs = mostSearchedTypeJobs;
                return View("ShowJobInfo", searchTypeJob);
            }
            return View("ShowJobInfo");
        }*/

        /*        public async Task<IActionResult> ShowJobInfo(int pageNumber = 1, int pageSize = 2)
                {
                    var totalJobInfo = await _repository.GetTotalJobInfo();
                    if (totalJobInfo != null && totalJobInfo is int)
                    {
                        var totalPages = (int)Math.Ceiling(totalJobInfo / (double)pageSize); // Số lượng trang
                        var jobInfos = _repository.GetJobInfosWithPagination(pageNumber, pageSize);
                        ViewBag.CurrentPage = pageNumber;
                        ViewBag.TotalPages = totalPages;
                        return View(jobInfos);
                    }
                    return View();
                }*/

        public IActionResult ShowMostSearchedTypeJobs(int count)
        {
            var mostSearchedTypeJobs = _repository.GetMostSearchedTypeJobs(count);
            return View(mostSearchedTypeJobs);
        }
        //Hien tat ca thong tin tuyen dung
        public IActionResult ShowJobInfo()
        {
            var allJobInfo = _repository.GetAllJobs(); // Lấy thông tin của tất cả các JobInfo từ repository
            return View(allJobInfo);
            //return Json(allJobInfo);
        }

        [Authorize]
        public async Task<IActionResult> ShowJobInfoUser()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                var userId = user.Id;
                var existingJobInfo = await _repository.GetJobInfosByUserId(userId);
                return PartialView(existingJobInfo);
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> ShowJobInfoByUser()
        {
            // Lấy danh sách công việc từ repository
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                var userId = user.Id;
                var jobInfos = _repository.GetJobInfosByUserId(userId);
                return Json(jobInfos);
            }
            return RedirectToAction("Index");
        }

        //cap nhat thong tin tuyen dung
        [Authorize]
        public IActionResult UpdateJobInfo(int id)
        {
            var job = _repository.GetJobById(id);
            var typeJobs = _repository.GetTypeJobs();

            // Gán danh sách TypeJob vào ViewBag
            ViewBag.ListTypeJobs = new SelectList(typeJobs, "Id", "NameJob", job.TypeJob.Id);

            return View(job);
        }

        [Authorize]
        public IActionResult UpdateJob(JobInfo model)
        {
            var existingJob = _repository.GetJobById(model.Id);

            // Gán thông tin của model vào JobInfo hiện tại
            existingJob.Position = model.Position;
            existingJob.Salary = model.Salary;
            existingJob.Address = model.Address;
            existingJob.Describe = model.Describe;
            existingJob.PostTime = model.PostTime;
            var existingTypeJob = _repository.GetTypeJobById(model.TypeJob.Id);

            // Gán thông tin của TypeJob vào JobInfo hiện tại
            existingJob.TypeJob = existingTypeJob;
            _repository.UpdateJobInfo(existingJob);
            return RedirectToAction("ShowJobInfoUser");
        }

        //Xoa thong tin tuyen dung cu the
        [Authorize]
        public IActionResult DeleteJobInfo(int id)
        {
            _repository.DeleteJobInfo(id);
            return RedirectToAction("ShowJobInfoUser");
        }
        public IActionResult DeleteJobInfoByAdmin(int id)
        {
            _repository.DeleteJobInfo(id);
            return RedirectToAction("ShowJobInfo");
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            _repository.DeleteJobInfo(id);
            return Ok();
        }

        //Hien thi TypeJob trong selected
        public IActionResult JobView()
        {
            var listJobs = _repository.GetTypeJobs();
            SelectList results = new SelectList(listJobs, "Id", "NameJob");
            ViewBag.listJobs = results;
            /*var listEmployer = _repository.GetEmployer();
            SelectList resultEmployer = new SelectList(listEmployer, "Id", "CompanyName");
            ViewBag.listEmployer = resultEmployer;*/
            return View();
        }

        // POST: /App/AddJobs
        [HttpPost]
        [Authorize]
        //Them thong tin tuyen dung
        public async Task<IActionResult> AddJobs(JobInfo model)
        {
            var existingTypeJob = _repository.GetTypeJobById(model.TypeJob.Id);
            if (existingTypeJob != null)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var existingEmployer = await _repository.GetEmployerByUserId(user.Id);

                if (existingEmployer != null)
                {
                    // Liên kết bảng TypeJob và Employer với JobInfo bằng cách sử dụng Id
                    model.TypeJob = existingTypeJob;
                    model.Employer = existingEmployer;
                    model.PostTime = DateTime.Now;

                    _repository.AddEntity(model);
                    _repository.SaveChanges();
                    // Trả về kết quả thông báo thành công dưới dạng JSON
                    return Ok("Form submitted successfully!");
                }
            }
            // Trả về kết quả thông báo lỗi dưới dạng JSON
            return BadRequest("Form data is not valid.");
        }
        /*public IActionResult AddJobs(JobInfo model)
        {
            *//*var typeJob = _repository.GetTypeJobById(model.TypeJob.Id);
            var employer = _repository.GetEmployerById(model.Employer.Id);
            if (typeJob != null)
            {
                var job = typeJob.JobInfos.Where(i => i.Id == typeJob.Id).FirstOrDefault();
                var job2 = employer.Jobs.Where(i=>i.Id == employer.Id).FirstOrDefault();
                if (job == null&&job2 ==null)
                {
                    typeJob.JobInfos.Add(model);
                    _repository.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View();*//*
            var userId = _userManager.GetUserId(HttpContext.User);
            var existingTypeJob = _repository.GetTypeJobById(model.TypeJob.Id);
            var existingEmployer = _repository.GetEmployerByUserId(userId);


            if (existingTypeJob != null && existingEmployer != null)
            {
                // Liên kết bảng TypeJob và Employer với JobInfo bằng cách sử dụng Id
                model.TypeJob = existingTypeJob;
                model.Employer = existingEmployer;
                model.PostTime = DateTime.Now;
                
                _repository.AddEntity(model);
                _repository.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }*/

        [HttpPost("EmployerView")]
        [Authorize]
        //Them nha tuyen dung
        public async Task<IActionResult> AddEmployer(Employer model)
        {
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var hasEmployer = await _repository.HasEmployer(user.Id);
                if (user != null && hasEmployer == null)
                {
                    var newEmployer = new Employer()
                    {
                        CompanyName = model.CompanyName,
                        AddressOfCompany = model.AddressOfCompany,
                        Email = model.Email,
                        Phone = model.Phone,
                        User = user
                    };
                    _repository.AddEntity(newEmployer);
                    _repository.SaveChanges();
                    return View("index");
                }
                else
                {
                    //thong bao loi!!!
                    return View("index");
                }
                /*if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(HttpContext.User);
                    if (user != null)
                    {
                        var newEmployer = new Employer()
                        {
                            CompanyName = model.CompanyName,
                            AddressOfCompany = model.AddressOfCompany,
                            Email = model.Email,
                            Phone = model.Phone,
                            User = user
                        };
                        _repository.AddEntity(newEmployer);
                        _repository.SaveChanges();
                        return View("index");
                    }

                }*/
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save a new order: {ex}");
            }
            return View("index");
        }

        [Authorize]
        public async Task<IActionResult> UpdateEmployer(Employer model)
        {
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var hasEmployer = await _repository.HasEmployer(user.Id);
                var existingEmployer = await _repository.GetEmployerByUserId(user.Id);

                if (user != null && existingEmployer != null)
                {
                    // Gán thông tin của model vào JobInfo hiện tại
                    existingEmployer.CompanyName = model.CompanyName;
                    existingEmployer.Phone = model.Phone;
                    existingEmployer.Email = model.Email;
                    existingEmployer.AddressOfCompany = model.AddressOfCompany;

                    _repository.UpdateEmployer(existingEmployer);
                    return View("index");
                }

                else if (user != null && !hasEmployer)
                {
                    var newEmployer = new Employer()
                    {
                        CompanyName = model.CompanyName,
                        AddressOfCompany = model.AddressOfCompany,
                        Email = model.Email,
                        Phone = model.Phone,
                        User = user
                    };
                    _repository.AddEntity(newEmployer);
                    _repository.SaveChanges();
                    return View("index");
                }
                else
                {
                    return View("index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save a new order: {ex}");
            }
            return View("index");
        }
    }
}
