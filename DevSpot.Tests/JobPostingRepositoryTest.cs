using DevSpot.Data;
using DevSpot.Models;
using DevSpot.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSpot.Tests
{
    public class JobPostingRepositoryTest
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;
        public JobPostingRepositoryTest()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("JobPostingDb")
                .Options;

        }
        private ApplicationDbContext CreateDbContext() => new ApplicationDbContext(_options);

        [Fact]
        public async Task AddAsync_ShouldAddJobPosting()
        {
            //db context
            var db = CreateDbContext();
            //Job posting Repo
            var repo = new JobPostingRepository(db);
            //job posting
            var jobPosting = new JobPosting
            {
                Title = "Test title add",
                Description = "Test des",
                PostedDate = DateTime.Now,
                Company = "company test",
                Location = "Location test",
                UserId = "1234"

            };
            //excute
            await repo.AddAsync(jobPosting);
            //result
            var result = db.JobPostings.Find(jobPosting.Id);
            //assert
            Assert.NotNull(result);
            Assert.Equal("Test title add", result.Title);
        }
        [Fact]
        public async Task GetByIdAsync_shouldReturnjobPosting()
        {
            //db context
            var db = CreateDbContext();
            //Job posting Repo
            var repo = new JobPostingRepository(db);
            //job posting
            var jobPosting = new JobPosting
            {
                Title = "Test title",
                Description = "Test des",
                PostedDate = DateTime.Now,
                Company = "company test",
                Location = "Location test",
                UserId = "1234"

            };
            //excute
            await db.JobPostings.AddAsync(jobPosting);
            await db.SaveChangesAsync();
            //result
            var result = await repo.GetByIdAsync(jobPosting.Id);
            //assert
            Assert.NotNull(result);
            Assert.Equal("Test title", result.Title);
        }


        [Fact]
        public async Task GetByIdAsync_ShouldthrowKeyNotFoundErrorException()
        {
            var db = CreateDbContext();
            var repo = new JobPostingRepository(db);
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => repo.GetByIdAsync(99));

        }

        [Fact]
        public async Task GetAllJobPosting_shouldReturnAllJP() 
        {

            //db context
            var db = CreateDbContext();
            //Job posting Repo
            var repo = new JobPostingRepository(db);
            //job posting
            var jobPosting = new JobPosting
            {
                Title = "Test title",
                Description = "Test des",
                PostedDate = DateTime.Now,
                Company = "company test",
                Location = "Location test",
                UserId = "1234"

            };
            var jobPosting2 = new JobPosting
            {
                Title = "Test title2",
                Description = "Test des2",
                PostedDate = DateTime.Now,
                Company = "company test2",
                Location = "Location test2",
                UserId = "1234"

            };


            await db.JobPostings.AddRangeAsync(jobPosting,jobPosting2);
            await db.SaveChangesAsync();

            var result = await repo.GetAllAsinc();
            Assert.NotNull(result);
            Assert.True( result.Count()>=2);
        }
        [Fact]
        public async Task UpdateAsync_ShouldUpdateJP()
        {
            //db context
            var db = CreateDbContext();
            //Job posting Repo
            var repo = new JobPostingRepository(db);
            //job posting
            var jobPosting = new JobPosting
            {
                Title = "Test title",
                Description = "Test des",
                PostedDate = DateTime.Now,
                Company = "company test",
                Location = "Location test",
                UserId = "1234"

            };
            await db.JobPostings.AddAsync(jobPosting);  
            await db.SaveChangesAsync();    

            jobPosting.Description = "Description Updated";
            await repo.UpdateAsync(jobPosting);
            var result = db.JobPostings.Find(jobPosting.Id); 
            
            Assert.NotNull(result);
            Assert.Equal("Description Updated", result.Description);


        }
        [Fact]
        public async Task Delete_ShouldDeleteJP()
        {

            //db context
            var db = CreateDbContext();
            //Job posting Repo
            var repo = new JobPostingRepository(db);
            //job posting
            var jobPosting = new JobPosting
            {
                Title = "Test title",
                Description = "Test des",
                PostedDate = DateTime.Now,
                Company = "company test",
                Location = "Location test",
                UserId = "TestUserID"

            };
            await db.JobPostings.AddAsync(jobPosting);
            await db.SaveChangesAsync();

            await repo.DeleteAsync(jobPosting.Id);

            var result = db.JobPostings.Find(jobPosting.Id);
            Assert.Null(result);
        }

    }


}
