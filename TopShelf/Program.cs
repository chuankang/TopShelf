using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using System.Configuration;
using Topshelf;
using Abp.Dependency;
using Abp;

namespace TopShelf
{
    class Program
    {
        static void Main(string[] args)
        {
            #region MyRegion
            AbpBootstrapper abpBootstrapper = new AbpBootstrapper();
            abpBootstrapper.Initialize();
            var logCfg = new FileInfo(System.AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.ConfigureAndWatch(logCfg);
            //定时时间多久执行一次
            var interval = int.Parse(ConfigurationManager.AppSettings["interval"] ?? (1 * 60 * 1000).ToString());

            Topshelf.HostFactory.Run(x =>//1.我们用HostFactory.Run来设置一个宿主主机。我们初始化一个新的lambda表达式X，来显示这个宿主主机的全部配置。
            {
                x.Service<IAppService>(s =>//2.告诉Topshelf ，有一个类型为“IAppService服务”,通过定义的lambda 表达式的方式，配置相关的参数。
                {
                    //依赖注入
                    s.ConstructUsing(name => IocManager.Instance.Resolve<GetOrderBatchService>(new { interval = interval }));
                    //直接new 配置一个完全定制的服务,对Topshelf没有依赖关系。常用的方式。
                    //3.告诉Topshelf如何创建这个服务的实例，目前的方式是通过new 的方式，但是也可以通过Ioc 容器的方式：getInstance<towncrier>()。
                    //s.ConstructUsing(name=>new GetOrderBatchService(interval));
                    //4.开始 Topshelf 服务。
                    s.WhenStarted(tc => tc.Start());
                    //5.停止 Topshelf 服务。
                    s.WhenStopped(tc => tc.Stop());
                });
                //服务使用NETWORK_SERVICE内置帐户运行。身份标识，有好几种方式，如：x.RunAs("username", "password"); x.RunAsPrompt(); x.RunAsNetworkService(); 等
                x.RunAsLocalSystem(); //服务使用NETWORK_SERVICE内置帐户运行

                x.SetDescription("订单同步");//服务的描述
                x.SetDisplayName("订单同步");//显示名称
                x.SetServiceName("SYNC_ORDER");//服务名称

            });


            #endregion

        }
    }
}
