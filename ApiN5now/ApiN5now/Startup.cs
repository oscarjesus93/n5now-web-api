using ApiN5now.Service;
using Connection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ApiN5now
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private string strConnection = "";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            strConnection = Configuration.GetConnectionString("BD").ToString() ?? "";
            services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(strConnection), ServiceLifetime.Scoped);
            services.AddMemoryCache();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            //Serilog
            services.AddLogging(logginBuilder =>
            {
                logginBuilder.ClearProviders();
                logginBuilder.AddSerilog();
            });

            //Services
            services.AddTransient<PermisionService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }

            //app.UseCallLoggerMiddleware();
            //app.UseMiddleware<TraceMiddleware>();
            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
