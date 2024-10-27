using Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using UniversityDepartment.Domain.Models;

namespace UniversityDepartment.Persistence.Repositories
{
	internal class TeacherRepository : RepositoryBase<Teacher>, ITeacherRepository
	{
		private readonly UniversityDepartmentContext _dbContext;

		public TeacherRepository(UniversityDepartmentContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IEnumerable<Teacher>> GetAllTeachersAsync(bool trackChanges = false) =>
			await FindAll(trackChanges)
				.OrderBy(t => t.Name)
				.ToListAsync();

		public IEnumerable<Teacher> GetTopTeachers(int rows) =>
			[.. FindAll().Include(x => x.Subjects).Take(rows)];
	}
}