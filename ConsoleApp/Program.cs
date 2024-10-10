using Azure;
using ConsoleApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ConsoleApp;

internal class Program
{
	static void Print<T>(string sqlText, IEnumerable<T>? items)
	{
		Console.WriteLine(sqlText);
		Console.WriteLine("Записи: ");
		if (!items.IsNullOrEmpty())
		{
			foreach (var item in items!)
			{
				Console.WriteLine(item?.ToString());
			}
		}
		else
		{
			Console.WriteLine("Пусто");
		}
		Console.WriteLine();
		Console.WriteLine("Нажмите любую клавишу");
		Console.ReadKey();
	}

	static void Select(UniversityDepartmentContext db)
	{
		var queryLINQ1 = from g in db.Faculties
						 select new
						 {
							 Название_Факультета = g.Name,
						 };
		string comment = "1. Результат выполнения запроса на выборку всех данных из таблицы, стоящей в схеме базы данных на стороне отношения 'Один'";
		Print(comment, queryLINQ1.Take(10).ToList());

		var queryLINQ2 = from emp in db.Teachers
						 where emp.Position == "Старший преподаватель"
						 select new
						 {
							 Имя_Преподавателя = emp.Name,
							 Должность_Преподавателя = emp.Position
						 };
		comment = "2. Результат выполнения запроса на выборку данных из таблицы, стоящей в схеме базы данных на стороне отношения 'Один', отфильтрованные по определенному условию";
		Print(comment, queryLINQ2.ToList());

		var queryLINQ3 = db.Departments.
						 Join(db.Faculties, m => m.FacultyId, g => g.FacultyId, (m, g) => new { m.FacultyId, m.IsGraduating, g.Name }).
						 GroupBy(m => new { m.FacultyId, m.Name }).
						 Select(gr => new
						 {
							 Название_Факультета = gr.Key.Name,
							 Количество_Выпускающих_кафедр = gr.Count(m => m.IsGraduating),
						 });
		comment = "3. Результат выполнения запроса на выборку данных из таблицы, стоящей в схеме базы данных на стороне отношения 'Многие', сгрупированных по любому из полей данных с выводом итогового результата";
		Print(comment, queryLINQ3.Take(10).ToList());

		var queryLINQ4 = from e in db.Specialties
						 join w in db.Departments
						 on e.DepartmentId equals w.DepartmentId
						 select new
						 {
							 Название_специальности = e.Name,
							 Название_кафедры = w.Name,
							 Выпускающая_кафедра = w.IsGraduating,
						 };
		comment = "4. Результат выполнения запроса на выборку данных двух полей из двух таблиц, связанных между собой отношением 'один-ко-многим'";
		Print(comment, queryLINQ4.Take(10).ToList());

		var queryLINQ5 = from e in db.Specialties
						 join w in db.Departments
						 on e.DepartmentId equals w.DepartmentId
						 where w.IsGraduating
						 select new
						 {
							 Название_специальности = e.Name,
							 Название_кафедры = w.Name,
							 Выпускающая_кафедра = w.IsGraduating,
						 };

		comment = "5. Результат выполнения запроса на выборку данных из двух таблиц, связанных между собой отношением 'один-ко-многим' и отфильтрованным по некоторому условию";
		Print(comment, queryLINQ5.ToList());
	}

	static void Insert(UniversityDepartmentContext db)
	{
		Faculty faculty = new Faculty
		{
			Name = "New faculty 1",
		};
		db.Faculties.Add(faculty);
		db.SaveChanges();
		string comment = "Выборка факультетов после вставки нового факультета";
		var queryLINQ1 = from g in db.Faculties
						 where g.Name == "New faculty 1"
						 select new
						 {
							 Нзавание_Факультета = g.Name
						 };

		var queryLINQ = from g in db.Faculties
						where g.Name.Length > 15
						select g.FacultyId;

		Guid faculty1 = queryLINQ.Take(1).First();

		Print(comment, queryLINQ1.ToList());

		Department op = new()
		{
			Name = "New department 1",
			IsGraduating = true,
			FacultyId = faculty1,
		};
		db.Departments.Add(op);
		db.SaveChanges();
		comment = "Выборка кафедр после вставки новой кафедры";
		var queryLINQ2 = from m in db.Departments
						 where m.Name == "New department 1"
						 select new
						 {
							 Название_кафедры = m.Name,
							 Выпускающая_кафедра = m.IsGraduating
						 };
		Print(comment, queryLINQ2.ToList());
	}

	static void Delete(UniversityDepartmentContext db)
	{
		string genreName = "New faculty 1";
		var genre = db.Faculties.Where(g => g.Name == genreName);

		if (genre != null)
		{
			db.Faculties.RemoveRange(genre);
			db.SaveChanges();
		}
		string comment = "Выборка факультетов после удаления";
		var queryLINQ1 = from g in db.Faculties
						 where g.Name == "New genre 1"
						 select new
						 {
							 Назавание_факультета= g.Name,
						 };
		Print(comment, queryLINQ1.ToList());

		string depName = "New department 1";
		var deps = db.Departments.Where(m => m.Name == depName);

		if (deps != null)
		{
			db.Departments.RemoveRange(deps);
			db.SaveChanges();
		}
		comment = "Выборка кафедр после удаления";
		var queryLINQ2 = from m in db.Departments
						 where m.Name == "New department 1"
						 select new
						 {
							 Название_кафедры = m.Name,
							 Выпускающая_кафедра = m.IsGraduating
						 };
		Print(comment, queryLINQ2.ToList());
	}

	static void Update(UniversityDepartmentContext db)
	{
		int amount = 2300;
		var subjs = db.Subjects.Where(w => w.LabHours > 100);
		if (!subjs.IsNullOrEmpty())
		{
			foreach (var s in subjs)
			{
				s.LabHours = 129;
			}
			db.SaveChanges();
		}

		string comment = "Выборка после обновления";
		var queryLINQ1 = from s in db.Subjects
						 where s.LabHours == 129
						 select new
						 {
							 Название_дисциплины = s.Name,
							 Часы_Лаб = s.LabHours,
							 Часы_Лекций = s.LectureHours
						 };
		Print(comment, queryLINQ1.ToList());
	}

	static void Main()
	{
		var builder = new ConfigurationBuilder();
		builder.SetBasePath(Directory.GetCurrentDirectory());
		builder.AddJsonFile("appsettings.json");
		var config = builder.Build();
		string? connectionString = config.GetConnectionString("DefaultConnection");

		var optionsBuilder = new DbContextOptionsBuilder<UniversityDepartmentContext>();
		var options = optionsBuilder
			.UseSqlServer(connectionString)
			.Options;

		using UniversityDepartmentContext db = new(options);
		Console.WriteLine("Будет выполнена выборка данных (нажмите любую клавишу) ============");
		Console.ReadKey();
		Select(db);

		Console.WriteLine("Будет выполнена вставка данных (нажмите любую клавишу) ============");
		Console.ReadKey();
		Insert(db);

		Console.WriteLine("Будет выполнено удаление данных (нажмите любую клавишу) ============");
		Console.ReadKey();
		Delete(db);

		Console.WriteLine("Будет выполнено обновление данных (нажмите любую клавишу) ============");
		Console.ReadKey();
		Update(db);
	}
}
