using SharpROM.Net.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using SharpROM.Events.Abstract;
using SharpROM.Events.Messages;
using SharpROM.MUD.Commands;
using SharpROM.MUD.Abstract;

namespace SharpROM.MUD.ConnectedStates
{
    public class ConnStateConnected : IConnStateHandler
    {
        public IEventRoutingService EventRoutingService { get ; set; }
        public GameDataManager DataManager { get; set; }
        public CommandProcessor CmdProcessor { get; set; }
        public CONN_STATE ConnectedState { get => CONN_STATE.CONN_STATE_CONNECTED; }
        private List<IMUDCommand> Commands = new List<IMUDCommand>();
        public void Initialize() { }

        public void AddCommand(IMUDCommand command)
        {
            Commands.Add(command);
        }


        public ConnStateConnected(IEventRoutingService eventRoutingService, GameDataManager dataManager)
        {
            EventRoutingService = eventRoutingService;
            DataManager = dataManager;
        }
        public void HandleInput(string input, UserInfo user)
        {
            //prompt - check user
            //todo: there's a chance that input can get here twice (or n times) before the state is actually updated
            // there needs to be some kind of command queueing or input lock to wait
            if(DataManager.UsernameAvailable(input))
            {
                DataManager.RegisterUsername(input, user.SessionID);
                DataManager.SetUserState(user.SessionID, CONN_STATE.CONN_STATE_PLAYING);
                OutMessage message = new OutMessage();
                message.Message = "Thanks " + input + " !\r\nYou are now connected\r\n";
                message.Target = user.descriptorData;
                EventRoutingService.QueueEvent(message);
            }
            else
            {
                OutMessage message = new OutMessage();
                message.Message = "That username isn't available. Please try another\r\n";
                message.Target = user.descriptorData;
                EventRoutingService.QueueEvent(message);
            }

        }
    }
}
