using SharpROM.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpROM.MUD.Messages
{
    public class DisplayViewMessage : EventMessage
    {
        public string ViewName { get; set; } = "";
        public object ViewModel { get; set; } = null;
        public int ToUserSession { get; set; } = -1;
    }
}
