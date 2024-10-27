using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UniversityDepartment.Domain.Models;

namespace Contracts.Services;

public interface ISubjectService
{
	IEnumerable<Subject> GetSubjects();
	void AddSubjects(string cacheKey);
	IEnumerable<Subject>? GetSubjects(string cacheKey);
	void AddSubjectsByCondition(string cacheKey, Expression<Func<Subject, bool>> expression);
}
