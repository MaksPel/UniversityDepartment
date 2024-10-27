using System.Linq.Expressions;
using UniversityDepartment.Domain.Models;

namespace Contracts.Repositories;

public interface ISpecialtyRepository
{
	Task<IEnumerable<Specialty>> GetAllSpecialtiesAsync(bool trackChanges);
	IEnumerable<Specialty> GetTopSpecialties(int rows);
	IQueryable<Specialty> FindByCondition(Expression<Func<Specialty, bool>> expression, bool trackChanges = false);
}

