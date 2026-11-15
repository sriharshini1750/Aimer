using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Aimer.Infrastructure;
using Aimer.Core;
using Aimer.Console;
using Aimer.Console.Actions;
using Aimer.Console.IActions;
using Microsoft.Extensions.Configuration;

var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddUserSecrets<Program>();
builder.Services.AddInfrastructure(builder.Configuration).AddCore();
builder.Services.AddTransient<Aimer.Console.App>();
builder.Services.AddTransient<IAction,UploadPdfAction>();
builder.Services.AddTransient<IAction,IngestPdfAction>();



var app = builder.Build();
await app.Services.GetRequiredService<Aimer.Console.App>().RunAsync();