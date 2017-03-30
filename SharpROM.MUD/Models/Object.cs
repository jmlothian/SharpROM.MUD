using System;
using System.Collections.Generic;
using System.Text;

namespace SharpROM.MUD.Models
{
    public class Object : Entity
    {
        public List<string> ExtraFlags { get; set; } = new List<string>();
        public List<string> WearFlags { get; set; } = new List<string>();
        public int Level { get; set; } = 0;
        public int Weight { get; set; } = 0;
        public int Cost { get; set; } = 0;
        public int Condition { get; set; } = 100;
        public string Clan { get; set; } = "";
        public Dictionary<string, string> ExtraDescriptions = new Dictionary<string, string>();
        //olc src shows that this ranges from 0-10,000
        // this is a 1 out of chance out of the LoadChance value - for example, if load == 150, then there is a 1/150 chance it will load)
        //typically unset, 0 means "always"
        public int LoadChance { get; set; } = 0;
        public List<ObjectApply> Applies = new List<ObjectApply>();

    }
}
