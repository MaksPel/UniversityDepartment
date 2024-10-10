namespace ConsoleApp.Models;

public partial class Faculty
{
	public Guid FacultyId { get; set; }

	public string Name { get; set; } = null!;

	public virtual ICollection<Department> Departments { get; set; } = new List<Department>();
}
