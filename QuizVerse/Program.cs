using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuizVerse.Data;
using QuizVerse.Models;
using QuizVerse.Repositories;
using QuizVerse.Reposotories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
}); 
builder.Services.AddControllersWithViews();
builder.Services.AddAntiforgery(options => options.HeaderName = "RequestVerificationToken");
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<IQuizRepository, QuizRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IAttemptRepository, AttemptRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync();

    foreach (var role in new[] { "Admin", "User" })
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    var adminEmail = "admin@quizverse.com";
    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var admin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FullName = "Admin",
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(admin, "ADm1n@2D");
        if (result.Succeeded)
            await userManager.AddToRoleAsync(admin, "Admin");
    }

    static Question Q(string text, string correct, params string[] wrong) => new()
    {
        Text = text,
        Option = wrong.Select(w => new AnswerOption { text = w })
            .Append(new AnswerOption { text = correct, IsCorrect = true })
            .OrderBy(_ => Guid.NewGuid())
            .ToList()
    };

    if (!db.Quizzes.Any())
    {
        db.Quizzes.AddRange(
            new Quiz
            {
                Title = "C# Basics",
                Description = "Test your knowledge of C# fundamentals.",
                TimeLimitMinutes = 10,
                IsPublished = true,
                Questions = new List<Question>
                {
                Q("Which keyword creates a new object?", "new", "make", "create", "alloc"),
                Q("Which type stores true or false?", "bool", "int", "string", "char"),
                Q("Which loop always runs at least once?", "do-while", "for", "foreach", "while")
                }
            },
            new Quiz
            {
                Title = "HTML Quiz",
                Description = "Check your HTML skills.",
                TimeLimitMinutes = 10,
                IsPublished = true,
                Questions = new List<Question>
                {
                Q("Which tag creates a hyperlink?", "<a>", "<link>", "<href>", "<url>"),
                Q("Which tag is the largest heading?", "<h1>", "<h6>", "<head>", "<title>")
                }
            },
            new Quiz
            {
                Title = "SQL Intro",
                Description = "Basic queries and filtering.",
                TimeLimitMinutes = 10,
                IsPublished = false,
                Questions = new List<Question>
                {
                Q("Which keyword reads rows from a table?", "SELECT", "GET", "FETCH", "OPEN"),
                Q("Which clause filters rows?", "WHERE", "ORDER BY", "GROUP BY", "JOIN")
                }
            });
        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
