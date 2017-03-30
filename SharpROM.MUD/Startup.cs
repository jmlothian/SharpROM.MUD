using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RazorLight;
using SharpROM.Events;
using SharpROM.Events.Abstract;
using SharpROM.MUD.ConnectedStates;
using SharpROM.MUD.Models;
using SharpROM.Net;
using SharpROM.Net.Abstract;
using SharpROM.Net.Telnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SharpROM.MUD.Utils;
using SharpROM.MUD.Presenters;

namespace SharpROM.MUD
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }

        //https://github.com/aspnet/Mvc/blob/576c0e6a654ae67ab77bfec1e9f9c412cb30427e/src/Microsoft.AspNetCore.Mvc.Core/Internal/MiddlewareFilterConfigurationProvider.cs
        // see "Invoke" for how this is done
        //todo: lots of cleanup here - we need the idea of an applicationbuilder, to get some of the logic out of this class, and also
        // to have a class that extension config methods can be added to that has access to the provider
        public void Configure(IServiceProvider serviceProvider,
            Dictionary<CONN_STATE, IConnStateHandler> connectedStateHandlers
            )
        {
            serviceProvider.GetService<ILoggerFactory>().AddConsole(LogLevel.Trace);


            //Configure connected state handlers
            serviceProvider.ConfigureAutoregistration(connectedStateHandlers);
        }

        //make this injectable as well, move telnet start/run here
        public void Run(IServiceProvider serviceProvider)
        {
            //run server
            serviceProvider.UseServer();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            Configuration = builder.Build();

            services.AddOptions();
            services.Configure<SocketListenerSettings>(Configuration);
            services.AddSingleton<TelOptManagement>();
            services.AddSingleton<TelnetEventHandler, MUDMainEventHandler>();
            services.AddSingleton<MUDServer>();
            services.AddSingleton<ViewController>();
            //todo: eventually we will need to pass a factory for a list of IEventManagers
            services.AddSingleton<IEventManager, EventManager>();
            services.AddSingleton<GameDataManager>();
            services.AddSingleton<GameData>();
            services.AddSingleton<ISocketListener, SocketListener>();
            services.AddSingleton<ISocketReceiveParserFactory, TelnetSocketReceiveParserFactory>();
            services.AddSingleton<IEventRoutingService, EventRoutingService>();
            services.AddSingleton<Dictionary<CONN_STATE, IConnStateHandler>>();
            services.AddSingleton<RazorLightEngine>();
            services.AddSingleton<RoomPresenter>();
            //ILoggerFactory loggerFactory = new LoggerFactory().AddConsole();
            services.AddLogging();

            //var provider = services.BuildServiceProvider();

            //ILoggerFactory loggerFactory 
            //autoregister connected state event handlers
            services.AddAutoRegistration();
        }
    }
}
