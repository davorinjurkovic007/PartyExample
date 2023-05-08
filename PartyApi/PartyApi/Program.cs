using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//This enforces global authentication, similar to using the [Authorize] attribute on controllers and / or operations
builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder()
                       .RequireAuthenticatedUser()
                       //.RequireClaim("azp", builder.Configuration.GetSection("AzureAd:AccessControlList").Get<string[]>()!)
                       .RequireRole("regular", "vip")
                       .RequireAssertion(handler =>
                       {
                           var scopeClaim = handler.User.FindFirst("http://schemas.microsoft.com/identity/claims/scope");
                           var scopes = scopeClaim?.Value.Split(' ');
                           var hasScope = scopes?.Where(scope => scope == "access_as_user").Any() ?? false;
                           return hasScope;
                       })
                       .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration, "AzureAd");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
