using System.Linq.Expressions;
using UniversityDepartment.Domain.Models;

namespace Contracts.Services;
public interface ISpecialtyService
{
	IEnumerable<Specialty> GetSpecialties();
	void AddSpecialties(string cacheKey);
	IEnumerable<Specialty>? GetSpecialties(string cacheKey);
	void AddSpecialtiesByCondition(string cacheKey, Expression<Func<Specialty, bool>> expression);
}
