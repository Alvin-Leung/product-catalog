using Microsoft.EntityFrameworkCore;
using Api.Data;

var builder = WebApplication.CreateBuilder(args);
var corsPolicyName = "_allowSpecificOrigins";

// --- Add CORS Policy ---
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicyName,
      policy =>
      {
          policy.WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod();
      });
});

builder.Services.AddControllers();

builder.Services.AddDbContext<ProductCatalogContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(corsPolicyName);

app.UseAuthorization();
app.MapControllers();
app.Run();