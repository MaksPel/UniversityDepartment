using Contracts.Repositories;

namespace Contracts;

public interface IRepositoryManager
{
	ICourseRepository Courses { get; }
	IDepartmentRepository Departments { get; }
	IFacultyRepository Faculties { get; }
	ISpecialtyRepository Specialties { get; }
	ISubjectRepository Subjects { get; }
	ITeacherRepository Teachers { get; }
	Task SaveAsync();
	void SaveChanges();
}
