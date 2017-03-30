using SharpROM.MUD.Models;
using SharpROM.MUD.MVPVM;
using SharpROM.Net.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpROM.MUD
{
    public enum CONN_STATE {
        CONN_STATE_NPC,
        CONN_STATE_NONE,
        CONN_STATE_CONNECTED,
        CONN_STATE_ASK_NAME,
        CONN_STATE_PLAYING,
        CONN_STATE_DISCONNECTED
    }


    public class UserInfo
    {
        public object SyncRoot = new object();
        public int SessionID { get; set; }
        public IDescriptorData descriptorData { get; set; }
        public CONN_STATE ConnectedState { get; set; } = CONN_STATE.CONN_STATE_NONE;
        public string Username { get; set; } = "";
        public string Color { get; set; } = "";
        public ViewOutputInformation OutputViews { get; set; } = null;
        public Entity EntityInfo { get; set; } = new Entity();
    }
}
