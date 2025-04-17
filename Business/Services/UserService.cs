using Business.Mappers;
using Data.Entities;
using Data.Repositories;
using Domain.DTOs;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;


// Gett the user repository, user manager, and sign-in manager from the constructor
// This class is responsible for user-related operations
// It uses the user repository to interact with the database and the user manager and sign-in manager for authentication
// The methods needed for this class are the following: SignUpAsync, SignInAsync, SignOutAsync, GetUserByIdAsync, GetUserByEmailAsync, UpdateUserAsync, DeleteUserAsync
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
    {
        await _signInManager.SignOutAsync();
    }
}
