using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WOWFocus.Application.Services;
using WOWFocus.Web.ViewModels;

namespace WOWFocus.Web.Controllers;

[Authorize(Policy = "perm:admin.manage")]
public class AdminUsersController : Controller
{
    private readonly IdentityAdminService _admin;

    public AdminUsersController(IdentityAdminService admin) => _admin = admin;

    public async Task<IActionResult> Index()
        => View(await _admin.GetUsersAsync());

    public async Task<IActionResult> Create()
    {
        var roles = await _admin.GetRolesAsync();
        return View(new UserEditViewModel { AvailableRoles = roles.Select(r => r.Name).ToList() });
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserEditViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);
        await _admin.CreateUserAsync(vm.UserName, vm.Password!, vm.SelectedRoles);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var user = await _admin.GetUserAsync(id);
        if (user == null) return NotFound();

        var roles = await _admin.GetRolesAsync();

        var vm = new UserEditViewModel
        {
            Id = user.Id,
            UserName = user.UserName,
            SelectedRoles = user.Roles,
            AvailableRoles = roles.Select(r => r.Name).ToList()
        };
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UserEditViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);
        await _admin.UpdateUserAsync(vm.Id, vm.UserName, vm.Password, vm.SelectedRoles);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _admin.DeleteUserAsync(id);
        return RedirectToAction(nameof(Index));
    }
}