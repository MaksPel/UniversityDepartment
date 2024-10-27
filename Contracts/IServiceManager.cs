using Contracts.Services;

namespace Contracts;

public interface IServiceManager
{
	IDepartmentService DepartmentService { get; }
	ICourseService CourseService { get; }
	IFacultyService FacultyService { get; }
	ISpecialtyService SpecialtyService { get; }
	ISubjectService SubjectService { get; }
	ITeacherService TeacherService { get; }
}
