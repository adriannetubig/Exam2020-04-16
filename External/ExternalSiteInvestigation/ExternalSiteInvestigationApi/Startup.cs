using ExternalSiteInvestigationApi.Helper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExternalSiteInvestigationApi
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
            services.AddControllers();
            var internalDomainCheckUrl = Configuration.GetSection("InternalDomainCheckUrl").Get<string>();
            var internalVirusTotalIntegrationUrl = Configuration.GetSection("InternalVirusTotalIntegrationUrl").Get<string>();
            var internalOrderUrl = Configuration.GetSection("InternalOrderUrl").Get<string>();

            Dependency.SetDependency(ref services, internalDomainCheckUrl, internalOrderUrl, internalVirusTotalIntegrationUrl);
            services.AddSingleton(AutoMapperConfig.Config());

            ApiVersioning.SetVersion(ref services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(a =>
            {
                a.SwaggerEndpoint("/swagger/v1/swagger.json", "External Site Investigator");
                a.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
