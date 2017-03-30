using SharpROM.Net.Telnet;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using SharpROM.Events.Abstract;
using SharpROM.Events.Messages.Telnet;
using SharpROM.MUD.ConnectedStates;
using SharpROM.Events.Messages;
using SharpROM.Net.Messages;
using System.Reflection;
using System.Linq;
using SharpROM.MUD.Commands;
using SharpROM.MUD.Abstract;

namespace SharpROM.MUD
{
    public class MUDMainEventHandler : TelnetEventHandler
    {
       // MUDServer Server { get; set; }
        private GameDataManager DataManager { get; set; }
        Dictionary<CONN_STATE, IConnStateHandler> StateHandlers { get; set; }
        public MUDMainEventHandler(IEventRoutingService evtRoutingService, 
            TelOptManagement telOpts, ILogger<TelnetEventHandler> logger,
            //MUDServer server,
            GameDataManager dataManager,
            Dictionary<CONN_STATE, IConnStateHandler> stateHandlers
            ) 
            : base(evtRoutingService, telOpts, logger)
        {
            StateHandlers = stateHandlers;
            //Server = server;
            DataManager = dataManager;
            //auto registration of connected states
            //todo: search statically linked assemblies
            //var runtimeId = RuntimeEnvironment.GetRuntimeIdentifier();
            //var assemblies = DependencyContext.Default.GetRuntimeAssemblyNames(runtimeId);

            //Type[] types = Assembly.GetEntryAssembly().GetTypes();

            /*
            List<IMUDCommand> commands = new List<IMUDCommand>();
            foreach(Type t in types)
            {
                if(t.GetInterfaces().Contains(typeof(IConnStateHandler)))
                {
                    
                    //load connected states
                    Console.WriteLine("Registering Connected State Handler -> " + t.Name);
                    IConnStateHandler connHandler = (IConnStateHandler)Activator.CreateInstance(t, evtRoutingService, server);
                    StateHandlers[connHandler.ConnectedState] = connHandler;
                    
                }
                if(t.GetInterfaces().Contains(typeof(IMUDCommand)))
                {
                    //handle these after connstate
                    commands.Add((IMUDCommand)Activator.CreateInstance(t, evtRoutingService));
                }
            }
            foreach(IMUDCommand cmd in commands)
            {
                if(StateHandlers.ContainsKey(cmd.ConnectedState))
                {
                    StateHandlers[cmd.ConnectedState].AddCommand(cmd);
                }
            }
            
            foreach(KeyValuePair<CONN_STATE, IConnStateHandler> handler in StateHandlers)
            {
                handler.Value.Initialize();
            }*/

        }
        public override bool HandleEvent(IEventMessage Message)
        {
            if(Message is ConnectUserMessage)
            {

                DataManager.RegisterUser(((ConnectUserMessage)Message).SessionID,
                    ((ConnectUserMessage)Message).Descriptor);
                OutMessage message = new OutMessage();
                message.Message = "Oh hai!  Plz can haz username?";
                message.Target = ((ConnectUserMessage)Message).Descriptor;
                eventRoutingService.QueueEvent(message);

            }
            else if (Message is DisconnectUserMessage)
            {
                DataManager.UnregisterUser(((DisconnectUserMessage)Message).SessionID);
            }
            return base.HandleEvent(Message);
        }
        protected override void HandleTextInput(TelnetTextInput Message)
        {
            CONN_STATE userState = DataManager.GetConnectedState(Message.SessionID);
            if (StateHandlers.ContainsKey(userState))
            {
                UserInfo user = DataManager.GetUser(Message.SessionID);
                StateHandlers[userState].HandleInput(Message.Message, user);
            } else
            {
                OutMessage message = new OutMessage();
                message.Message = "You've entered an unknown state - congrats!";
                message.Target = Message.Descriptor;
                eventRoutingService.QueueEvent(message);
            }
            //base.HandleTextInput(Message);
        }
    }
}
