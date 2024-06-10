using Donut.MemberShip.Authentication.AssemblyReference;
using Donut.MemberShip.Authentication.EndPoints.Scanner;
using Donut.MemberShip.Core.AssemblyReference;
using Donut.SharedKernel.DependencyInjection.Scanner;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

Assembly[] assemblies = [typeof(AuthenticationAssemblyReference).Assembly, typeof(MemberShipAssemblyReference).Assembly];

builder.Services.RegisterServices(builder.Configuration, assemblies);
// Add services to the container.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.MapIdentityApi<ApplicationUser>();
app.UseAuthorization();
app.MapAuthenticationEndPoints();
app.Run();
