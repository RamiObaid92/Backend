using Data.Context;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));
builder.Services.AddIdentity<UserEntity, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();



var app = builder.Build();

app.MapOpenApi();

app.UseHttpsRedirection();

app.UseCors(x  => x.AllowAnyOrigin().AllowCredentials().AllowAnyHeader());
app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
