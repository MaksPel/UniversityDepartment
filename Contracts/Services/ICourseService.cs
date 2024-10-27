using System.Linq.Expressions;
using UniversityDepartment.Domain.Models;

namespace Contracts.Services;

public interface ICourseService
{
	IEnumerable<Course> GetCourses();
	void AddCourses(string cacheKey);
	IEnumerable<Course>? GetCourses(string cacheKey);
	void AddCoursesByCondition(string cacheKey, Expression<Func<Course, bool>> expression);
}
