using CatServiceAPI.Data;
using CatServiceAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "swagger";
    config.Title = "Cat Service API";
    config.Description = "API for fetching and storing cat data";
    config.Version = "v1";
    config.UseControllerSummaryAsTagDescription = true;
});
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<CatService>();
builder.Services.AddHttpClient<CatService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["CatApi:BaseUrl"]);
    client.DefaultRequestHeaders.Add("x-api-key", builder.Configuration["CatApi:ApiKey"]);
});


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi(options =>
    {
        options.DocumentName = "swagger";
        options.Path = "/swagger/v1/swagger.json";
    });
    app.UseSwaggerUi(swaggerUiOptions =>
    {
        swaggerUiOptions.Path = "/swagger";
        swaggerUiOptions.DocumentTitle = "Cat Service API";
        swaggerUiOptions.DocumentPath = "/swagger/v1/swagger.json";
    });
}


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
