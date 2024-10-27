using Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using UniversityDepartment.Domain.Models;

namespace UniversityDepartment.Persistence.Repositories
{
	internal class CourseRepository : RepositoryBase<Course>, ICourseRepository
	{
		private readonly UniversityDepartmentContext _dbContext;

		public CourseRepository(UniversityDepartmentContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IEnumerable<Course>> GetAllCoursesAsync(bool trackChanges = false) =>
			await FindAll(trackChanges)
				.OrderBy(c => c.CourseNumber)
				.ToListAsync();

		public IEnumerable<Course> GetTopCourses(int rows) =>
			[.. FindAll().Include(x => x.Subjects).Include(x => x.Specialty).Take(rows)];
	}
}