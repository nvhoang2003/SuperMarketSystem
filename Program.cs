using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SuperMarketSystem.Data;
using Microsoft.AspNetCore.Authentication.Google;




var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddDbContext<DataContext>();
builder.Services.AddDbContext<MyDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<ApplicationUser>(
    options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<MyDbContext>();
// Add default Identity check email to login

//builder.Services.AddIdentityCore<IdentityUser>(
//    options => options.SignIn.RequireConfirmedAccount = true)
//    .AddRoles<IdentityRole>()
//    .AddEntityFrameworkStores<MyDbContext>();

// Add Identity using Providers to check login

//builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
//    .AddDefaultTokenProviders();

// Force Identity's security stamp to be validated every minute.
// Hệ thống tự động check tài khoản mỗi phút 1 lần nếu có thay đổi về tài khoản sẽ thực hiện đăng xuất. 
builder.Services.Configure<SecurityStampValidatorOptions>(o =>
                   o.ValidationInterval = TimeSpan.FromMinutes(1)); 

builder.Services.AddRazorPages();

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
builder.Services.AddAuthentication()
            .AddCookie(options =>
            {
                options.AccessDeniedPath = "/account/denied";
                options.LoginPath = "/account/login";
            });
// Thêm xác thực bằng google nếu cần
            //.AddGoogle(googleOptions =>
            //{
            //    googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
            //    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
            //});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.Cookie.Name = "SupperMarketCookie";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.LoginPath = "/Identity/Account/Login";
    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    options.SlidingExpiration = true;
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
//app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
