using System.Linq.Expressions;
using UniversityDepartment.Domain.Models;

namespace Contracts.Repositories;

public interface ISubjectRepository
{
	Task<IEnumerable<Subject>> GetAllSubjectsAsync(bool trackChanges);
	IEnumerable<Subject> GetTopSubjects(int rows);
	IQueryable<Subject> FindByCondition(Expression<Func<Subject, bool>> expression, bool trackChanges = false);
}

