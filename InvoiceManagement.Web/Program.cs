using FluentValidation;
using FluentValidation.AspNetCore;
using InvoiceManagement.Application.DTOs;
using InvoiceManagement.Application.Validators;
using InvoiceManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorPages();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddControllersWithViews();




// Configure infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// Configure application layer
builder.Services.AddApplication();

// Configure API HTTP client
builder.Services.AddHttpClient("InvoiceAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ??
        throw new InvalidOperationException("API base URL not configured"));
});
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddPageRoute("/Invoices/Index", "/");
});
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("en-US");
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<InvoiceDtoValidator>();




builder.Services.AddScoped<IValidator<InvoiceDto>, InvoiceDtoValidator>();
var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=600");
    }
});
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();


// Apply database migrations
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

app.Run();