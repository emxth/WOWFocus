using WOWFocus.Application.Interfaces;
using WOWFocus.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var studentsFile = builder.Configuration["JsonStorage:StudentsFile"]!;
builder.Services.AddScoped<IStudentRepository>(_ => new JsonStudentRepository(studentsFile));

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapDefaultControllerRoute();
app.Run();