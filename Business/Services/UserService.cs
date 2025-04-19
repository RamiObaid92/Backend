using Business.Factories;
using Data.Entities;
using Domain.DTOs;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public interface IUserService
{
    Task<AuthResult?> SignInAsync(SignInForm formData);
    Task SignOutAsync();
    Task<UserModel?> SignUpAsync(SignUpForm formData);
}

public class UserService(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, ITokenService tokenService) : IUserService
{
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<UserModel?> SignUpAsync(SignUpForm formData)
    {
        var userExists = await _userManager.FindByEmailAsync(formData.Email);
        if (userExists is not null) return null;

        var entity = UserFactory.ToEntity(formData);
        var result = await _userManager.CreateAsync(entity, formData.Password);
        if (!result.Succeeded) return null;

        var totalEntities = await _userManager.Users.CountAsync();
        var role = totalEntities == 1 ? "Admin" : "User";
        await _userManager.AddToRoleAsync(entity, role);

        return UserFactory.ToModel(entity);
    }

    // Modifierade SignInAsync för att lägga till token till cookie vid SignIn. Tog hjälp av AI med hur man gör det.
    public async Task<AuthResult?> SignInAsync(SignInForm formData)
    {
        var result = await _signInManager.PasswordSignInAsync(formData.Email, formData.Password, isPersistent: formData.RememberMe, lockoutOnFailure: false);
        if (!result.Succeeded) return null;

        var user = await _userManager.FindByEmailAsync(formData.Email);
        var roles = await _userManager.GetRolesAsync(user!);

        var token = _tokenService.GenerateToken(user!, roles);
        var model = UserFactory.ToModel(user);

        return new AuthResult(model, token);
    }

    public async Task SignOutAsync()
        => await _signInManager.SignOutAsync();
}
