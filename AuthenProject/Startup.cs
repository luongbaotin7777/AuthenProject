using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthenProject.Authorization;
using AuthenProject.Authorization.AuthorizationHandler;
using AuthenProject.Common;
using AuthenProject.EFModel;
using AuthenProject.Entities;
using AuthenProject.Service.Handle;
using AuthenProject.Service.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
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
            //Register ApplicationDbContext
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                //Read string connection user SQLSERVER
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            //Register Identity service
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                //Password.RequiredDigit is default true
                //Password.RequiredLowerCase is default true
                //Password.RequiredUpperCase is default true
                // Cấu hình Lockout - khóa user
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
                options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lần thì khóa
                options.Lockout.AllowedForNewUsers = true;
                options.User.RequireUniqueEmail = true;
                // Cấu hình đăng nhập.
                /* options.SignIn.RequireConfirmedEmail = true;*/            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
                /* options.SignIn.RequireConfirmedPhoneNumber = false;*/     // Xác thực số điện thoại
            }).AddEntityFrameworkStores<ApplicationDbContext>() // lưu trữ thông tin identity trên EF( dbcontext->MySQL)
                .AddDefaultTokenProviders();            // register tokenprovider : phát sinh token (resetpassword, email...)
                                                        //Adding Authentication

            services.AddAuthentication(auths =>
            {
                auths.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auths.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                //auths.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
                //auths.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            // Adding JWT Bearer
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters()
                 {
                     ValidateIssuer = false,
                     ValidateAudience = false,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,//xác minh rằng khóa được sử dụng để ký mã thông báo đến là một phần của danh sách các khóa đáng tin cậy
                     RequireExpirationTime = true,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwts:Key"]))
                 };
             })
            .AddGoogle(options =>

            {
                
                options.SignInScheme = IdentityConstants.ExternalScheme;
                IConfigurationSection googleAuthNSection =
                Configuration.GetSection("Authentication:Google");
               
                options.ClientId = googleAuthNSection["ClientId"];
                options.ClientSecret = googleAuthNSection["ClientSecret"];
               


                //options.SaveTokens = true;
                //options.ClientId = "828325491609-03jmf69n74fmeq6t2a1easqj24cdudd1.apps.googleusercontent.com";
                //options.ClientSecret = "Y__Jz7IyM40v2f6tui7_Sr3-";
                //options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //options.ClaimActions.Clear();


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

            services.AddTransient<ITokenService,TokenService>();
           
            //DI IProductService
            services.AddTransient<IProductService, ProductService>();
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
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen();
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
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });
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
