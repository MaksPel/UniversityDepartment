namespace ConsoleApp.Models;

public partial class Subject
{
	public Guid SubjectId { get; set; }

	public string Name { get; set; } = null!;

	public int LectureHours { get; set; }

	public int? PracticalHours { get; set; }

	public int? LabHours { get; set; }

	public string? ReportingType { get; set; }

	public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

	public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}
