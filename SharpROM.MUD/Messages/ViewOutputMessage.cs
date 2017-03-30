using SharpROM.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpROM.MUD.Messages
{
    public class ViewOutputMessage : EventMessage
    {
        public string ViewOutput { get; set; }
        public int SessionId { get; set; }
        public ViewOutputMessage() : base()
        {
        }
    }
}
