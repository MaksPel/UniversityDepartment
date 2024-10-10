namespace ConsoleApp.Models;

public partial class Department
{
	public Guid DepartmentId { get; set; }

	public string Name { get; set; } = null!;

	public Guid FacultyId { get; set; }

	public bool IsGraduating { get; set; }

	public virtual Faculty Faculty { get; set; } = null!;

	public virtual ICollection<Specialty> Specialties { get; set; } = new List<Specialty>();
}
