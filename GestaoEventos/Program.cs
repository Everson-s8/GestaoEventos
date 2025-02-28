using Microsoft.EntityFrameworkCore;
using GestaoEventos.Data;

var builder = WebApplication.CreateBuilder(args);


// Configurar CORS para permitir o front-end Next.js (por exemplo, rodando em http://localhost:3000)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextJs", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
}); 

// Configurar o DbContext com PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            // N�o configure o ReferenceHandler para evitar altera��es no formato do JSON
            options.JsonSerializerOptions.WriteIndented = true;
        });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Aplicar as migra��es automaticamente (se houver migra��es pendentes)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

// Ativar o CORS com a pol�tica definida
app.UseCors("AllowNextJs");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
