using Microsoft.AspNetCore.Identity;

namespace ArzonOL.Services.AuthService.Interfaces;

public interface IRegisterService
{
    Task<IdentityResult> RegisterAsync(string username, string password, string role);
    Task<bool> ValidateUsernameIsExist(string username);
    Task<IdentityResult> ChangePasswordAsync(Guid id, string oldPassword, string newPassword);
    Task<IdentityResult> ChangeUsernameAsync(Guid id, string newUsername);
}