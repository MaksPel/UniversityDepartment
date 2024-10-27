using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UniversityDepartment.Domain.Models;

namespace Contracts.Services;

public interface IFacultyService
{
	IEnumerable<Faculty> GetFaculties();
	void AddFaculties(string cacheKey);
	IEnumerable<Faculty>? GetFaculties(string cacheKey);
	void AddFacultiesByCondition(string cacheKey, Expression<Func<Faculty, bool>> expression);
}
