using HtmlToPdf.Common.Broker.DI;
using HtmlToPdf.Common.Configuration;
using HtmlToPdf.ConversionApi.Broker.Producing.DI;
using HtmlToPdf.ConversionApi.Data.AppDatabase.DI;
using HtmlToPdf.ConversionApi.WebApi.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCommonBrokerServices();
builder.Services.AddBrokerProducingServices();
builder.Services.AddAppDatabaseServices(builder.Configuration);

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    var corsConfiguration = builder.Configuration.GetSection<CorsConfiguration>();

    options.AddPolicy(name: myAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins(corsConfiguration.AllowedOrigins)
                .SetIsOriginAllowedToAllowWildcardSubdomains();
        });

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseCors(myAllowSpecificOrigins);

app.MapControllers();

app.Run();