using Business.Factories;
using Data.Entities;
using Domain.DTOs;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public class UserService(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager)
{
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;

    public async Task<UserModel?> SignUpAsync(SignUpForm formData)
    {
        var userExists = await _userManager.FindByEmailAsync(formData.Email);
        if (userExists is not null) return null;

        var entity = UserFactory.ToEntity(formData);
        var result = await _userManager.CreateAsync(entity, formData.Password);
        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(entity, isPersistent: false);
            return UserFactory.ToModel(entity);
        }
        return null;
    }

    public async Task<UserModel?> SignInAsync(SignInForm formData)
    {
        var result = await _signInManager.PasswordSignInAsync(formData.Email, formData.Password, isPersistent: formData.RememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(formData.Email);
            return UserFactory.ToModel(user);
        }
        return null;
    }

    public async Task SignOutAsync()
        => await _signInManager.SignOutAsync();
}
