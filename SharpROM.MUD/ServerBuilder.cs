using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SharpROM.MUD
{
    public class ServerBuilder<T>
    {
        ServiceCollection Services = new ServiceCollection();
        IServiceProvider ServerServiceProvider { get; set; }
        ConfigureBuilder ConfigBuilder { get; set; }
        object StartupObj { get; set; }
        public ServerBuilder()
        {
            StartupObj = Activator.CreateInstance<T>();
            typeof(T).GetMethod("ConfigureServices").Invoke(StartupObj, new[] { Services });
            Services.AddSingleton(this);
            ServerServiceProvider = Services.BuildServiceProvider();
            ConfigBuilder = new ConfigureBuilder(typeof(Startup), StartupObj, ServerServiceProvider);
        }
        public void Run()
        {
            ConfigBuilder.Run();
        }

        private class ConfigureBuilder
        {
            IServiceProvider ServiceProvider { get; set; }
            private MethodInfo FindMethod(string methodName, Type returnType = null)
            {

                var methods = StartupType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                var selectedMethods = methods.Where(method => method.Name.Equals(methodName)).ToList();


                var methodInfo = selectedMethods.FirstOrDefault();

                return methodInfo;
            }
            private object Instance { get; }
            private Type StartupType { get; } 
            public ConfigureBuilder(Type t, object instance, IServiceProvider serviceProvider)
            {
                StartupType = t;
                Instance = instance;
                ServiceProvider = serviceProvider;
                Invoke(FindMethod("Configure"));
            }
            public void Run()
            {
                Invoke(FindMethod("Run"));
            }
            private void Invoke(MethodInfo methodInfo)
            {
                var parameterInfos = methodInfo.GetParameters();
                var parameters = new object[parameterInfos.Length];
                for (var index = 0; index < parameterInfos.Length; index++)
                {
                    var parameterInfo = parameterInfos[index];
                    //since this is constructed by the DI, it can't exactly exist in the service list, so we shortcircuit this request here
                    // for the configure method
                    if (parameterInfo.ParameterType == typeof(IServiceProvider))
                    {
                        parameters[index] = ServiceProvider;
                    }
                    else
                    {
                        try
                        {
                            parameters[index] = ServiceProvider.GetRequiredService(parameterInfo.ParameterType);
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidOperationException("Error getting parameter info for method: " + ex.Message);
                        }
                    }

                }
                methodInfo.Invoke(Instance, parameters);
            }
        }
    }
}
