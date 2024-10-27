using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UniversityDepartment.Domain.Models;

namespace Contracts.Services;

public interface ITeacherService
{
	IEnumerable<Teacher> GetTeachers();
	void AddTeachers(string cacheKey);
	IEnumerable<Teacher>? GetTeachers(string cacheKey);
	void AddTeachersByCondition(string cacheKey, Expression<Func<Teacher, bool>> expression);
}
