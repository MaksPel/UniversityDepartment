using System.Linq.Expressions;
using Contracts;
using Contracts.Services;
using Microsoft.Extensions.Caching.Memory;
using UniversityDepartment.Domain.Models;

namespace UniversityDepartment.Application.Services;

internal sealed class SubjectService : ISubjectService
{
	private readonly IRepositoryManager _rep;
	private readonly IMemoryCache _cache;
	private int _rowsNumber = 20;

	public SubjectService(IRepositoryManager rep, IMemoryCache memoryCache)
	{
		_rep = rep;
		_cache = memoryCache;
	}

	public IEnumerable<Subject> GetSubjects()
	{
		return _rep.Subjects.GetTopSubjects(_rowsNumber);
	}

	public void AddSubjects(string cacheKey)
	{
		IEnumerable<Subject> subjects = _rep.Subjects.GetTopSubjects(_rowsNumber);
		_cache.Set(cacheKey, subjects, new MemoryCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(290)
		});
	}

	public IEnumerable<Subject>? GetSubjects(string cacheKey)
	{
		if (!_cache.TryGetValue(cacheKey, out IEnumerable<Subject>? subjects))
		{
			subjects = _rep.Subjects.GetTopSubjects(_rowsNumber);
			if (subjects != null)
			{
				_cache.Set(cacheKey, subjects,
				new MemoryCacheEntryOptions()
					.SetAbsoluteExpiration(TimeSpan.FromSeconds(290)));
			}
		}
		return subjects;
	}

	public void AddSubjectsByCondition(string cacheKey, Expression<Func<Subject, bool>> expression)
	{
		IEnumerable<Subject> subjects = _rep.Subjects.FindByCondition(expression).Take(_rowsNumber);
		_cache.Set(cacheKey, subjects, new MemoryCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(290)
		});
	}
}