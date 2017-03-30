using System;
using System.Collections.Generic;
using System.Text;

namespace SharpROM.MUD.Models
{
    public class Entity
    {
        public string VNUM { get; set; } = "0";
        public string Name { get; set; } = "";
        public string ShortDescription { get; set; } = "";
        public string LongDescription { get; set; } = "";
        public List<string> Keywords { get; set; } = new List<string>();
        public string RoomVNUM { get; set; } = "204";
    }
}
