using System;
using System.Collections.Generic;
using System.Text;

namespace SharpROM.MUD.Models
{
    public class RoomReset
    {
        public string ResetType { get; set; } = "";
        public int arg1 { get; set; } = -1;
        public int arg2 { get; set; } = -1;
        public int arg3 { get; set; } = -1;
        public int arg4 { get; set; } = -1;
    }
}
