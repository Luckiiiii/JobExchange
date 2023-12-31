﻿using JobExchange.Data;
using JobExchange.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime;

namespace JobExchange.Controllers
{
    public class AppController : Controller
    {
        private readonly IJobExchangeRepository _repository;
        private readonly UserManager<StoreUser> _userManager;
        private readonly ILogger<AppController> _logger;

        public AppController(IJobExchangeRepository repository, UserManager<StoreUser>userManager, ILogger<AppController> logger) 
        {
            _repository = repository;
            _userManager = userManager;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult EmployerView() 
        {
            ViewBag.Title = "Employer View";
            return View();
        }
        //Hien tat ca thong tin tuyen dung
        public IActionResult ShowJobInfo()
        {
            var allJobInfo = _repository.GetAllJobs(); // Lấy thông tin của tất cả các JobInfo từ repository
            return View(allJobInfo);
        }
        public async Task <IActionResult> ShowJobInfoUser()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                var userId = user.Id;
                var existingJobInfo = await _repository.GetJobInfosByUserId(userId);
                return View(existingJobInfo);
            }
            
            return RedirectToAction("Index");
        }
        //cap nhat thong tin tuyen dung
        public IActionResult UpdateJobInfo(int id)
        {
            var job = _repository.GetJobById(id);
            var typeJobs = _repository.GetTypeJobs();

            // Gán danh sách TypeJob vào ViewBag
            ViewBag.ListTypeJobs = new SelectList(typeJobs, "Id", "NameJob", job.TypeJob.Id);

            return View(job);
        }
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
            return RedirectToAction("ShowJobInfo");
        }
        //Xoa thong tin tuyen dung cu the
        public IActionResult DeleteJobInfo(int id)
        {
            _repository.DeleteJobInfo(id);
            return RedirectToAction("ShowJobInfo");
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
        [HttpPost]
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
                    return RedirectToAction("ShowJobInfo");
                }
            }
            return View();
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
        //Them nha tuyen dung
        public async Task<IActionResult> AddEmployer (Employer model)
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


    }
}
