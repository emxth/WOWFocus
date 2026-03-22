using WOWFocus.Domain.Identity;

namespace WOWFocus.Application.Interfaces;

public interface IIdentityRepository
{
    Task<IdentityStore> LoadAsync();
    Task SaveAsync(IdentityStore store);
}