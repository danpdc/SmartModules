using System.Collections.Generic;
using Codewrinkles.MinimalApi.SmartModules.Extensions.WebApplicationExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => 
{
    c.SwaggerDoc("v1", new() { Title = builder.Environment.ApplicationName, Version="v1"});
    c.TagActionsBy(ta =>
    {
        return new List<string> { ta.ActionDescriptor.DisplayName! };
    });
});

builder.Services.AddSmartModules(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseSmartModules();

app.Run();