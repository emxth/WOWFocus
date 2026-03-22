using WOWFocus.Application.Interfaces;
using WOWFocus.Application.Services;
using WOWFocus.Infrastructure.Repositories;
using WOWFocus.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var studentsFile = builder.Configuration["JsonStorage:StudentsFile"]!;
builder.Services.AddScoped<IStudentRepository>(_ => new JsonStudentRepository(studentsFile));
builder.Services.AddScoped<IStudentService, StudentService>();

builder.Services.AddHostedService<ArchivedStudentCleanupService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapDefaultControllerRoute();
app.Run();