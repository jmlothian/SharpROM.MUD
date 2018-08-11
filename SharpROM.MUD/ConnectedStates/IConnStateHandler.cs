using SharpROM.Events.Abstract;
using SharpROM.MUD.Abstract;
using SharpROM.MUD.Commands;
using SharpROM.Net.Abstract;
using System.Collections.Generic;

namespace SharpROM.MUD.ConnectedStates
{
    public interface IConnStateHandler
    {
        IEventRoutingService EventRoutingService { get; set; }
        GameDataManager DataManager { get; set; }

        void HandleInput(string input, UserInfo user);
        CommandProcessor CmdProcessor { get; set; }
        CONN_STATE ConnectedState { get; }
        void AddCommand(IMUDCommand command);
        void Initialize();

    }
}