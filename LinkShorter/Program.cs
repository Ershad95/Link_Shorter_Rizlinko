

using LinkShorter.Models;
using LinkShorter.Repository;
using LinkShorter.Services.LinkService;
using LinkShorter.Services.StatisticalService;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
const string MyAllowSpecificOrigins = "AllowAnyOrigin";
builder.Services.AddResponseCompression(option =>
{
    option.EnableForHttps = true;
    option.Providers.Add<GzipCompressionProvider>();
    option.ExcludedMimeTypes.Concat(new[] { "image/svg+xml","application/atom+xml","application/atom+json"});
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder
                          .AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                      });
});


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<LinkShorterContext>();
builder.Services.AddTransient(typeof(IRepo<>), typeof(Repo<>));
builder.Services.AddTransient<ILinkService, LinkService>();
builder.Services.AddTransient<IStatisticalService, StatisticalService>();
// Register the Swagger generator, defining 1 or more Swagger documents
builder.Services.AddSwaggerGen();
//builder.Services.AddAntiforgery();

// Register the Swagger generator, defining 1 or more Swagger documents
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Rizlinko Documention",
        Description = "USe Rizlinko Api for create your short link",
        TermsOfService = new Uri("http://l2rl.ir"),
        Contact = new OpenApiContact
        {
            Name = "Ershad Raoufi",
            Email = "Ershad74Raoufi@gmail.com",
            Url = new Uri("http://l2rl.ir"),
        },
        //License = new OpenApiLicense
        //{
        //    Name = "Use under LICX",
        //    Url = new Uri("https://example.com/license"),
        //}
    });
    c.IgnoreObsoleteActions();
});

// Add detection services container and device resolver service.
builder.Services.AddDetection();

// Needed by Wangkanai Detection
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseResponseCompression();
app.UseHttpsRedirection();
app.UseStaticFiles();
// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
// specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RizLinko Rest Api");
});

app.UseDetection();
app.UseRouting();
app.UseCors();


app.UseAuthorization();



app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{url?}");


app.Run();
