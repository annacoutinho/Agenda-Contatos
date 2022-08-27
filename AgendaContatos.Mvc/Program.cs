using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Habilitar o uso de Cookies de Autenticação 
builder.Services.Configure<CookiePolicyOptions>(options => { options.MinimumSameSitePolicy = SameSiteMode.None;  });

 builder.Services.AddAuthentication
    (CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

//Habilitar o uso de Cookies de Autenticação do AspNet MVC
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
