using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using KTUSTPPBiudzetas.Models;
using KTUSTPPBiudzetas.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.AspNetCore.Identity;
using KTUSTPPBiudzetas.Repositories;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Routing;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Web.Mvc;
using System.Net.Http;
using System.Globalization;

namespace KTUSTPPBiudzetas
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddIdentity<User, IdentityRole>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<BudgetContext>()
                .AddDefaultTokenProviders();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddRazorPagesOptions(options =>
                {
                    options.AllowAreas = true;
                    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                    options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
                });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

            // using Microsoft.AspNetCore.Identity.UI.Services;
            //services.AddSingleton<IEmailSender, EmailSender>();

            // configure jwt authentication
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = Configuration["JwtIssuer"],
                    ValidAudience = Configuration["JwtIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
                    ClockSkew = TimeSpan.Zero // remove delay of token when expire
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireClaimMember", policy => policy.RequireClaim("Member"));
                options.AddPolicy("RequireClaimController", policy => policy.RequireClaim("Controller"));
                options.AddPolicy("RequireClaimFamilyHead", policy => policy.RequireClaim("FamilyHead"));
            });

            // configure DI for application services
            services.AddScoped<IUserService, UserService>();


            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IMemberService, MemberService>();

            services.AddScoped<ICheckRepository, CheckRepository>();
            services.AddScoped<ICheckService, CheckService>();

            services.AddScoped<IPurchaseRepository, PurchaseRepository>();
            services.AddScoped<IPurchaseService, PurchaseService>();

            services.AddDbContext<BudgetContext>(cfg =>
                cfg.UseSqlServer(Configuration.GetConnectionString("BudgetConnectionString")));
            
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("Budget", new Info { Title = "My API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    In = "header",
                    Description = "Please insert: Bearer (token) into field",
                    Name = "Authorization",
                    Type = "apiKey"
                });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", Array.Empty<string>() }
                });
            });

            services.Configure<IdentityOptions>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequireUppercase = false;
                o.Password.RequiredLength = 6;
                o.Password.RequiredUniqueChars = 1;

                o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                o.Lockout.MaxFailedAccessAttempts = 5;
                o.Lockout.AllowedForNewUsers = true;

                o.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                o.User.RequireUniqueEmail = false;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseRequestLocalization();

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("lt");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("lt");

            System.Globalization.CultureInfo customCulture = new CultureInfo("lt");
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            CultureInfo.DefaultThreadCurrentCulture = customCulture;
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("lt");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Budget/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            //app.UseAuthentication();

            //// Enable middleware to serve generated Swagger as a JSON endpoint.
            //app.UseSwagger();

            //// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            //// specifying the Swagger JSON endpoint.
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/budget/swagger.json", "My API V1");
            //    c.RoutePrefix = "budget";
            //});

            //app.UseMvc();
            //.UseMvcWithDefaultRoute();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    //template: "{controller}/{action}/{id?}",
                    template: "{controller}/{id?}",
                    defaults: new { controller = "Budget", action = "Index" }
                    );
            });
        }
    }
}
