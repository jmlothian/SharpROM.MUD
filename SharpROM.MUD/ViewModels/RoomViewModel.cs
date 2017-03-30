using SharpROM.MUD.Abstract;
using SharpROM.MUD.Presenters;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpROM.MUD.ViewModels
{
    public class RoomViewModel : IViewModel 
    {
        public string Name { get; set; } = "";
        public string LongDescription { get; set; } = "";
        public List<object> Models { get; set; }
    }
}
