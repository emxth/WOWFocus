using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WOWFocus.Application.Services;
using WOWFocus.Web.ViewModels;

namespace WOWFocus.Web.Controllers;

[Authorize(Policy = "perm:admin.manage")]
public class AdminRolesController : Controller
{
    private readonly IdentityAdminService _admin;
    private readonly IConfiguration _config;

    public AdminRolesController(IdentityAdminService admin, IConfiguration config)
    {
        _admin = admin;
        _config = config;
    }

    public async Task<IActionResult> Index()
        => View(await _admin.GetRolesAsync());

    public IActionResult Create()
    {
        var permissions = _config.GetSection("Permissions").Get<List<string>>() ?? new();
        return View(new RoleEditViewModel { AvailablePermissions = permissions });
    }

    [HttpPost]
    public async Task<IActionResult> Create(RoleEditViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);
        await _admin.CreateRoleAsync(vm.Name, vm.SelectedPermissions);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(string id)
    {
        var role = await _admin.GetRoleAsync(id);
        if (role == null) return NotFound();

        var permissions = _config.GetSection("Permissions").Get<List<string>>() ?? new();

        var vm = new RoleEditViewModel
        {
            Name = role.Name,
            SelectedPermissions = role.Permissions,
            AvailablePermissions = permissions
        };
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(RoleEditViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);
        await _admin.UpdateRoleAsync(vm.Name, vm.SelectedPermissions);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        await _admin.DeleteRoleAsync(id);
        return RedirectToAction(nameof(Index));
    }
}