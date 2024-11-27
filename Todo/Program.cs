using Microsoft.OpenApi.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Todo.Model;
using Todo.Core;

var builder = WebApplication.CreateBuilder(args);

Dictionary<string, Item> Items = new Dictionary<string, Item>();
var itemList = Enumerable.Range(1, 5).Select(index => new Item($"Item {index}", $"Description for item {index}"));
foreach(var item in itemList){
    Items.Add(item.Id, item);
}

// Add services to the container.
builder.Services.AddSingleton<Dictionary<string, Item>>(Items);
builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);
builder.Services.AddControllersWithViews();
// add swagger
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "WebApp",
            Description = "Todo Api"
        });

    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}else{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("CorsPolicy");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
