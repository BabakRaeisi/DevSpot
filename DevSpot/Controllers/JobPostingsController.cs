using DevSpot.Models;
 
using DevSpot.Repositories;
using DevSpot.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DevSpot.Controllers
{
    [Authorize]
    public class JobPostingsController : Controller
    {
        private readonly IRepository<JobPosting> _repository;
        private readonly UserManager<IdentityUser>_userManager;

        public JobPostingsController(IRepository<JobPosting> repo , UserManager<IdentityUser> user)
        {
            _repository = repo;
            _userManager = user;
        }
       [AllowAnonymous]
        public async Task <IActionResult> Index()
        {
            var jobPostings = await _repository.GetAllAsinc();
            return View(jobPostings);
        }

       [Authorize(Roles ="Admin,Employer")] 
        public IActionResult Create()
        {
            return View();
        }
      
        [HttpPost]
        [Authorize(Roles = "Admin,Employer")]
        public async Task<IActionResult> Create(JobPostingViewModel jobPostingvm) // I guess this should get called but I get null refrence somewhere else 
        {
            // we need userID 
            if (ModelState.IsValid)// is used to check when you want to post new data ,the data is suppose to be valid 
            {

                var jobPosting = new JobPosting
                {
                    
                    Title = jobPostingvm.Title,
                    Description = jobPostingvm.Description,
                    Company= jobPostingvm.Company,
                    Location= jobPostingvm.Location,
                    UserId= _userManager.GetUserId(User)
                };
                System.Console.WriteLine(_userManager.GetUserId(User));
            
                await _repository.AddAsync(jobPosting);
                return RedirectToAction(nameof(Index));
            }
            return View(jobPostingvm);
           
        }
        [HttpDelete]
        [Authorize(Roles = "Admin,Employer")]
        public async Task<IActionResult> Delete(int id)
        {

            return Ok();
        }


    }
}
