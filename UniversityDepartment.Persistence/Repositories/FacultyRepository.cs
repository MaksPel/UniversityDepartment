using Contracts.Repositories;
using UniversityDepartment.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace UniversityDepartment.Persistence.Repositories
{
	internal class FacultyRepository : RepositoryBase<Faculty>, IFacultyRepository
	{
		private readonly UniversityDepartmentContext _dbContext;

		public FacultyRepository(UniversityDepartmentContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IEnumerable<Faculty>> GetAllFacultiesAsync(bool trackChanges = false) =>
			await FindAll(trackChanges)
				.OrderBy(f => f.Name)
				.ToListAsync();

		public IEnumerable<Faculty> GetTopFaculties(int rows) =>
			[.. FindAll().Include(x => x.Departments).Take(rows)];
	}
}