var builder = WebApplication.CreateBuilder(args);

var port = 7004;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder
    .Services
    .AddCors(options =>
    {
        options.AddPolicy("AllowAnyOrigin",
            builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAnyOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();

app.Run("http://0.0.0.0:" + port);