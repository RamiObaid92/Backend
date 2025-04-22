using Business.Factories;
using Data.Entities;
using Domain.DTOs;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Business.Services;

public interface IUserService
{
    Task<AuthResult?> SignInAsync(SignInForm formData);
    Task SignOutAsync();
    Task<UserModel?> SignUpAsync(SignUpForm formData);
}

public class UserService(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, ITokenService tokenService, IConfiguration configuration) : IUserService
{
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IConfiguration _configuration = configuration;

    public async Task<UserModel?> SignUpAsync(SignUpForm formData)
    {
        var userExists = await _userManager.FindByEmailAsync(formData.Email);
        Console.WriteLine($"Searching for: {formData.Email}");
        Console.WriteLine($"Found user: {(userExists != null ? userExists.Email : "null")}");
        if (userExists is not null) return null;

        var entity = UserFactory.ToEntity(formData);
        var result = await _userManager.CreateAsync(entity, formData.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                Console.WriteLine($"CreateAsync Error: {error.Code} - {error.Description}");

            return null;
        }

        var totalEntities = await _userManager.Users.CountAsync();
        var role = totalEntities == 1 ? "Admin" : "User";
        await _userManager.AddToRoleAsync(entity, role);

        return UserFactory.ToModel(entity);
    }

    // Modifierade SignInAsync för att lägga till token till cookie vid SignIn. Tog hjälp av AI med hur man gör det.
    public async Task<AuthResult?> SignInAsync(SignInForm formData)
    {
        var result = await _signInManager.PasswordSignInAsync(formData.Email, formData.Password, formData.RememberMe, false);
        if (!result.Succeeded) return null;

        var user = await _userManager.FindByEmailAsync(formData.Email);
        var roles = await _userManager.GetRolesAsync(user!);

        var token = _tokenService.GenerateToken(user!, roles);
        var apiKeys = new ApiKeysDto(
            AdminKey: _configuration["SecretKeys:AdminKey"]!,
            UserKey: _configuration["SecretKeys:UserKey"]!
        );

        var model = UserFactory.ToModel(user);
        model.Roles = roles.ToList();

        return new AuthResult(model, token, apiKeys);
    }

    public async Task SignOutAsync()
        => await _signInManager.SignOutAsync();
}
