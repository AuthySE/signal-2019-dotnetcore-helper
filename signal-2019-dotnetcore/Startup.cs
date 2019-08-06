﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Twilio;

namespace signal_2019_dotnetcore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var twilioSection = Configuration.GetSection("Twilio");
            var accountSid = twilioSection.GetValue<string>("AccountSid");
            var authToken = twilioSection.GetValue<string>("AuthToken");
            var authyApiKey = twilioSection.GetValue<string>("AuthyApiKey");
            var verifyServiceSid = twilioSection.GetValue<string>("VerifyServiceSid");

            if (string.IsNullOrEmpty(accountSid))
                throw new ArgumentNullException(nameof(accountSid));

            if (string.IsNullOrEmpty(authToken))
                throw new ArgumentNullException(nameof(authToken));

            if (string.IsNullOrEmpty(verifyServiceSid))
                throw new ArgumentException(nameof(verifyServiceSid));

            if (string.IsNullOrEmpty(authyApiKey))
                throw new ArgumentNullException(nameof(authyApiKey));

            TwilioClient.Init(accountSid, authToken);

            services.AddHttpClient("authy", c =>
            {
                c.BaseAddress = new Uri("https://api.authy.com");
                c.DefaultRequestHeaders.Add("X-Authy-API-Key", authyApiKey);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.Configure<TwilioOptions>(twilioSection);

            services.AddSingleton<IUserRepository, UserRepository>();

            services.AddSession(options =>
            {
                options.Cookie.Name = "twilio.demo";
                options.Cookie.IsEssential = true;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDefaultFiles()
                .UseStaticFiles()
                .UseStaticFiles()
                .UseSession()
                .UseMvc(routes =>
                {
                    routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
                });
        }
    }
}
