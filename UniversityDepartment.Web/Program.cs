using System.Text.Json;
using Contracts;
using Contracts.Services;
using Microsoft.AspNetCore.HttpOverrides;
using UniversityDepartment.Domain.Models;
using UniversityDepartment.Web.Extensions;

namespace UniversityDepartment.Web;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		ConfigureServices(builder.Services, builder.Configuration);

		var app = builder.Build();
		if (app.Environment.IsProduction())
			app.UseHsts();

		ConfigureApp(app);

		app.Map("/info", Info);
		app.Map("/courses", Courses);
		app.Map("/faculties", Faculties);
		app.Map("/specialties", Specialties);
		app.Map("/subjects", Subjects);
		app.Map("/teachers", Teachers);
		app.Map("/searchteacher", SearchFormTeacher);
		app.Map("/searchteachersession", SearchFormTeacherSession);

		app.Run(async (context) =>
		{
			ICourseService cachedCourseService = context.RequestServices.GetService<ICourseService>();
			cachedCourseService?.AddCourses("Courses20");

			IFacultyService cachedFacultyService = context.RequestServices.GetService<IFacultyService>();
			cachedFacultyService?.AddFaculties("Faculties20");

			ITeacherService cachedTeacherService = context.RequestServices.GetService<ITeacherService>();
			cachedTeacherService?.AddTeachers("Teachers20");

			string HtmlString = "<HTML><HEAD><TITLE>Главная</TITLE></HEAD>" +
			"<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
			"<BODY><H1>Главная</H1>";
			HtmlString += "<H2>Данные записаны в кэш сервера</H2>";
			HtmlString += "<BR><A href='/'>Главная</A>";
			HtmlString += "<BR><A href='/courses'>Курсы</A>";
			HtmlString += "<BR><A href='/faculties'>Факультеты</A>";
			HtmlString += "<BR><A href='/specialties'>Специальности</A>";
			HtmlString += "<BR><A href='/subjects'>Предметы</A>";
			HtmlString += "<BR><A href='/teachers'>Преподаватели</A>";
			HtmlString += "<BR><A href='/searchteacher'>Поиск прподавателей (Cookies)</A>";
			HtmlString += "<BR><A href='/searchteachersession'>Поиск прподавателей (Session)</A>";
			HtmlString += "<BR><A href='/info'>Информация о клиенте</A>";
			HtmlString += "</BODY></HTML>";

			await context.Response.WriteAsync(HtmlString);
		});

		app.Run();
	}

	public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
	{
		services.ConfigureCors();

		services.ConfigureSqlContext(configuration);

		services.AddMemoryCache();
		services.AddDistributedMemoryCache();
		services.AddSession();

		services.ConfigureRepositoryManager();
		services.ConfigureServiceManager();
	}

	public static void ConfigureApp(IApplicationBuilder app)
	{
		app.UseHttpsRedirection();

		app.UseForwardedHeaders(new ForwardedHeadersOptions
		{
			ForwardedHeaders = ForwardedHeaders.All
		});

		app.UseCors("CorsPolicy");
		app.UseSession();
		app.UseCookiePolicy();
	}

	private static void Info(IApplicationBuilder app)
	{
		app.Run(async (context) =>
		{
			string strResponse = "<HTML><HEAD><TITLE>Информация</TITLE></HEAD>" +
			"<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
			"<BODY><H1>Информация:</H1>";
			strResponse += "<BR> Сервер: " + context.Request.Host;
			strResponse += "<BR> Путь: " + context.Request.PathBase;
			strResponse += "<BR> Протокол: " + context.Request.Protocol;
			strResponse += "<BR><A href='/'>Главная</A></BODY></HTML>";
			await context.Response.WriteAsync(strResponse);
		});
	}

	private static void Faculties(IApplicationBuilder app)
	{
		app.Run(async context =>
		{
			IFacultyService? cachedFacultyService = context.RequestServices.GetService<IServiceManager>()?.FacultyService;
			IEnumerable<Faculty>? faculties = cachedFacultyService?.GetFaculties();

			string HtmlString = "<HTML><HEAD>" +
				"<TITLE>Факультеты</TITLE></HEAD>" +
				"<META http-equiv='Content-Type' content='text/html; charset=utf-8' />" +
				"<BODY><H1>Список факультетов</H1>" +
				"<TABLE BORDER=1 cellspacing=0>";
			HtmlString += "<TH>";
			HtmlString += "<TD>Название</TD>";
			HtmlString += "</TH>";

			foreach (Faculty faculty in faculties)
			{
				HtmlString += "<TR>";
				HtmlString += "<TD>" + faculty.FacultyId + "</TD>";
				HtmlString += "<TD>" + faculty.Name + "</TD>";
				HtmlString += "</TR>";
			}
			HtmlString += "</table></BODY></HTML>";

			await context.Response.WriteAsync(HtmlString);
		});
	}

	private static void Specialties(IApplicationBuilder app)
	{
		app.Run(async context =>
		{
			ISpecialtyService? cachedSpecialtyService = context.RequestServices.GetService<IServiceManager>()?.SpecialtyService;
			IEnumerable<Specialty>? specialties = cachedSpecialtyService?.GetSpecialties();

			string HtmlString = "<HTML><HEAD>" +
				"<TITLE>Специальности</TITLE></HEAD>" +
				"<META http-equiv='Content-Type' content='text/html; charset=utf-8' />" +
				"<BODY><H1>Список специальностей</H1>" +
				"<TABLE BORDER=1 cellspacing=0>";
			HtmlString += "<TH>";
			HtmlString += "<TD>Название</TD>";
			HtmlString += "<TD>Отдел</TD>";
			HtmlString += "</TH>";

			foreach (Specialty specialty in specialties)
			{
				HtmlString += "<TR>";
				HtmlString += "<TD>" + specialty.SpecialtyId + "</TD>";
				HtmlString += "<TD>" + specialty.Name + "</TD>";
				HtmlString += "<TD>" + specialty.Department.Name + "</TD>";
				HtmlString += "</TR>";
			}
			HtmlString += "</table></BODY></HTML>";

			await context.Response.WriteAsync(HtmlString);
		});
	}

	private static void Subjects(IApplicationBuilder app)
	{
		app.Run(async context =>
		{
			ISubjectService? cachedSubjectService = context.RequestServices.GetService<IServiceManager>()?.SubjectService;
			IEnumerable<Subject>? subjects = cachedSubjectService?.GetSubjects();

			string HtmlString = "<HTML><HEAD>" +
				"<TITLE>Предметы</TITLE></HEAD>" +
				"<META http-equiv='Content-Type' content='text/html; charset=utf-8' />" +
				"<BODY><H1>Список предметов</H1>" +
				"<TABLE BORDER=1 cellspacing=0>";
			HtmlString += "<TH>";
			HtmlString += "<TD>Название</TD>";
			HtmlString += "<TD>Часы лекций</TD>";
			HtmlString += "<TD>Практические часы</TD>";
			HtmlString += "<TD>Лабораторные часы</TD>";
			HtmlString += "<TD>Тип отчетности</TD>";
			HtmlString += "</TH>";

			foreach (Subject subject in subjects)
			{
				HtmlString += "<TR>";
				HtmlString += "<TD>" + subject.SubjectId + "</TD>";
				HtmlString += "<TD>" + subject.Name + "</TD>";
				HtmlString += "<TD>" + subject.LectureHours + "</TD>";
				HtmlString += "<TD>" + subject.PracticalHours + "</TD>";
				HtmlString += "<TD>" + subject.LabHours + "</TD>";
				HtmlString += "<TD>" + subject.ReportingType + "</TD>";
				HtmlString += "</TR>";
			}
			HtmlString += "</table></BODY></HTML>";

			await context.Response.WriteAsync(HtmlString);
		});
	}

	private static void Teachers(IApplicationBuilder app)
	{
		app.Run(async context =>
		{
			ITeacherService? cachedTeacherService = context.RequestServices.GetService<IServiceManager>()?.TeacherService;
			IEnumerable<Teacher>? teachers = cachedTeacherService?.GetTeachers();

			string HtmlString = "<HTML><HEAD>" +
				"<TITLE>Преподаватели</TITLE></HEAD>" +
				"<META http-equiv='Content-Type' content='text/html; charset=utf-8' />" +
				"<BODY><H1>Список преподавателей</H1>" +
				"<TABLE BORDER=1 cellspacing=0>";
			HtmlString += "<TH>";
			HtmlString += "<TD>Имя</TD>";
			HtmlString += "<TD>Фамилия</TD>";
			HtmlString += "<TD>Отчество</TD>";
			HtmlString += "<TD>Должность</TD>";
			HtmlString += "<TD>Возраст</TD>";
			HtmlString += "</TH>";

			foreach (Teacher teacher in teachers)
			{
				HtmlString += "<TR>";
				HtmlString += "<TD>" + teacher.TeacherId + "</TD>";
				HtmlString += "<TD>" + teacher.Name + "</TD>";
				HtmlString += "<TD>" + teacher.Surname + "</TD>";
				HtmlString += "<TD>" + teacher.Midname + "</TD>";
				HtmlString += "<TD>" + teacher.Position + "</TD>";
				HtmlString += "<TD>" + teacher.Age + "</TD>";
				HtmlString += "</TR>";
			}
			HtmlString += "</table></BODY></HTML>";

			await context.Response.WriteAsync(HtmlString);
		});
	}

	private static void Courses(IApplicationBuilder app)
	{
		app.Run(async context =>
		{
			ICourseService? cachedCourseService = context.RequestServices.GetService<IServiceManager>()?.CourseService;
			IEnumerable<Course>? courses = cachedCourseService?.GetCourses();

			string HtmlString = "<HTML><HEAD>" +
				"<TITLE>Курсы</TITLE></HEAD>" +
				"<META http-equiv='Content-Type' content='text/html; charset=utf-8' />" +
				"<BODY><H1>Список курсов</H1>" +
				"<TABLE BORDER=1 cellspacing=0>";
			HtmlString += "<TH>";
			HtmlString += "<TD>Номер курса</TD>";
			HtmlString += "<TD>Номер семестра</TD>";
			HtmlString += "<TD>Специальность</TD>";
			HtmlString += "</TH>";

			foreach (Course course in courses)
			{
				HtmlString += "<TR>";
				HtmlString += "<TD>" + course.CourseId + "</TD>";
				HtmlString += "<TD>" + course.CourseNumber + "</TD>";
				HtmlString += "<TD>" + course.SemesterNumber + "</TD>";
				HtmlString += "<TD>" + course.Specialty.Name + "</TD>"; // Предполагается, что Specialty загружена
				HtmlString += "</TR>";
			}
			HtmlString += "</table></BODY></HTML>";

			await context.Response.WriteAsync(HtmlString);
		});
	}

	private static void Departments(IApplicationBuilder app)
	{
		app.Run(async context =>
		{
			IDepartmentService? cachedDepartmentService = context.RequestServices.GetService<IServiceManager>()?.DepartmentService;
			IEnumerable<Department>? departments = cachedDepartmentService.GetDepartments();

			string HtmlString = "<HTML><HEAD>" +
				"<TITLE>Отделы</TITLE></HEAD>" +
				"<META http-equiv='Content-Type' content='text/html; charset=utf-8' />" +
				"<BODY><H1>Список отделов</H1>" +
				"<TABLE BORDER=1 cellspacing=0>";
			HtmlString += "<TH>";
			HtmlString += "<TD>Название</TD>";
			HtmlString += "<TD>Факультет</TD>";
			HtmlString += "<TD>Выпускающий</TD>";
			HtmlString += "</TH>";

			foreach (Department department in departments)
			{
				HtmlString += "<TR>";
				HtmlString += "<TD>" + department.DepartmentId + "</TD>";
				HtmlString += "<TD>" + department.Name + "</TD>";
				HtmlString += "<TD>" + department.Faculty.Name + "</TD>";
				HtmlString += "<TD>" + department.IsGraduating + "</TD>";
				HtmlString += "</TR>";
			}
			HtmlString += "</table></BODY></HTML>";

			await context.Response.WriteAsync(HtmlString);
		});
	}

	private static void SearchFormTeacher(IApplicationBuilder app) =>
	app.Run(HandleSearchFormTeacher);

	private static async Task HandleSearchFormTeacher(HttpContext context)
	{
		var userJson = context.Request.Cookies["searchData"];
		var searchData = string.IsNullOrEmpty(userJson) ? new SearchData() : JsonSerializer.Deserialize<SearchData>(userJson);

		ArgumentNullException.ThrowIfNull(searchData);

		if (context.Request.Query.ContainsKey("name"))
		{
			searchData.Name = context.Request.Query["name"];
		}
		if (context.Request.Query.ContainsKey("surname"))
		{
			searchData.Surname = context.Request.Query["surname"];
		}
		if (context.Request.Query.ContainsKey("position"))
		{
			searchData.Position = context.Request.Query["position"];
		}

		ITeacherService? cachedTeacherService = context.RequestServices.GetService<IServiceManager>()?.TeacherService;

		cachedTeacherService?.AddTeachersByCondition(
			"Teacher20",
			x => x.Name.Contains(searchData.Name) &&
				  x.Surname.Contains(searchData.Surname) &&
				  (string.IsNullOrEmpty(searchData.Position) || x.Position == searchData.Position));
		var teachers = cachedTeacherService?.GetTeachers("Teacher20");

		context.Response.Cookies.Append("searchData", JsonSerializer.Serialize(searchData), new CookieOptions
		{
			Expires = DateTimeOffset.UtcNow.AddDays(30)
		});

		string tableHtml = "<TABLE BORDER=1 cellspacing=0>";
		tableHtml += "<TH><TD>Имя</TD><TD>Фамилия</TD><TD>Должность</TD><TD>Возраст</TD></TH>";

		foreach (Teacher teacher in teachers ?? Enumerable.Empty<Teacher>())
		{
			tableHtml += "<TR>";
			tableHtml += $"<TD>{teacher.TeacherId}</TD>";
			tableHtml += $"<TD>{teacher.Name}</TD>";
			tableHtml += $"<TD>{teacher.Surname}</TD>";
			tableHtml += $"<TD>{teacher.Position}</TD>";
			tableHtml += $"<TD>{teacher.Age}</TD>";
			tableHtml += "</TR>";
		}
		tableHtml += "</TABLE>";

		var positions = new[] { "Преподаватель", "Ассистент", "Лектор", "Доцент" };

		string formHtml = "<HTML><HEAD><TITLE>Форма поиска преподавателей</TITLE></HEAD>" +
			"<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
			"<BODY>" +
			"<FORM method='get' action='/searchteacher'>" +
			"Поиск по имени:<BR><INPUT type='text' name='name' value='" + searchData.Name + "'>" +
			"<BR>Поиск по фамилии:<BR><INPUT type='text' name='surname' value='" + searchData.Surname + "'>" +
			"<BR>Выберите должность:<BR><SELECT name='position'>" +
			"<OPTION value=''>Все</OPTION>";

		foreach (var position in positions)
		{
			formHtml += $"<OPTION value='{position}'" + (searchData.Position == position ? " selected" : "") + $">{position}</OPTION>";
		}

		formHtml += "</SELECT><BR><BR><INPUT type='submit' value='Искать'></FORM>" +
			"<BR><A href='/'>Главная</A>" +
			"<H2>Результаты поиска:</H2>" +
			tableHtml +
			"</BODY></HTML>";

		await context.Response.WriteAsync(formHtml);
	}

	private static void SearchFormTeacherSession(IApplicationBuilder app) =>
	app.Run(HandleSearchFormTeacherSession);

	private static async Task HandleSearchFormTeacherSession(HttpContext context)
	{
		var userJson = context.Session.GetString("searchData");
		var searchData = string.IsNullOrEmpty(userJson) ? new SearchData() : JsonSerializer.Deserialize<SearchData>(userJson);

		ArgumentNullException.ThrowIfNull(searchData);

		if (context.Request.Query.ContainsKey("name"))
		{
			searchData.Name = context.Request.Query["name"];
		}
		if (context.Request.Query.ContainsKey("surname"))
		{
			searchData.Surname = context.Request.Query["surname"];
		}
		if (context.Request.Query.ContainsKey("position"))
		{
			searchData.Position = context.Request.Query["position"];
		}

		ITeacherService? cachedTeacherService = context.RequestServices.GetService<IServiceManager>()?.TeacherService;

		cachedTeacherService?.AddTeachersByCondition(
			"Teacher20",
			x => x.Name.Contains(searchData.Name) &&
				  x.Surname.Contains(searchData.Surname) &&
				  (string.IsNullOrEmpty(searchData.Position) || x.Position == searchData.Position));

		var teachers = cachedTeacherService?.GetTeachers("Teacher20");

		context.Session.SetString("searchData", JsonSerializer.Serialize(searchData));

		string tableHtml = "<TABLE BORDER=1 cellspacing=0>";
		tableHtml += "<TH><TD>Имя</TD><TD>Фамилия</TD><TD>Должность</TD><TD>Возраст</TD></TH>";

		foreach (Teacher teacher in teachers ?? Enumerable.Empty<Teacher>())
		{
			tableHtml += "<TR>";
			tableHtml += $"<TD>{teacher.TeacherId}</TD>";
			tableHtml += $"<TD>{teacher.Name}</TD>";
			tableHtml += $"<TD>{teacher.Surname}</TD>";
			tableHtml += $"<TD>{teacher.Position}</TD>";
			tableHtml += $"<TD>{teacher.Age}</TD>";
			tableHtml += "</TR>";
		}
		tableHtml += "</TABLE>";

		var positions = new[] { "Преподаватель", "Ассистент", "Лектор", "Доцент" };
		string formHtml = "<HTML><HEAD><TITLE>Форма поиска преподавателей</TITLE></HEAD>" +
			"<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
			"<BODY>" +
			"<FORM method='get' action='/searchteacher'>" +
			"Поиск по имени:<BR><INPUT type='text' name='name' value='" + searchData.Name + "'>" +
			"<BR>Поиск по фамилии:<BR><INPUT type='text' name='surname' value='" + searchData.Surname + "'>" +
			"<BR>Выберите должность:<BR><SELECT name='position'>" +
			"<OPTION value=''>Все</OPTION>";

		foreach (var position in positions)
		{
			formHtml += $"<OPTION value='{position}'" + (searchData.Position == position ? " selected" : "") + $">{position}</OPTION>";
		}

		formHtml += "</SELECT><BR><BR><INPUT type='submit' value='Искать'></FORM>" +
			"<BR><A href='/'>Главная</A>" +
			"<H2>Результаты поиска:</H2>" +
			tableHtml +
			"</BODY></HTML>";

		await context.Response.WriteAsync(formHtml);
	}
}

public class SearchData
{
	public string Name { get; set; } = string.Empty;
	public string Surname { get; set; } = string.Empty;
	public string Position { get; set; } = string.Empty;
}
