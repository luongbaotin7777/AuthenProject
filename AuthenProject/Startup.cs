using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthenProject.Authorization;
using AuthenProject.Authorization.AuthorizationHandler;
using AuthenProject.EFModel;
using AuthenProject.Entities;
using AuthenProject.Service.Handle;
using AuthenProject.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using static AuthenProject.Authorization.Permission;

namespace AuthenProject
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
            //Register SQL Dbcontext
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            //Register Identity service
            services.AddIdentity<AppUser, AppRole>(options =>
            {

                // Cấu hình Lockout - khóa user
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
                options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lần thì khóa
                options.Lockout.AllowedForNewUsers = true;
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ApplicationDbContext>() // lưu trữ thông tin identity trên EF( dbcontext->MySQL)
                .AddDefaultTokenProviders();            // register tokenprovider : phát sinh token (resetpassword, email...)
            //Adding Authentication
            services.AddAuthentication(auths =>
            {
                auths.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auths.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            // Adding JWT Bearer
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters()
                 {
                     ValidateIssuer = false,
                     ValidateAudience = false,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     RequireExpirationTime = true,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwts:Key"]))
                 };
             });
            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                // Trên 30 giây truy cập lại sẽ nạp lại thông tin User (Role)
                // SecurityStamp trong bảng User đổi -> nạp lại thông tinn Security
                options.ValidationInterval = TimeSpan.FromSeconds(30);
            });
            //DI IUserService
            services.AddTransient<IUserService, UserService>();

            //DI IRoleService
            services.AddTransient<IRoleService, RoleService>();
            //DI Um,Rm,SM
            services.AddTransient<UserManager<AppUser>, UserManager<AppUser>>();
            services.AddTransient<SignInManager<AppUser>, SignInManager<AppUser>>();
            services.AddTransient<RoleManager<AppRole>, RoleManager<AppRole>>();
            //Register the handler
            services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
            //create a policy for each permission
            services.AddAuthorization(options =>
            {
                //Policy for users
                options.AddPolicy(Permission.Users.Create, policy =>
                {
                    policy.AddRequirements(new PermissionRequirement(Permission.Users.Create));
                });
                options.AddPolicy(Permission.Users.View, policy =>
                {
                    policy.AddRequirements(new PermissionRequirement(Permission.Users.View));
                });
                options.AddPolicy(Permission.Users.Edit, policy =>
                {
                    policy.AddRequirements(new PermissionRequirement(Permission.Users.Edit));
                });
                options.AddPolicy(Permission.Users.Delete, policy =>
                {
                    policy.AddRequirements(new PermissionRequirement(Permission.Users.Delete));
                });
                //Policy for dashboard
                options.AddPolicy(Permission.Dashboards.Create, policy =>
                {
                    policy.AddRequirements(new PermissionRequirement(Permission.Dashboards.Create));
                });
                options.AddPolicy(Permission.Dashboards.View, policy =>
                {
                    policy.AddRequirements(new PermissionRequirement(Permission.Dashboards.View));
                });
                options.AddPolicy(Permission.Dashboards.Edit, policy =>
                {
                    policy.AddRequirements(new PermissionRequirement(Permission.Dashboards.Edit));
                });
                options.AddPolicy(Permission.Dashboards.Delete, policy =>
                {
                    policy.AddRequirements(new PermissionRequirement(Permission.Dashboards.Delete));
                });


            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
