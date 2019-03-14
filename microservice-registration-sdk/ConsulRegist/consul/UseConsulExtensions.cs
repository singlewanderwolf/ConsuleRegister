using System;
using System.Collections.Generic;
using System.Text;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace PhoenixMicroserviceRegistration.consul
{
    public static class UseConsulExtensions
    {
        public static IApplicationBuilder UseConsul(this IApplicationBuilder app,
            IApplicationLifetime lifetime,
            IConsulClient consul,
            IOptions<ServiceRegisterOptions> serviceRegisterOptions)
        {
            var serviceId =
                $"{serviceRegisterOptions.Value.ServiceName}_{serviceRegisterOptions.Value.ServiceHost}_{serviceRegisterOptions.Value.ServicePort}";

            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                Interval = TimeSpan.FromSeconds(30),
                HTTP = $"http://{serviceRegisterOptions.Value.ServiceHost}:{serviceRegisterOptions.Value.ServicePort}/api/health"
            };

            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                Address = serviceRegisterOptions.Value.ServiceHost,
                ID = serviceId,
                Name = serviceRegisterOptions.Value.ServiceName,
                Port = serviceRegisterOptions.Value.ServicePort
            };

            consul.Agent.ServiceRegister(registration).GetAwaiter().GetResult();

            lifetime.ApplicationStopping.Register(() =>
            {
                consul.Agent.ServiceDeregister(serviceId).GetAwaiter().GetResult();
            });

            return app;
        }
    }
}
