﻿using AutoMapper;
using backend_app.Application.Services;
using backend_app.Data.Collections;
using backend_app.Data.Contexts;
using backend_app.Domain.Collections;
using backend_app.Domain.Contexts;
using backend_app.Domain.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace backend_app
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Database Context
            services.Configure<DatabaseContext>(Configuration.GetSection(nameof(DatabaseContext)));
            services.AddSingleton<IDatabaseContext>(sp => sp.GetRequiredService<IOptions<DatabaseContext>>().Value);

            // Account collection
            services.AddScoped<IAccountCollection, AccountCollection>();
            services.AddScoped<IContactCollection, ContactCollection>();
            services.AddScoped<IConversationCollection, ConversationCollection>();

            // Services
            services.AddScoped<ISignUpService, SignUpService>();
            services.AddScoped<ILogInService, LogInService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IConversationService, ConversationService>();

            // Automapper service
            services.AddAutoMapper(typeof(Startup));
            
            // Authentication
            services.AddAuthentication(x => x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    byte[] keyByteArray = Encoding.ASCII.GetBytes(Configuration["JWT_KEY"]);
                    SymmetricSecurityKey key = new SymmetricSecurityKey(keyByteArray);
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = key,
                        ValidIssuer = "chat-system/backend-app",
                        ValidAudience = "chat-system/frontend-app",
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.FromMinutes(0)
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}