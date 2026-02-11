using EPM_Blogger.API.Middlewares;
using EPM_Blogger.Application.Interfaces;
using EPM_Blogger.Application.Services;
using EPM_Blogger.Domain.Interfaces;
using EPM_Blogger.Infrastructure.Persistence;
using EPM_Blogger.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// CORS Middleware

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularClient",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // Angular dev server
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BloggingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BlogDb")));
// Dependency Injections 

// Repositories DI
builder.Services.AddScoped<IAuthRepository,AuthRepository>();
builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IPostRepository,PostRepository>();
builder.Services.AddScoped<ILikeRepository,LikeRepository>();

// Services DI
builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IPostService,PostService>();
builder.Services.AddScoped<ILikeService,LikeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Use CORS
app.UseCors("AllowAngularClient");

// Register Middleware
app.UseJwtValidationMiddleware();
//app.UseMiddleware<TokenBucketMiddleware>();
//app.UseLeakyBucketMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope()) {
    var db = scope.ServiceProvider.GetRequiredService<BloggingDbContext>();
    db.Database.Migrate();
}

app.Run();
