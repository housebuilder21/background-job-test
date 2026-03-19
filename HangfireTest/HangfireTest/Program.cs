using ExampleServiceLibrary;
using Hangfire;
using Hangfire.Storage.SQLite;
using HangfireTest.Components;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();



// Add Example Service to showcase dpendency injection with Hangfire.
builder.Services.AddExampleService(
    builder.Configuration.GetValue<bool>("ExampleServiceConfiguration:TestBoolean"),
    builder.Configuration.GetValue<string>("ExampleServiceConfiguration:DefaultMessage"));

// Add Hangfire + Configuration (connection string set from secrets.json)
var connectionString = builder.Configuration.GetConnectionString("Hangfire")
    ?? throw new InvalidOperationException("Connection string 'Hangfire' not found.");
builder.Services.AddHangfire(configuration => configuration
    //.UseSqlServerStorage(connectionString));
    .UseSQLiteStorage("hangfire.db")); // SQLite can be used 

// Add the processing server as IHostedService
builder.Services.AddHangfireServer();



var app = builder.Build();

// Add Hangfire Dashoard (visit "localhost:<port number>/hangfire)
// ex. http://localhost:5206/hangfire (this page is only accessible locally)
app.UseHangfireDashboard();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
