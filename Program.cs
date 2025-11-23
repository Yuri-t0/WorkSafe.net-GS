using Microsoft.EntityFrameworkCore;
using WorkSafe.Api.Infrastructure.Data;
using WorkSafe.Api.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

 builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Dados inválidos enviados."
            };
            return new BadRequestObjectResult(problemDetails);
        };
    });

 builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

 builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

 builder.Services.AddPersistence(builder.Configuration);

var app = builder.Build();

 app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Status = 500,
            Title = "Erro interno no servidor.",
            Detail = "Tente novamente mais tarde."
        };

        await context.Response.WriteAsJsonAsync(problem);
    });
});

 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();