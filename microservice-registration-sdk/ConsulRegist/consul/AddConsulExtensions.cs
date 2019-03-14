using System;
using System.Collections.Generic;
using System.Text;
using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace PhoenixMicroserviceRegistration.consul
{
    public  static class AddConsulExtensions
    {
        /// <summary>
        /// 注入consul客户端
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddConsul(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ServiceRegisterOptions>(configuration.GetSection("ServiceRegister"));
            services.AddSingleton<IConsulClient>(c => new ConsulClient(cfg =>
            {
                var serviceConfiguration = c.GetRequiredService<IOptions<ServiceRegisterOptions>>().Value;
                if (!string.IsNullOrWhiteSpace(serviceConfiguration.Register.HttpEndpoint))
                {
                    cfg.Address = new Uri(serviceConfiguration.Register.HttpEndpoint);
                }
            }));
            return services;
        }
    }
}
