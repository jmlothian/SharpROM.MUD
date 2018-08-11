using SharpROM.Events.Abstract;
using SharpROM.Events.Messages;
using SharpROM.MUD.Abstract;
using SharpROM.MUD.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpROM.MUD.Commands.Move
{
    public class UpCmd : IMUDCommand
    {
        public IEventRoutingService EventRoutingService { get; set; }
        public CONN_STATE ConnectedState => CONN_STATE.CONN_STATE_PLAYING;
        public List<string> Aliases => new List<string> { "u" };

        public string Command => "up";

        public int MinArgs => 0;

        public int MaxArgs => 0;

        public bool LastArgIsText => false;

        public string Helptext => "No help here.";
        private GameDataManager DataManager { get; set; }
        private CommandProcessor CommandProc { get; set; }
        public UpCmd(IEventRoutingService evtRoutingService, GameDataManager dataManager, MUDMainEventHandler MudEvents)
        {
            EventRoutingService = evtRoutingService;
            DataManager = dataManager;
            CommandProc = MudEvents.PlayingCommands;
        }
        public void HandleInput(List<string> args, Entity entity, UserInfo user = null)
        {
            ExitInfo exit = DataManager.GetExitFromDir(user.EntityInfo.RoomVNUM, "up");
            if(exit == null || exit.Open == false)
            {
                OutMessage message = new OutMessage();
                message.Message = "You cannot go that way.";
                message.Target = user.descriptorData;
                EventRoutingService.QueueEvent(message);
            } else
            {
                user.EntityInfo.RoomVNUM = exit.OutVNUM;
                //run this command for the user, so they know what they've done
                CommandProc.DoCommand("look", entity, user);
            }
        }
    }
}
