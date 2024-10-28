using System.Linq.Expressions;
using Contracts;
using Contracts.Services;
using Microsoft.Extensions.Caching.Memory;
using UniversityDepartment.Domain.Models;

namespace UniversityDepartment.Application.Services;

internal sealed class FacultyService : IFacultyService
{
	private readonly IRepositoryManager _rep;
	private readonly IMemoryCache _cache;
	private int _rowsNumber = 20;

	public FacultyService(IRepositoryManager rep, IMemoryCache memoryCache)
	{
		_rep = rep;
		_cache = memoryCache;
	}

	public IEnumerable<Faculty> GetFaculties()
	{
		return _rep.Faculties.GetTopFaculties(_rowsNumber);
	}

	public void AddFaculties(string cacheKey)
	{
		IEnumerable<Faculty> faculties = _rep.Faculties.GetTopFaculties(_rowsNumber);
		_cache.Set(cacheKey, faculties, new MemoryCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(290)
		});
	}

	public IEnumerable<Faculty>? GetFaculties(string cacheKey)
	{
		if (!_cache.TryGetValue(cacheKey, out IEnumerable<Faculty>? faculties))
		{
			faculties = _rep.Faculties.GetTopFaculties(_rowsNumber);
			if (faculties != null)

			{
				_cache.Set(cacheKey, faculties,
				new MemoryCacheEntryOptions()
					.SetAbsoluteExpiration(TimeSpan.FromSeconds(290)));
			}
		}
		return faculties;
	}

	public void AddFacultiesByCondition(string cacheKey, Expression<Func<Faculty, bool>> expression)
	{
		IEnumerable<Faculty> faculties = _rep.Faculties.FindByCondition(expression).Take(_rowsNumber);
		_cache.Set(cacheKey, faculties, new MemoryCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(290)
		});
	}
}