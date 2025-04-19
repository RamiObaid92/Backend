// ------------------------------------------------------------
//  Swagger example payloads for Auth‑ and Member‑endpoints
//  Place the file in  WebApi/Documentation  (or any folder you like)
//  Swashbuckle.AspNetCore.Filters must be installed and registered
//  in Program.cs:
//      builder.Services.AddSwaggerGen(o =>
//      {
//          ...
//          o.ExampleFilters();
//      });
//      builder.Services.AddSwaggerExamplesFromAssemblyOf<SwaggerExamples>();
// ------------------------------------------------------------
// Skapad med hjälp av AI

#nullable enable
using Domain.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace WebApi.Documentation;

public static class SwaggerExamples { /* empty marker type for assembly scanning */ }

// -----------------------------
// Auth‑endpoint examples
// -----------------------------

public sealed class SignUpDataExample : IExamplesProvider<SignUpForm>
{
    public SignUpForm GetExamples() => new()
    {
        FirstName = "Ada",
        LastName = "Lovelace",
        Email = "ada.lovelace@example.com",
        Password = "Pass!1234",
        PasswordConfirmation = "Pass!1234"
    };
}

public sealed class SignInDataExample : IExamplesProvider<SignInForm>
{
    public SignInForm GetExamples() => new()
    {
        Email = "ada.lovelace@example.com",
        Password = "Pass!1234",
        RememberMe = true
    };
}

// Vi har ingen ErrorMessage‑DTO i projektet, så vi skapar en enkel record
public record ErrorMessage(string Message);

public sealed class UserValidationErrorExample : IExamplesProvider<ErrorMessage>
{
    public ErrorMessage GetExamples() => new("Validation failed – Email is required and Password is Required");
}

public sealed class SignInErrorExample : IExamplesProvider<ErrorMessage>
{
    public ErrorMessage GetExamples() => new("Invalid email or password.");
}

// -----------------------------
// Member‑endpoint examples
// -----------------------------

public sealed class AddMemberDataExample : IExamplesProvider<AddMemberForm>
{
    public AddMemberForm GetExamples() => new()
    {
        FirstName = "Grace",
        LastName = "Hopper",
        Email = "grace.hopper@example.com",
        Phone = "+46 70‑123 45 67",
        Title = "Backend Developer",
        MemberRole = "User",
        StreetName = "Storgatan 1",
        PostalCode = "111 22",
        CityName = "Stockholm"
        // NewImageFile kan inte exemplifieras som ren JSON –
        // i Swagger visas den som file‑upload‑fält automatiskt.
    };
}

public sealed class EditMemberDataExample : IExamplesProvider<EditMemberForm>
{
    public EditMemberForm GetExamples() => new()
    {
        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
        ImageFileName = "grace.png",
        FirstName = "Grace",
        LastName = "Hopper",
        Email = "grace.hopper@example.com",
        Phone = "+46 70-123 45 67",
        Title = "Senior Backend Developer",
        MemberRole = "Admin",
        StreetName = "Storgatan 1",
        PostalCode = "111 22",
        CityName = "Stockholm"
        // NewImageFile kan lämnas null i example.
    };

}

