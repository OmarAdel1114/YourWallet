using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using YourWallet.Data;
using YourWallet.Services;
using YourWallet.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<YourWalletContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

builder.Services.AddScoped<IExpensesService, ExpensesService>();

// --- Authentication ---
builder.Services.AddAuthentication(options =>
{
    // Cookie is the default — this is what keeps the user "logged in"
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    // Google is only used when we explicitly Challenge() with it
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
})
.AddGoogle(options =>
{
    // These come from User Secrets (never hardcoded!)
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;

    // Ask Google for these specific pieces of user data
    options.Scope.Add("email");
    options.Scope.Add("profile");

    // Save the token so you can use it later (e.g. to call Google APIs)
    options.SaveTokens = true;

    // Map the "picture" field from Google's response to a claim
    options.ClaimActions.MapJsonKey("picture", "picture");
});

// --- Services ---
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddControllersWithViews();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); // <-- Add this line to enable authentication
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");


app.Run();
