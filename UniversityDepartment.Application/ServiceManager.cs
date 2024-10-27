using Contracts;
using Contracts.Services;
using Microsoft.Extensions.Caching.Memory;

namespace UniversityDepartment.Application.Services;

public class ServiceManager : IServiceManager
{
	private readonly IRepositoryManager _repManager;
	private readonly IMemoryCache _cache;

	private readonly Lazy<IDepartmentService> _depService;
	private readonly Lazy<ICourseService> _courseService;
	private readonly Lazy<IFacultyService> _facultyService;
	private readonly Lazy<ISpecialtyService> _specialtyService;
	private readonly Lazy<ISubjectService> _subjectService;
	private readonly Lazy<ITeacherService> _teacherService;

	public ServiceManager(IRepositoryManager repManager, IMemoryCache cache)
	{
		_repManager = repManager;
		_cache = cache;

		_depService = new(() => new DepartmentService(_repManager, _cache));
		_courseService = new(() => new CourseService(_repManager, _cache));
		_facultyService = new(() => new FacultyService(_repManager, _cache));
		_specialtyService = new(() => new SpecialtyService(_repManager, _cache));
		_subjectService = new(() => new SubjectService(_repManager, _cache));
		_teacherService = new(() => new TeacherService(_repManager, _cache));
	}

	public IDepartmentService DepartmentService => _depService.Value;
	public ICourseService CourseService => _courseService.Value;
	public IFacultyService FacultyService => _facultyService.Value;
	public ISpecialtyService SpecialtyService => _specialtyService.Value;
	public ISubjectService SubjectService => _subjectService.Value;
	public ITeacherService TeacherService => _teacherService.Value;
}