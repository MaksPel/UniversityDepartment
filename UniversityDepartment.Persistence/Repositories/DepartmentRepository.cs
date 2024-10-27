using Microsoft.EntityFrameworkCore;
using Contracts.Repositories;
using UniversityDepartment.Domain.Models;
using UniversityDepartment.Persistence.Repositories;

namespace UniversityDepartment.Persistence.Repositories;

internal class DepartmentRepository(UniversityDepartmentContext dbContext) : RepositoryBase<Department>(dbContext), IDepartmentRepository
{
	private readonly UniversityDepartmentContext _dbContext = dbContext;

	public async Task<IEnumerable<Department>> GetAllDepartmentsAsync(bool trackChanges = false) =>
		await FindAll(trackChanges)
			.OrderBy(c => c.Name)
			.ToListAsync();

	public IEnumerable<Department> GetDepartmentsTop(int rows) =>
		 [.. FindAll().Include(x => x.Specialties).Include(x => x.Faculty).Take(rows)];
}

