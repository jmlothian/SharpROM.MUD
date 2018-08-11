using SharpROM.Events.Abstract;
using SharpROM.MUD.Models;
using SharpROM.Net.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpROM.MUD.Abstract
{
    public interface IMUDCommand
    {
        IEventRoutingService EventRoutingService { get; set; }
        CONN_STATE ConnectedState { get; }
        List<string> Aliases { get; }
        string Command { get; }
        int MinArgs { get; }
        int MaxArgs { get; }
        bool LastArgIsText { get; }
        string Helptext { get; }
        void HandleInput(List<string> args, Entity entity, UserInfo user=null);
    }
}
