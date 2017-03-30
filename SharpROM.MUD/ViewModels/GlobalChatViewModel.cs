using System;
using System.Collections.Generic;
using System.Text;

namespace SharpROM.MUD.ViewModels
{
    public class GlobalChatViewModel
    {
        public string ChannelName { get; set; } = "";
        public string Color { get; set; } = "";
        public string Speakder { get; set; } = "";
        public string Message { get; set; } = "";
    }
}
