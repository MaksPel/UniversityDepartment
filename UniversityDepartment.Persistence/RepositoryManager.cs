using Contracts;
using Contracts.Repositories;
using UniversityDepartment.Persistence.Repositories;

namespace UniversityDepartment.Persistence
{
	public class RepositoryManager : IRepositoryManager
	{
		private readonly UniversityDepartmentContext _dbContext;

		private readonly Lazy<IDepartmentRepository> _depRep;
		private readonly Lazy<ICourseRepository> _courseRep;
		private readonly Lazy<IFacultyRepository> _facultyRep;
		private readonly Lazy<ISpecialtyRepository> _specialtyRep;
		private readonly Lazy<ISubjectRepository> _subjectRep;
		private readonly Lazy<ITeacherRepository> _teacherRep;

		public RepositoryManager(UniversityDepartmentContext dbContext)
		{
			_dbContext = dbContext;
			_depRep = new Lazy<IDepartmentRepository>(() => new DepartmentRepository(_dbContext));
			_courseRep = new Lazy<ICourseRepository>(() => new CourseRepository(_dbContext));
			_facultyRep = new Lazy<IFacultyRepository>(() => new FacultyRepository(_dbContext));
			_specialtyRep = new Lazy<ISpecialtyRepository>(() => new SpecialtyRepository(_dbContext));
			_subjectRep = new Lazy<ISubjectRepository>(() => new SubjectRepository(_dbContext));
			_teacherRep = new Lazy<ITeacherRepository>(() => new TeacherRepository(_dbContext));
		}

		public IDepartmentRepository Departments => _depRep.Value;
		public ICourseRepository Courses => _courseRep.Value;
		public IFacultyRepository Faculties => _facultyRep.Value;
		public ISpecialtyRepository Specialties => _specialtyRep.Value;
		public ISubjectRepository Subjects => _subjectRep.Value;
		public ITeacherRepository Teachers => _teacherRep.Value;

		public async Task SaveAsync() => await _dbContext.SaveChangesAsync();
		public void SaveChanges() => _dbContext.SaveChanges();
	}
}