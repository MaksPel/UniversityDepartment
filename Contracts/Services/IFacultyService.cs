using System.Linq.Expressions;
using UniversityDepartment.Domain.Models;

namespace Contracts.Services;

public interface IFacultyService
{
	IEnumerable<Faculty> GetFaculties();
	void AddFaculties(string cacheKey);
	IEnumerable<Faculty>? GetFaculties(string cacheKey);
	void AddFacultiesByCondition(string cacheKey, Expression<Func<Faculty, bool>> expression);
}
