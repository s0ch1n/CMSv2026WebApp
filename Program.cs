using CMSv2026WebApp.Repositories;
using CMSv2026WebApp.Services;

namespace CMSv2026WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //Fetching Connection String from appsettings.json
            var connectionString = builder.Configuration.GetConnectionString("ConnStrMVC");

            //Registering Connection String in Dependency Injection
            builder.Services.AddSingleton(connectionString);

            //Register Services and Repositories
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

            builder.Services.AddScoped<IAppointmentService, AppointmentService>();

            builder.Services.AddScoped<IPatientRepository, PatientRepository>();

            builder.Services.AddScoped<IPatientService, PatientService>();

            builder.Services.AddScoped<IDoctorService, DoctorService>();

            builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();


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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Accounts}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
