using Domain.Models;

namespace Domain.DTOs;

public record AuthResult(UserModel User, string Token);
