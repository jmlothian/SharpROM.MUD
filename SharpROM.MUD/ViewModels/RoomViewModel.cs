using SharpROM.MUD.Abstract;
using SharpROM.MUD.Models;
using SharpROM.MUD.Presenters;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpROM.MUD.ViewModels
{
    public class ExitViewModel : IViewModel
    {
        public bool North { get; set; } = false;
        public bool East { get; set; } = false;
        public bool South { get; set; } = false;
        public bool West { get; set; } = false;
        public bool Up { get; set; } = false;
        public bool Down { get; set; } = false;
        public bool HasExit => North || South || East || West || Up || Down; //true if there is any available exit
        public List<object> Models { get; set; }

    }
    public class RoomViewModel : IViewModel
    {
        public string Name { get; set; } = "";
        public string LongDescription { get; set; } = "";
        public List<object> Models { get; set; }
        public ExitViewModel Exits { get; set; } = new ExitViewModel();
    }
}
