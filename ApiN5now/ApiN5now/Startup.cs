using Connection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ApiN5now
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        readonly string MyAllowSpecificOrigins = "_ApiN5now";
        private string strConnection = "";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            strConnection = Configuration.GetConnectionString("BD").ToString() ?? "";
            services.AddDbContext<Context>(opt => opt.UseSqlServer(strConnection));
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
           

            //Settings
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
