using Microsoft.AspNetCore.Mvc;
using MODELS;
using Newtonsoft.Json;
using System.Text;

namespace EmployeeManagement.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        public EmployeeController(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _apiUrl = configuration["ApiUrl"];
        }
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetStringAsync($"{_apiUrl}/api/employee");
            var employees = JsonConvert.DeserializeObject<List<Employee>>(response);
            return View(employees);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(employee), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_apiUrl}/api/employee", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(employee);
        }
    }
}
