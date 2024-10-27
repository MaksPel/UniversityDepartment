using System.Linq.Expressions;
using UniversityDepartment.Domain.Models;

namespace Contracts.Repositories;

public interface ITeacherRepository
{
	Task<IEnumerable<Teacher>> GetAllTeachersAsync(bool trackChanges);
	IEnumerable<Teacher> GetTopTeachers(int rows);
	IQueryable<Teacher> FindByCondition(Expression<Func<Teacher, bool>> expression, bool trackChanges = false);
}
