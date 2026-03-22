using Microsoft.AspNetCore.Mvc;
using WOWFocus.Application.Interfaces;
using WOWFocus.Application.Models;
using WOWFocus.Web.ViewModels;

namespace WOWFocus.Web.Controllers;

public class StudentsController : Controller
{
    private readonly IStudentService _service;

    public StudentsController(IStudentService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index()
    {
        var students = await _service.GetAllAsync();
        return View(students);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var student = await _service.GetByIdAsync(id);
        if (student == null) return NotFound();
        return View(student);
    }

    public IActionResult Create()
    {
        return View(new StudentCreateViewModel
        {
            DateOfBirth = DateTime.UtcNow.AddYears(-15)
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(StudentCreateViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var created = await _service.CreateAsync(new StudentCreateRequest
        {
            FullName = vm.FullName,
            DateOfBirth = vm.DateOfBirth,
            Gender = vm.Gender
        });

        return RedirectToAction(nameof(Details), new { id = created.Id });
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var student = await _service.GetByIdAsync(id);
        if (student == null) return NotFound();

        var vm = new StudentEditViewModel
        {
            Id = student.Id,
            StudentId = student.StudentId,
            FullName = student.FullName,
            DateOfBirth = student.DateOfBirth,
            Gender = student.Gender
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(StudentEditViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var updated = await _service.UpdateAsync(new StudentUpdateRequest
        {
            Id = vm.Id,
            FullName = vm.FullName,
            DateOfBirth = vm.DateOfBirth,
            Gender = vm.Gender
        });

        if (!updated) return NotFound();
        return RedirectToAction(nameof(Details), new { id = vm.Id });
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var student = await _service.GetByIdAsync(id);
        if (student == null) return NotFound();
        return View(student);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _service.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}