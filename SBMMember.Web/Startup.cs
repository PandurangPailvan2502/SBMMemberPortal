using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SBMMember.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SBMMember.Data.DataFactory;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System.IO;
using NToastNotify;
//using AspNetCoreHero.ToastNotification;

namespace SBMMember.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = new PathString("/Login/Login");
            });

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddDbContext<SBMMemberDBContext>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
             sqlServerOptionsAction: sqlOptions =>
             {
                 sqlOptions.EnableRetryOnFailure(
                 maxRetryCount: 10,
                 maxRetryDelay: TimeSpan.FromSeconds(30),
                 errorNumbersToAdd: null);
             }

             )
             );
            services.AddDbContext<MatrimonyDBContext>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("MatrimonyConnection"),
             sqlServerOptionsAction: sqlOptions =>
             {
                 sqlOptions.EnableRetryOnFailure(
                 maxRetryCount: 10,
                 maxRetryDelay: TimeSpan.FromSeconds(30),
                 errorNumbersToAdd: null);
             }

             )
             );
            services.AddTransient<IMemberDataFactory, MemberDataFactory>();
            services.AddTransient<IMemberSearchDataFactory, MemberSearchDataFactory>();
            services.AddScoped<IMemberContactDetailsDataFactory, MemberContactDetailsDataFactory>();
            services.AddScoped<IMemberBusinessDataFactory, MemberBusinessDataFactory>();
            services.AddScoped<IMemberEducationEmploymentDataFactory, MemberEducationEmploymentDataFactory>();
            services.AddScoped<IMemberFamilyDetailsDataFactory, MemberFamilyDetailsDataFactory>();
            services.AddScoped<IMemberFormStatusDataFactory, MemberFormStatusDataFactory>();
            services.AddScoped<IMemberPaymentsDataFactory, MemberPaymentsDataFactory>();
            services.AddScoped<IMemberPersonalDataFactory, MemberPersonalDataFactory>();
            services.AddScoped<IJobPostingDataFactory, JobPostingDataFactory>();
            services.AddScoped<IEventDataFactory, EventDataFactory>();
            services.AddScoped<IEventAdsDataFactory, EventAdsDataFactory>();
            services.AddScoped<IAdminUsersDataFactory, AdminUsersDataFactory>();
            services.AddScoped<IMarqueeDataFactory, MarqueeDataFactory>();
            services.AddScoped<IUpcomingEventsDataFactory, UpcomingEventsDataFactory>();
            services.AddScoped<IEventTitlesDataFactory, EventTitlesDataFactory>();
            services.AddScoped<IMemberMeetingsDataFactory, MemberMeetingsDataFactory>();
            services.AddScoped<IBannerAdsDataFactory, BannerAdsDataFactory>();
            services.AddScoped<ISubscriptionDataFactory, SubscriptionDataFactory>();
            services.AddScoped<IMatrimonySubscriptionDataFactory, MatrimonySubscriptionDataFactory>();

            // services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.BottomRight; });
           services.AddRazorPages().AddNToastNotifyNoty(new NotyOptions
           {
               ProgressBar = true,
               Timeout = 5000
           });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            

            app.UseSession();
            loggerFactory.AddFile("Logs/SBMMember-{Date}.txt");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
           Path.Combine(env.ContentRootPath, "EventGallery")),
                RequestPath = "/EventGallery"
            });

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();
            app.UseNToastNotify();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
              //pattern: "{controller=admindashboard}/{action=adminhome}/{id?}");
              //pattern: "{controller=Home}/{action=MemberHome}/{id?}");
                 //pattern: "{controller=admindashboard}/{action=adminhome}/{id?}");
               //pattern: "{controller=Home}/{action=MemberHome}/{id?}");

               pattern: "{controller=SplashScreen}/{action=SplashScreen}/{id?}");
                //pattern: "{controller=Payment}/{action=AcceptMemberPayment}/{id?}");
                //pattern: "{controller=MemberDashboard}/{action=ProfileUpdate}/{id?}");


            });
        }
    }
}
