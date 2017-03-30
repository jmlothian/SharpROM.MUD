using System;
using System.Collections.Generic;
using System.Text;
using SharpROM.Events.Abstract;
using SharpROM.Net.Abstract;
using SharpROM.Net.Telnet;
using SharpROM.Events.Messages;
using SharpROM.MUD.Models;
using System.IO;
using Newtonsoft.Json;
using SharpROM.Core;
using SharpROM.MUD.Messages;

namespace SharpROM.MUD
{
    public class MUDServer: ServerObject, IDisposable
    {
        private GameDataManager DataManager { get; set; }
        private ViewController ViewController { get; set; }
        public MUDServer(IEventRoutingService eventRoutingService, ISocketListener socketListener, 
            GameDataManager dataManager, ViewController viewController) : base()
        {
            EventRoutingService = eventRoutingService;
            SocketListener = socketListener;
            DataManager = dataManager;
            ViewController = viewController;
            EventRoutingService.RegisterHandler(this, typeof(ViewOutputMessage));
        }

        public virtual void StartServer()
        {
            //Log.Info("Starting SharpROM Server!");
            DataManager.LoadAreas();
            // TODO - start services
            EventRoutingService.ProcessQueues();
            //Log.Info("SharpROM Server is now running!");
            _isRunning = true;
        }


        // Injected services.
        protected IEventRoutingService EventRoutingService { get; set; }
        protected ISocketListener SocketListener { get; set; }

        // Other members.
        //private List<IEventManager> EventManagers { get; set; }

        protected bool _isRunning;
        public virtual void StopServer()
        {
            //Log.Info("Stopping SharpROM Server!");

            // TODO - stop services and clean up

            //Log.Info("SharpROM Server is no longer running!");
            _isRunning = false;
        }

        public virtual bool IsRunning()
        {
            return _isRunning;
        }

        public virtual void Dispose()
        {
            EventRoutingService.Dispose();
        }
        public override bool HandleEvent(IEventMessage Message)
        {
            bool continueProcessing = true;
            if(Message is ViewOutputMessage)
            {

                DataManager.OutputToUser(((ViewOutputMessage)Message).ViewOutput, ((ViewOutputMessage)Message).SessionId);
                continueProcessing = false;
            }
            return continueProcessing;
        }

    }
}
