using CourseManager.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// === הגדרת מחרוזת חיבור ל־SQL Server ===
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// === הרשמת בקרי API ===
builder.Services.AddControllers();

// === Swagger (תיעוד API) ===
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CourseManager API",
        Version = "v1"
    });
});

// === הגדרת CORS: אישור בקשות מ־localhost:4200 (אנגולר) ===
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDevClient", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// === הפעלת Swagger במצב פיתוח בלבד ===
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// === הפעלת CORS לפני שאר ה־middlewares ===
app.UseCors("AllowAngularDevClient");

// === HTTPS Redirect, Authorization וכו' ===
app.UseHttpsRedirection();
app.UseAuthorization();

// === מיפוי הבקרים (Controllers) ===
app.MapControllers();

// === הפעלת האפליקציה ===
app.Run();
