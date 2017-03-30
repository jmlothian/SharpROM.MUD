using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SharpROM.Events;
using SharpROM.Events.Abstract;
using SharpROM.MUD.Models;
using SharpROM.Net;
using SharpROM.Net.Abstract;
using SharpROM.Net.Telnet;
using System;
using System.Linq;
using System.Reflection;

namespace SharpROM.MUD
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerBuilder<Startup> serverBuilder = new ServerBuilder<Startup>();
            serverBuilder.Run();
        }
    }
}