using System;
using System.Collections.Generic;
using System.Text;

namespace SharpROM.MUD.Models
{
    public class Area
    {
        public string Name { get; set; } = "";
        public List<Object> Objects = new List<Object>();
        public List<Mobile> Mobiles = new List<Mobile>();
        public List<Room> Rooms = new List<Room>();
        public List<string> Specials = new List<string>();
        public List<RoomReset> Resets = new List<RoomReset>();
    }
}
