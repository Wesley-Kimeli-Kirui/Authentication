using AuthenAuthor.Data;
using AuthenAuthor.Services;
using AuthenAuthor.Services.IService;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection")));

// Service for Dependency Injection
builder.Services.AddScoped<IUserInterface, UserService>();
// Auto Mapper Configurations
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// ApplyMigration();
app.Run();
//Migration
// void ApplyMigration()
// {
//     using (var scope = app.Services.CreateScope())
//     {
//         var _db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
//         if (_db.Database.GetPendingMigrations().Count() > 0)
//         {
//             _db.Database.Migrate();
//         }
//     }
// }
