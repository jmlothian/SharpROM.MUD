using System;
using System.Collections.Generic;
using System.Text;

namespace SharpROM.MUD.Models
{
    public class ExitInfo
    {
        public string Dir { get; set; } = "";
        public string OutVNUM { get; set; } = "";
        public string KeyVNUM { get; set; } = "";
        public string LockInfo { get; set; } = "";
        public bool Open { get; set; } = true;
    }
    public class ExitCollection
    {
        public ExitInfo North { get; set; } = null;
        public ExitInfo South { get; set; } = null;
        public ExitInfo East { get; set; } = null;
        public ExitInfo West { get; set; } = null;
        public ExitInfo Up { get; set; } = null;
        public ExitInfo Down { get; set; } = null;

    }
    public class Room
    {
        public string VNUM { get; set; } = "";
        public string Name { get; set; } = "";
        public string LongDescription { get; set; } = "";
        public string Clan { get; set; } = "";
        public ExitCollection Exits { get; set; } = new ExitCollection();

    }
}
