using System.Linq.Expressions;
using Contracts;
using Contracts.Services;
using Microsoft.Extensions.Caching.Memory;
using UniversityDepartment.Domain.Models;

namespace UniversityDepartment.Application.Services;

internal sealed class SpecialtyService : ISpecialtyService
{
	private readonly IRepositoryManager _rep;
	private readonly IMemoryCache _cache;
	private int _rowsNumber = 20;

	public SpecialtyService(IRepositoryManager rep, IMemoryCache memoryCache)
	{
		_rep = rep;
		_cache = memoryCache;
	}

	public IEnumerable<Specialty> GetSpecialties()
	{
		return _rep.Specialties.GetTopSpecialties(_rowsNumber);
	}

	public void AddSpecialties(string cacheKey)
	{
		IEnumerable<Specialty> specialties = _rep.Specialties.GetTopSpecialties(_rowsNumber);
		_cache.Set(cacheKey, specialties, new MemoryCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(290)
		});
	}

	public IEnumerable<Specialty>? GetSpecialties(string cacheKey)
	{
		if (!_cache.TryGetValue(cacheKey, out IEnumerable<Specialty>? specialties))
		{
			specialties = _rep.Specialties.GetTopSpecialties(_rowsNumber);
			if (specialties != null)
			{
				_cache.Set(cacheKey, specialties,
				new MemoryCacheEntryOptions()
					.SetAbsoluteExpiration(TimeSpan.FromSeconds(290)));
			}
		}
		return specialties;
	}

	public void AddSpecialtiesByCondition(string cacheKey, Expression<Func<Specialty, bool>> expression)
	{
		IEnumerable<Specialty> specialties = _rep.Specialties.FindByCondition(expression).Take(_rowsNumber);
		_cache.Set(cacheKey, specialties, new MemoryCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(290)
		});
	}
}