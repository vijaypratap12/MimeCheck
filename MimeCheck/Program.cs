using MimeCheck.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() 
    { 
        Title = "MimeCheck API", 
        Version = "v1",
        Description = "A comprehensive MIME type validation library for .NET. Detect and validate file types using magic byte signatures."
    });
});

// Add MimeCheck validation services
builder.Services.AddMimeValidation(options =>
{
    // Configure global validation options
    options.DenyExecutables(); // Deny executable files by default
    options.WithMaxSizeMB(50); // 50MB max file size
    options.RequireKnownType = false; // Allow unknown types by default
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Optional: Use MimeCheck validation middleware for automatic validation
// app.UseMimeValidation();

app.MapControllers();

app.Run();
