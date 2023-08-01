using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Google;
using SuperMarketSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using SuperMarketSystem.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddAutoMapper(typeof(Program));

DotNetEnv.Env.Load(); // Đọc dữ liệu từ tệp .env

// Cấu hình ứng dụng
var envConfig = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();

// Lấy connection string từ biến môi trường
var connectionString = envConfig["CONNECTION_STRING"];

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<MyDBContext>(options =>
        options.UseSqlServer(connectionString));
//Add default Identity check email to login
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddEntityFrameworkStores<MyDBContext>()
        .AddSignInManager<SignInManager<ApplicationUser>>();
//builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
//        .AddEntityFrameworkStores<MyDBContext>()
//        .AddSignInManager<SignInManager<ApplicationUser>>();

//builder.Services.AddIdentity<IdentityUser, IdentityRole>()
//        .AddEntityFrameworkStores<MyDBContext>()
//        .AddDefaultTokenProviders();
//Add Identity SignIn


// Add Identity with role

//builder.Services.AddIdentityCore<IdentityUser>(
//    options => options.SignIn.RequireConfirmedAccount = true)
//    .AddRoles<IdentityRole>()
//    .AddEntityFrameworkStores<MyDBContext>();

// Add Identity using Providers to check login

//builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
//    .AddDefaultTokenProviders();

//Đăng kí send email
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);
// Force Identity's security stamp to be validated every minute.
// Hệ thống tự động check tài khoản mỗi phút 1 lần nếu có thay đổi về tài khoản sẽ thực hiện đăng xuất.

//Thay đổi thời gian chờ email và hoạt động 
builder.Services.Configure<SecurityStampValidatorOptions>(o =>
                   o.ValidationInterval = TimeSpan.FromMinutes(1));

//Thay đổi tất cả tuổi thọ của mã thông báo bảo vệ dữ liệu
builder.Services.ConfigureApplicationCookie(o => {
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
    options.Conventions.AllowAnonymousToPage("/Guest");
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
        options.AccessDeniedPath = "/Identity/Account/AccessDenied";
        options.Cookie.Name = "SupperMarketCookie";
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.LoginPath = "/Identity/Account/Login";
        options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
        options.SlidingExpiration = true;
    });
// Thêm xác thực bằng google nếu cần
//.AddGoogle(googleOptions =>
//{
//    googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
//    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
//});


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
      policy.RequireRole("Admin", "Customer", "Guest"));
options.AddPolicy("CustomerPolicy", policy =>
      policy.RequireRole("Customer", "Guest"));
options.AddPolicy("GuestPolicy", policy =>
      policy.RequireRole("Guest"));
});

//builder.Services.AddControllers(config =>
//{
//    var policy = new AuthorizationPolicyBuilder()
//        .RequireAuthenticatedUser()
//        .Build();
//    config.Filters.Add(new AuthorizeFilter(policy));
//});
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
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    Secure = CookieSecurePolicy.Always
});
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
