using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{

    public static class ApplicationBuilderExtensions
    {
        //public static IApplicationBuilder RegisterWithConsul(this IApplicationBuilder app, IApplicationLifetime lifetime)
        //{
        //    //var consulClient = new ConsulClient(x => x.Address = new Uri($"http://{Program.IP}:8500"));//如果服务和 Consul 在同一台服务器上，使用此代码
        //    var consulClient = new ConsulClient();//请求注册的 Consul 地址
        //    var httpCheck = new AgentServiceCheck()
        //    {
        //        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
        //        Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔，或者称为心跳间隔
        //        HTTP = $"http://{Program.IP}:{Program.Port}/HealthCheck",//健康检查地址
        //        Timeout = TimeSpan.FromSeconds(5)
        //    };

        //    // Register service with consul
        //    var registration = new AgentServiceRegistration()
        //    {
        //        Checks = new[] { httpCheck },
        //        ID = Guid.NewGuid().ToString(),
        //        Name = Program.ServiceName,
        //        Address = Program.IP,
        //        Port = Program.Port,
        //        Tags = new[] { $"urlprefix-/{Program.ServiceName}" }//添加 urlprefix-/servicename 格式的 tag 标签，以便 Fabio 识别
        //    };

        //    consulClient.Agent.ServiceRegister(registration).Wait();//服务启动时注册，内部实现其实就是使用 Consul API 进行注册（HttpClient发起）
        //    lifetime.ApplicationStopping.Register(() =>
        //    {
        //        consulClient.Agent.ServiceDeregister(registration.ID).Wait();//服务停止时取消注册
        //    });
        //    return app;
        //}
    }
}
