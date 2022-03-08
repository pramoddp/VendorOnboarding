using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BPCloud.VendorMasterService.DBContexts;
using BPCloud.VendorMasterService.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BPCloud.VendorMasterService
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
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            }); 
            services.AddDbContext<MasterContext>(o => o.UseSqlServer(Configuration.GetConnectionString("MasterContext")));
            services.AddTransient<ITypeRepository, TypeRepository>();
            services.AddTransient<IPostalRepository, PostalRepository>();
            services.AddTransient<IIdentityRepository, IdentityRepository>();
            services.AddTransient<IBankRepository, BankRepository>();
            services.AddTransient<ITitleRepository, TitleRepository>();
            services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            services.AddTransient<IAppRepository, AppRepository>();
            services.AddTransient<ILocationRepository, LocationRepository>();
            services.AddTransient<IFieldMasterRepository, FieldMasterRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("MyPolicy");
            app.UseMvc();
        }
    }
}
