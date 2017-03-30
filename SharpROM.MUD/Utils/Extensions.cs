﻿using Microsoft.Extensions.DependencyInjection;
using SharpROM.MUD.Abstract;
using SharpROM.MUD.ConnectedStates;
using SharpROM.Net.Telnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SharpROM.MUD.Utils
{
    public static class Extensions
    {
        public static void ConfigureAutoregistration(this IServiceProvider serviceProvider, Dictionary<CONN_STATE, IConnStateHandler> connectedStateHandlers)
        {
            Type[] types = Assembly.GetEntryAssembly().GetTypes();

            foreach (Type t in types)
            {
                if (t.GetInterfaces().Contains(typeof(IConnStateHandler)))
                {
                    //load connected states
                    Console.WriteLine("Requesting Connected State Handler -> " + t.Name);
                    var handler = (IConnStateHandler)serviceProvider.GetRequiredService(t);
                    connectedStateHandlers[handler.ConnectedState] = handler;
                }
            }
            foreach (Type t in types)
            {
                if (t.GetInterfaces().Contains(typeof(IMUDCommand)))
                {
                    //load connected states
                    Console.WriteLine("Requesting Command -> " + t.Name);
                    var command = (IMUDCommand)serviceProvider.GetRequiredService(t);
                    connectedStateHandlers[command.ConnectedState].AddCommand(command);
                }
            }
            foreach (KeyValuePair<CONN_STATE, IConnStateHandler> handler in connectedStateHandlers)
            {
                handler.Value.Initialize();
            }
        }

        public static void UseServer(this IServiceProvider serviceProvider)
        {
            using (var TelnetService = serviceProvider.GetService<MUDServer>())
            {
                //todo: find a better way to get this instantiation :(
                var EventHandler = serviceProvider.GetService<TelnetEventHandler>();
                TelnetService.StartServer();
                string closeString = "Z";
                string stringToCompare = "";
                while (stringToCompare != closeString)
                {
                    stringToCompare = Console.ReadLine().ToUpper();
                }
                TelnetService.StopServer();
            }
        }
        public static void AddAutoRegistration(this IServiceCollection services)
        {
            Type[] types = Assembly.GetEntryAssembly().GetTypes();
            foreach (Type t in types)
            {
                if (t.GetInterfaces().Contains(typeof(IConnStateHandler)))
                {
                    //load connected states
                    Console.WriteLine("Registering Connected State Handler -> " + t.Name);
                    //god forgive me
                    var a = typeof(ServiceCollectionServiceExtensions).GetMethods()
                        .Where(x => x.Name == "AddSingleton"
                        && x.GetParameters().Length == 1
                        && x.GetGenericArguments().Length == 1)
                        .FirstOrDefault()
                        .MakeGenericMethod(t).Invoke(services, new[] { services });
                } else if(t.GetInterfaces().Contains(typeof(IMUDCommand)))
                {
                    Console.WriteLine("Registering Command -> " + t.Name);
                    //god forgive me
                    var a = typeof(ServiceCollectionServiceExtensions).GetMethods()
                        .Where(x => x.Name == "AddSingleton"
                        && x.GetParameters().Length == 1
                        && x.GetGenericArguments().Length == 1)
                        .FirstOrDefault()
                        .MakeGenericMethod(t).Invoke(services, new[] { services });
                }
            }
        }
        public static string ReadWord(this String data, ref Int32 position)
        {
            int start = position;
            int end = position;
            while (position < data.Length && (data[position] != ' '))
            {
                position++;
            }
            end = position;
            position++;
            if (start != end)
                return data.Substring(start, end - start);
            return "";
        }
        
    }
}
