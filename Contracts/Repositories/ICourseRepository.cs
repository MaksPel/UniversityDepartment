using System.Linq.Expressions;
using UniversityDepartment.Domain.Models;

namespace Contracts.Repositories;

public interface ICourseRepository
{
    Task<IEnumerable<Course>> GetAllCoursesAsync(bool trackChanges);
    IEnumerable<Course> GetTopCourses(int rows);
    IQueryable<Course> FindByCondition(Expression<Func<Course, bool>> expression, bool trackChanges = false);
}

