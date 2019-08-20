Distributed .NET Core – Service discovery & Load balancing with Consul + Fabio

 a service registry, which enables the service discovery capabilities for the microservices and eventually, we add Fabio on top of the stack, which provides an additional load balancer and a dynamic routing table.
 
 
app.Use(async (context, next) =>
            {
                var consulClient = new ConsulClient();
                var httpCheck = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                    Interval = TimeSpan.FromSeconds(10),
                    HTTP = $"http://localhost:{context.Request.Host.Port}/HealthCheck",
                    Timeout = TimeSpan.FromSeconds(5)
                };

                // Register service with consul
                var registration = new AgentServiceRegistration()
                {
                    Checks = new[] { httpCheck },
                    ID = "consul-" + context.Request.Host.Port.Value,
                    Name = "consul-api",
                    Address = "localhost",
                    Port = (int)context.Request.Host.Port,
                    Tags = new[] { $"urlprefix-/consul-api" }
                };

                await consulClient.Agent.ServiceRegister(registration);

                lifetime.ApplicationStopping.Register(async () =>
                {
                    await consulClient.Agent.ServiceDeregister(registration.ID);
                });
            });           
