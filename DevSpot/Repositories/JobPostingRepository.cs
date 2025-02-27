using DevSpot.Data;
using DevSpot.Models;
using Microsoft.EntityFrameworkCore;

namespace DevSpot.Repositories
{
    public class JobPostingRepository : IRepository<JobPosting>
    {
        private readonly ApplicationDbContext _context;
        public JobPostingRepository(ApplicationDbContext _context)
        {
            this._context = _context; 
        }
        public async Task AddAsync(JobPosting entity) //this function gets called and  entity has missing data regarding user ID
        {
          await _context.JobPostings.AddAsync(entity);  
            await _context.SaveChangesAsync();  
        }

        public async Task DeleteAsync(int id)
        {
            var jobPosting = await _context.JobPostings.FindAsync(id);

            if (jobPosting == null)
            {
                throw new KeyNotFoundException();
            }
            _context.JobPostings.Remove(jobPosting);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<JobPosting>> GetAllAsinc()
        {
           return await _context.JobPostings.ToListAsync();
        }

        public async Task<JobPosting> GetByIdAsync(int Id)
        {
            var jobPosting = await _context.JobPostings.FindAsync(Id);
            if (jobPosting == null)
            {
                throw new KeyNotFoundException();
            }
            
            return jobPosting;
        }

        public async Task UpdateAsync(JobPosting entity)
        {
         _context.JobPostings.Update(entity);
           await _context.SaveChangesAsync();
        }
    }
}
