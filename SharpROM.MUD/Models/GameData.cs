using System;
using System.Collections.Generic;
using System.Text;

namespace SharpROM.MUD.Models
{
    public class GameData
    {
        public Dictionary<int, UserInfo> Users = new Dictionary<int, UserInfo>();
        public Dictionary<string, Area> Areas = new Dictionary<string, Area>();
        public Dictionary<string, Area> RoomAreaIndex = new Dictionary<string, Area>();
        public Dictionary<string, Room> RoomIndex = new Dictionary<string, Room>();
    }
}
