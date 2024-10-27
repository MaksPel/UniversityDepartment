using System.Linq.Expressions;
using UniversityDepartment.Domain.Models;


namespace Contracts.Repositories;

public interface IFacultyRepository
{
	Task<IEnumerable<Faculty>> GetAllFacultiesAsync(bool trackChanges);
	IEnumerable<Faculty> GetTopFaculties(int rows);
	IQueryable<Faculty> FindByCondition(Expression<Func<Faculty, bool>> expression, bool trackChanges = false);
}

