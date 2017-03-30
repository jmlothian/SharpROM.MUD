using System;
using System.Collections.Generic;
using System.Text;
using SharpROM.Events.Abstract;
using SharpROM.Net.Abstract;
using SharpROM.MUD.Messages;
using SharpROM.MUD.ViewModels;
using SharpROM.MUD.Abstract;
using SharpROM.MUD.Presenters;

namespace SharpROM.MUD.Commands
{
    public class LookCmd : IMUDCommand
    {
        public IEventRoutingService EventRoutingService { get; set; }
        public CONN_STATE ConnectedState => CONN_STATE.CONN_STATE_PLAYING;
        public List<string> Aliases => new List<string> { "l" };

        public string Command => "look";

        public int MinArgs => 0;

        public int MaxArgs => 1;

        public bool LastArgIsText => false;

        public string Helptext => "No help here.";
        private GameDataManager DataManager { get; set; }

        public LookCmd(IEventRoutingService evtRoutingService, GameDataManager dataManager)
        {
            EventRoutingService = evtRoutingService;
            DataManager = dataManager;
        }

        public void HandleInput(List<string> args, UserInfo user)
        {
            if(args.Count == 1)
            {
                //1 arg, we're trying to look at an object, person, or extra desc in the room
                //
            } else
            {
                //no args, look at the room
                var RoomVNUM = user.EntityInfo.RoomVNUM;
                RoomPresenter p = new RoomPresenter();
                RoomViewModel vm = p.CreateViewModel(new List<object> { DataManager.GetRoom(user.EntityInfo.RoomVNUM) });
                DisplayViewMessage ViewInfo = new DisplayViewMessage() { ToUserSession = user.SessionID, ViewName="Room", ViewModel=vm };
                EventRoutingService.QueueEvent(ViewInfo);
            }
        }
    }
}
