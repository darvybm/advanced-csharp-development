using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Config;
using TaskManagementAPI.Models.Data;
using TaskManagementAPI.Repositories;
using TaskManagementAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// SQL Server config
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Servicios
builder.Services.AddScoped<TaskRepository>();
builder.Services.AddScoped<TaskService>();
builder.Services.AddSingleton<ReactiveTaskQueue>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins("http://127.0.0.1:5500") // la URL exacta de Live Server
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // importante si usas SignalR
    });
});

var app = builder.Build();

app.UseMiddleware<ErrorHandler>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(); // antes de MapHub y MapControllers

app.MapHub<TaskHub>("/taskhub");
app.MapHub<ChatHub>("/chatHub");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
