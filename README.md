# ConsuleRegister
## description
this  is a simple SDK for register services into consule
## sample
```
 public void ConfigureServices(IServiceCollection services)
    {
        services.AddConsul(Configuration);

    }
```
```
 public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime lifetime, IConsulClient consulClient, IOptions<ServiceRegisterOptions> serviceRegisterOptions)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseConsul(lifetime, consulClient, serviceRegisterOptions);
        }
```
appsettings.json
```
  "ServiceRegister": {
    "IsActive": true,
    "ServiceName": "Name",
    "ServiceHost": "Host",
    "ServicePort": 80,
    "Register": {
      "HttpEndpoint": "http://consul-agent-1:8500"
    }
  }
```
