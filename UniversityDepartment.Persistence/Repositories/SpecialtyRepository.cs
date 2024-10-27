using Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using UniversityDepartment.Domain.Models;

namespace UniversityDepartment.Persistence.Repositories
{
	internal class SpecialtyRepository : RepositoryBase<Specialty>, ISpecialtyRepository
	{
		private readonly UniversityDepartmentContext _dbContext;

		public SpecialtyRepository(UniversityDepartmentContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IEnumerable<Specialty>> GetAllSpecialtiesAsync(bool trackChanges = false) =>
			await FindAll(trackChanges)
				.OrderBy(s => s.Name)
				.ToListAsync();

		public IEnumerable<Specialty> GetTopSpecialties(int rows) =>
			[.. FindAll().Include(x => x.Courses).Include(x => x.Department).Take(rows)];
	}
}