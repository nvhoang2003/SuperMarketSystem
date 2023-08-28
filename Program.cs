using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Google;
using SuperMarketSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using SuperMarketSystem.Repositories;
using MailKit;
using SendGrid.Helpers.Mail;
using System.Configuration;
using SuperMarketSystem.Repositories.Implements;
using SuperMarketSystem.Repositories.Interfaces;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DotNetEnv;
using SuperMarketSystem.Services.EmailService;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.AddAutoMapper(typeof(Program));

DotNetEnv.Env.Load(); // Đọc dữ liệu từ tệp .env

// Cấu hình ứng dụng
var envConfig = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("secrets.json", optional: true)
    .Build();

// Lấy connection string từ biến môi trường
var connectionString = envConfig["CONNECTION_STRING"];

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<MyDBContext>(options =>
        options.UseSqlServer(connectionString));
//add identity with user and role 
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<MyDBContext>()
        .AddDefaultTokenProviders();

//Đăng kí 
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<IAccountRepository, AccountRepository>();
builder.Services.AddTransient<IEmailService, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration.GetSection(nameof(MailSettings)));



//Thay đổi thời gian chờ email và hoạt động 
builder.Services.Configure<SecurityStampValidatorOptions>(o =>
                   o.ValidationInterval = TimeSpan.FromMinutes(1));

//Thay đổi tất cả tuổi thọ của mã thông báo bảo vệ dữ liệu
builder.Services.ConfigureApplicationCookie(o =>
{
    o.ExpireTimeSpan = TimeSpan.FromDays(5);
    o.SlidingExpiration = true;
});

builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>
       o.TokenLifespan = TimeSpan.FromHours(3));
//Cấu hình quyền truy cập vào các trang chi tiết
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizePage("/Guest");
    options.Conventions.AuthorizePage("/Admin");
    options.Conventions.AuthorizePage("/Customer");
    options.Conventions.AllowAnonymousToPage("/Home/Index");
});
//builder.Services.AddRazorPages();
builder.Services.Configure<IdentityOptions>(options =>
{
    // Default SignIn settings.
    options.SignIn.RequireConfirmedEmail = false; // Yêu cầu một email xác nhận để đăng nhập.
    options.SignIn.RequireConfirmedPhoneNumber = false; // Yêu cầu số điện thoại được xác nhận để đăng nhập.
    // Password settings.
    options.Password.RequireDigit = true; // Yêu cầu một số từ 0-9 trong mật khẩu.
    options.Password.RequireLowercase = true; // Yêu cầu một ký tự chữ thường trong mật khẩu.
    options.Password.RequireNonAlphanumeric = true; // Yêu cầu một ký tự không phải chữ và số trong mật khẩu.
    options.Password.RequireUppercase = true; // Yêu cầu một ký tự viết hoa trong mật khẩu.
    options.Password.RequiredLength = 6; // Độ dài tối thiểu của mật khẩu.
    options.Password.RequiredUniqueChars = 1; // Yêu cầu số ký tự riêng biệt trong mật khẩu

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Thời gian bị khóa sau khi đã vượt quá số lần thất bại đăng nhập cho phép.
    options.Lockout.MaxFailedAccessAttempts = 5; // Số lần được cho phép đăng nhập sai.
    options.Lockout.AllowedForNewUsers = true; // Nười dùng mới vẫn bị khóa nếu đăng nhập sai nhiều lần.

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"; //Các ký tự được phép trong tên người dùng.
    options.User.RequireUniqueEmail = false; //	Yêu cầu mỗi người dùng phải có một email duy nhất.
});
// Sets the default scheme to cookies and googles
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.Cookie.Name = "SupperMarketCookie";
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.LoginPath = "/Account/Login";
        options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
        options.SlidingExpiration = true;
        options.Cookie.SameSite = SameSiteMode.None; // Đảm bảo cài đặt cho tất cả các cookies
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Đảm bảo sử dụng HTTPS

    })
//Thêm xác thực bằng google nếu cần
    .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
    {
        options.ClientId = configuration["Authentication:Google:ClientId"];
        options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
        // Cấu hình Url callback lại từ Google (không thiết lập thì mặc định là /signin-google)
        options.CallbackPath = "/signin-google";
    });
builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

});

builder.Services.AddDistributedMemoryCache();
// add session
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "MyApp.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.IsEssential = true;
});

//add authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
          policy.RequireRole("AdminPolicy", "CustomerPolicy"));
    options.AddPolicy("CustomerPolicy", policy =>
          policy.RequireRole("Customer"));
});
// using Microsoft.AspNetCore.Identity;

builder.Services.Configure<PasswordHasherOptions>(option =>
{
    option.IterationCount = 12000;
});

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

app.UseAuthentication();
app.UseAuthorization();
CookiePolicyOptions cookiePolicy = new CookiePolicyOptions() { Secure = CookieSecurePolicy.Always };
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
//Tao role và tai khoan adm neu chua co

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "Customer" };
    var services = scope.ServiceProvider;
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    string email = "admin123@gmail.com";
    string password = configuration["SeedUserPW"];
    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new ApplicationUser()
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
        };
        var userResult = await userManager.CreateAsync(user, password);

        if (userResult.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "Admin");
        }
    }
}

app.Run();
