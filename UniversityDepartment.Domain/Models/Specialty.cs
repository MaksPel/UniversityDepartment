namespace UniversityDepartment.Domain.Models;

public class Specialty
{
	public Guid SpecialtyId { get; set; }

	public string Name { get; set; } = null!;

	public Guid DepartmentId { get; set; }

	public virtual ICollection<Course> Courses { get; set; } = [];

	public virtual Department Department { get; set; } = null!;
}
