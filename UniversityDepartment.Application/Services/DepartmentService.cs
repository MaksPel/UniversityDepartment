using System.Linq.Expressions;
using Contracts;
using Contracts.Services;
using Microsoft.Extensions.Caching.Memory;
using UniversityDepartment.Domain.Models;

namespace UniversityDepartment.Application.Services;

internal sealed class DepartmentService : IDepartmentService
{
	private readonly IRepositoryManager _rep;
	private readonly IMemoryCache _cache;
	private int _rowsNumber = 20;

	public DepartmentService(IRepositoryManager rep, IMemoryCache memoryCache)
	{
		_rep = rep;
		_cache = memoryCache;
	}

	public IEnumerable<Department> GetDepartments()
	{
		return _rep.Departments.GetDepartmentsTop(_rowsNumber);
	}

	public void AddDepartments(string cacheKey)
	{
		IEnumerable<Department> departments = _rep.Departments.GetDepartmentsTop(_rowsNumber);
		_cache.Set(cacheKey, departments, new MemoryCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(298)
		});
	}

	public IEnumerable<Department>? GetDepartments(string cacheKey)
	{
		if (!_cache.TryGetValue(cacheKey, out IEnumerable<Department>? departments))
		{
			departments = _rep.Departments.GetDepartmentsTop(_rowsNumber);
			if (departments != null)
			{
				_cache.Set(cacheKey, departments,
				new MemoryCacheEntryOptions()
					.SetAbsoluteExpiration(TimeSpan.FromSeconds(298)));
			}
		}
		return departments;
	}

	public void AddDepartmentsByCondition(string cacheKey, Expression<Func<Department, bool>> expression)
	{
		IEnumerable<Department> departments = _rep.Departments.FindByCondition(expression).Take(_rowsNumber);
		_cache.Set(cacheKey, departments, new MemoryCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(298)
		});
	}
}