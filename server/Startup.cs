using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Okta.AspNetCore;
using Microsoft.AspNetCore.Cors;
using server.Account;
using server.Configuration;
using Microsoft.Extensions.Options;
using server.Repository;
using SendGrid;
using server.Email;
using server.Hubs;
using server.Game;
using server.Lobbies;
using AutoMapper;
using server.Mappers;

namespace server
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
            ConfigureDependencies(services);

            services.Configure<LobbyDatabaseSettings>(Configuration.GetSection(nameof(LobbyDatabaseSettings)));
            services.AddSingleton<ILobbyDatabaseSettings>(sp => 
                sp.GetRequiredService<IOptions<LobbyDatabaseSettings>>().Value
            );

            services.Configure<EmailSettings>(Configuration.GetSection(nameof(EmailSettings)));
            services.AddSingleton<EmailSettings>(sp => 
                sp.GetRequiredService<IOptions<EmailSettings>>().Value
            );

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
                options.DefaultChallengeScheme = OktaDefaults.ApiAuthenticationScheme;
                options.DefaultSignInScheme = OktaDefaults.ApiAuthenticationScheme;
            })
            .AddOktaWebApi(new OktaWebApiOptions()
            {
                //OktaDomain = Configuration["Okta:OktaDomain"],
                OktaDomain = "https://dev-545127.okta.com",
            });
            services.AddCors();   
            services.AddSignalR(); 
            services.AddAutoMapper(typeof(DefaultProfile));

            services.AddAuthorization();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();
            var corsSection = Configuration.GetSection("CorsUrls").GetChildren().ToArray().Select(x => x.Value).ToArray();

            app.UseCors(builder => {
                builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins(corsSection)
                    .AllowCredentials();
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<LobbyHub>("/lobbyHub");
                endpoints.MapHub<ChatHub>("/chatHub");
                endpoints.MapHub<GameHub>("/gameHub");
            });
        }

        public void ConfigureDependencies(IServiceCollection services)
        {
            string apiToken = Configuration["Okta:ApiToken"];
            services.AddHttpClient<AccountService>("apiClient", configureClient => {
                configureClient.BaseAddress = new Uri(Configuration["Okta:BaseUrl"]);
                configureClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("SSWS", apiToken);
            });

            services.AddSingleton<ILobbyRepository, InMemoryLobbyRepository>();
            services.AddSingleton<ICardGameRepository, InMemoryCardGameRepository>();
            services.AddTransient<LobbyService>();
            services.AddTransient<GameService>();
            services.AddTransient<EmailService>();
            services.AddTransient<GameService>();
            services.AddTransient<PhaseService>();

            services.AddSingleton<ISendGridClient>(sp => 
                new SendGridClient(Configuration["EmailSettings:ApiKey"])
            );
        }
    }
}
