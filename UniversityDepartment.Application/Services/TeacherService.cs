using System.Linq.Expressions;
using Contracts;
using Contracts.Services;
using Microsoft.Extensions.Caching.Memory;
using UniversityDepartment.Domain.Models;

namespace UniversityDepartment.Application.Services;

internal sealed class TeacherService : ITeacherService
{
	private readonly IRepositoryManager _rep;
	private readonly IMemoryCache _cache;
	private int _rowsNumber = 20;

	public TeacherService(IRepositoryManager rep, IMemoryCache memoryCache)
	{
		_rep = rep;
		_cache = memoryCache;
	}

	public IEnumerable<Teacher> GetTeachers()
	{
		return _rep.Teachers.GetTopTeachers(_rowsNumber);
	}

	public void AddTeachers(string cacheKey)
	{
		IEnumerable<Teacher> teachers = _rep.Teachers.GetTopTeachers(_rowsNumber);
		_cache.Set(cacheKey, teachers, new MemoryCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(298)
		});
	}

	public IEnumerable<Teacher>? GetTeachers(string cacheKey)
	{
		if (!_cache.TryGetValue(cacheKey, out IEnumerable<Teacher>? teachers))
		{
			teachers = _rep.Teachers.GetTopTeachers(_rowsNumber);
			if (teachers != null)
			{
				_cache.Set(cacheKey, teachers,
				new MemoryCacheEntryOptions()
					.SetAbsoluteExpiration(TimeSpan.FromSeconds(298)));
			}
		}
		return teachers;
	}

	public void AddTeachersByCondition(string cacheKey, Expression<Func<Teacher, bool>> expression)
	{
		IEnumerable<Teacher> teachers = _rep.Teachers.FindByCondition(expression).Take(_rowsNumber);
		_cache.Set(cacheKey, teachers, new MemoryCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(298)
		});
	}
}