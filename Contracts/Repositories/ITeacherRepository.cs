using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UniversityDepartment.Domain.Models;

namespace Contracts.Repositories;

public interface ITeacherRepository
{
	Task<IEnumerable<Teacher>> GetAllTeachersAsync(bool trackChanges);
	IEnumerable<Teacher> GetTopTeachers(int rows);
	IQueryable<Teacher> FindByCondition(Expression<Func<Teacher, bool>> expression, bool trackChanges = false);
}
