using System.Linq.Expressions;
using Contracts;
using Contracts.Services;
using Microsoft.Extensions.Caching.Memory;
using UniversityDepartment.Domain.Models;

namespace UniversityDepartment.Application.Services;

internal sealed class CourseService : ICourseService
{
	private readonly IRepositoryManager _rep;
	private readonly IMemoryCache _cache;
	private int _rowsNumber = 20;

	public CourseService(IRepositoryManager rep, IMemoryCache memoryCache)
	{
		_rep = rep;
		_cache = memoryCache;
	}

	public IEnumerable<Course> GetCourses()
	{
		return _rep.Courses.GetTopCourses(_rowsNumber);
	}

	public void AddCourses(string cacheKey)
	{
		IEnumerable<Course> courses = _rep.Courses.GetTopCourses(_rowsNumber);
		_cache.Set(cacheKey, courses, new MemoryCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(290)
		});
	}

	public IEnumerable<Course>? GetCourses(string cacheKey)
	{
		if (!_cache.TryGetValue(cacheKey, out IEnumerable<Course>? courses))
		{
			courses = _rep.Courses.GetTopCourses(_rowsNumber);
			if (courses != null)
			{
				_cache.Set(cacheKey, courses,
				new MemoryCacheEntryOptions()
					.SetAbsoluteExpiration(TimeSpan.FromSeconds(290)));
			}
		}
		return courses;
	}

	public void AddCoursesByCondition(string cacheKey, Expression<Func<Course, bool>> expression)
	{
		IEnumerable<Course> courses = _rep.Courses.FindByCondition(expression).Take(_rowsNumber);
		_cache.Set(cacheKey, courses, new MemoryCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(290)
		});
	}
}