using System.Linq.Expressions;
using Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace UniversityDepartment.Persistence.Repositories;

internal abstract class RepositoryBase<T>(UniversityDepartmentContext db) : IRepositoryBase<T> where T : class
{
	protected UniversityDepartmentContext _db = db;

	public IQueryable<T> FindAll(bool trackChanges = false) =>
		!trackChanges ? _db.Set<T>()
		.AsNoTracking() : _db.Set<T>();

	public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false) =>
		!trackChanges ? _db.Set<T>().Where(expression)
		.AsNoTracking() : _db.Set<T>().Where(expression);

	public void Create(T entity) => _db.Set<T>().Add(entity);
	public void Update(T entity) => _db.Set<T>().Update(entity);
	public void Delete(T entity) => _db.Set<T>().Remove(entity);
}
