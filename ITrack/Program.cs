using ITrack.Data;
using ITrack.Data.Roles;
using ITrack.Data.Entities;
using ITrack.Data.Interfaces;
using ITrack.Data.Repositories;
using ITrack.Service.Implementations;
using ITrack.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<TrackDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IDataSeederService, DataSeederService>();

// [INFO] Register roles service
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();

// [INFO] Register users service
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// [INFO] Register directions service
builder.Services.AddScoped<IDirectionRepository, DirectionRepository>();
builder.Services.AddScoped<IDirectionService, DirectionService>();

// [INFO] Register statuses service
builder.Services.AddScoped<IStatusRepository, StatusRepository>();
builder.Services.AddScoped<IStatusService, StatusService>();

// [INFO] Register study plans service
builder.Services.AddScoped<IStudyPlanRepository, StudyPlanRepository>();
builder.Services.AddScoped<IStudyPlanService, StudyPlanService>();

// [INFO] Register plan stages service
builder.Services.AddScoped<IPlanStageRepository, PlanStageRepository>();
builder.Services.AddScoped<IPlanStageService, PlanStageService>();

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<TrackDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Mentoring", policy =>
        policy.RequireRole(RoleNames.Admin, RoleNames.Mentor));
});

var app = builder.Build();

// [INFO] Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// [INFO] Add authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope()) await DataSeeder.EnsureSeed(scope.ServiceProvider);

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
