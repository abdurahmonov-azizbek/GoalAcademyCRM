// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using GoalAcademyCRM.Api.Brokers.DateTimes;
using GoalAcademyCRM.Api.Brokers.Loggings;
using GoalAcademyCRM.Api.Brokers.Storages;
using GoalAcademyCRM.Api.Services.Foundations.Attendances;
using GoalAcademyCRM.Api.Services.Foundations.Auth;
using GoalAcademyCRM.Api.Services.Foundations.Courses;
using GoalAcademyCRM.Api.Services.Foundations.Groups;
using GoalAcademyCRM.Api.Services.Foundations.Payments;
using GoalAcademyCRM.Api.Services.Foundations.StudentGroups;
using GoalAcademyCRM.Api.Services.Foundations.Users;
using GoalAcademyCRM.Api.Services.Helpers.Auth;
using GoalAcademyCRM.Api.Services.Helpers.Auth.Models;
using GoalAcademyUpdate001.Api.Services.Foundations.Payments;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace GoalAcademyCRM.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddOData(options =>
                    options.Select().Filter().OrderBy().Count().Expand());

            services.AddDbContext<StorageBroker>();

            services.AddCors(option =>
            {
                option.AddPolicy("MyPolicy", config =>
                {
                    config.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            services.Configure<JwtSettings>(Configuration.GetSection(nameof(JwtSettings)));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    var jwtSettings = Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();

                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = jwtSettings.ValidateIssuer,
                        ValidIssuer = jwtSettings.ValidIssuer,
                        ValidateAudience = jwtSettings.ValidateAudience,
                        ValidAudience = jwtSettings.ValidAudience,
                        ValidateLifetime = jwtSettings.ValidateLifeTime,
                        ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                    };
                });

            services.AddAuthorization();

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc(
                    name: "v1",
                    info: new OpenApiInfo { Title = "GoalAcademyCRM.Api", Version = "v1" });

                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] then your valid token here"
                });

                config.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });


            AddBrokers(services);
            AddFoundationServices(services);
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(config =>
                config.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "GoalAcademyCRM.Api v1"));

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("MyPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        private static void AddBrokers(IServiceCollection services)
        {
            services.AddTransient<IStorageBroker, StorageBroker>();
            services.AddTransient<ILoggingBroker, LoggingBroker>();
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();
        }

        private static void AddFoundationServices(IServiceCollection services)
        {
            services.AddTransient<IAttendanceService, AttendanceService>();
            services.AddTransient<ICourseService, CourseService>();
            services.AddTransient<IGroupService, GroupService>();
            services.AddTransient<IStudentGroupService, StudentGroupService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITokenGeneratorService, TokenGeneratorService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IPaymentService, PaymentService>();
        }
    }
}
