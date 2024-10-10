namespace ConsoleApp.Models;

public partial class Course
{
	public Guid CourseId { get; set; }

	public int CourseNumber { get; set; }

	public int SemesterNumber { get; set; }

	public Guid SpecialtyId { get; set; }

	public virtual Specialty Specialty { get; set; } = null!;

	public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}
