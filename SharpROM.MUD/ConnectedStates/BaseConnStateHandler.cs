using System;
using System.Collections.Generic;
using System.Text;
using SharpROM.Events.Abstract;
using SharpROM.MUD.Commands;
using SharpROM.Net.Abstract;
using SharpROM.MUD.Abstract;

namespace SharpROM.MUD.ConnectedStates
{
    public class BaseConnStateHandler : IConnStateHandler
    {
        public GameDataManager DataManager { get; set; }

        public IEventRoutingService EventRoutingService { get; set; }
        public virtual CONN_STATE ConnectedState { get => CONN_STATE.CONN_STATE_NONE; }
        private List<IMUDCommand> Commands = new List<IMUDCommand>();
        protected CommandProcessor CmdProcessor { get; set; }
        public void AddCommand(IMUDCommand command)
        {
            Commands.Add(command);
        }
        public virtual void Initialize()
        {
            CmdProcessor = new CommandProcessor(Commands);
        }

        public BaseConnStateHandler(IEventRoutingService eventRoutingService, GameDataManager dataManager)
        {
            EventRoutingService = eventRoutingService;
            DataManager = dataManager;
        }

        public virtual void HandleInput(string input, UserInfo user)
        {
        }
    }
}
