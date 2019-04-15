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
