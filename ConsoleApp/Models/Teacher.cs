namespace ConsoleApp.Models;

public partial class Teacher
{
	public Guid TeacherId { get; set; }

	public string Name { get; set; } = null!;

	public string Surname { get; set; } = null!;

	public string? Midname { get; set; }

	public string? Position { get; set; }

	public int? Age { get; set; }

	public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}
