using AutoMapper;
using JobExchange.Data;
using JobExchange.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;


namespace JobExchange.Controllers
{
    
    [Route("api/[Controller]")]
    public class EmployerController : Controller
    {
        private readonly IJobExchangeRepository _repository;
        private readonly ILogger _logger;

        //private readonly IMapper _mapper;

        public EmployerController(IJobExchangeRepository repository, ILogger logger /*IMapper mapper*/) 
        {
            _repository = repository;
            //_mapper = mapper;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var order = _repository.GetEmployerById(id);
                if (order != null) return Ok(order);
                else return NotFound();

            }
            catch (Exception ex)
            {
                //_logger.LogError($"failed to get orders: {ex}");
                return BadRequest("failed to get orders");
            }
        }

        //[HttpPost]
        /*public IActionResult Post(int employerId, [FromBody] JobInfo model)
        {
            try
            {
                var employer = _repository.GetEmployerById(employerId); // Lấy Order từ cơ sở dữ liệu
                if (employer== null)
                {
                    return NotFound();
                }

                var newItem = new JobInfo
                {
                    Describe = model.Describe,
                    Salary = model.Salary,
                    TypeOfJob = model.TypeOfJob,
                    Employer = employer
                    
                };

                employer.Items.Add(newItem);
                _repository.UpdateEmployer(employer); // Cập nhật Order vào cơ sở dữ liệu
                _repository.SaveAll(); // Lưu thay đổi

                return Ok(newItem); // Trả về thông tin về OrderItem mới được thêm vào
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add order item to order: {ex}");
                return BadRequest("Failed to add order item to order");
            }
        }*/


    }
}
