using Contracts.Repositories;
using UniversityDepartment.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace UniversityDepartment.Persistence.Repositories
{
	internal class SubjectRepository : RepositoryBase<Subject>, ISubjectRepository
	{
		private readonly UniversityDepartmentContext _dbContext;

		public SubjectRepository(UniversityDepartmentContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IEnumerable<Subject>> GetAllSubjectsAsync(bool trackChanges = false) =>
			await FindAll(trackChanges)
				.OrderBy(s => s.Name) 
				.ToListAsync();

		public IEnumerable<Subject> GetTopSubjects(int rows) =>
			[.. FindAll().Take(rows)];
	}
}