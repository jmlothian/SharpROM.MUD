using RazorLight;
using SharpROM.Core;
using SharpROM.Events.Abstract;
using SharpROM.MUD.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharpROM.MUD
{
    public class ViewController : ServerObject
    {
        GameDataManager DataManager { get; set; }
        //does it need gamedata?
        IRazorLightEngine ViewEngine { get; set; }
        IEventRoutingService EventRoutingService { get; set; }
        public ViewController(IEventRoutingService eventRoutingService, GameDataManager dataManager) :base()
        {
            ViewEngine = EngineFactory.CreatePhysical(Directory.GetCurrentDirectory() + @".\Views\");
            DataManager = dataManager;
            EventRoutingService = eventRoutingService;
            EventRoutingService.RegisterHandler(this, typeof(DisplayViewMessage));
            
        }

        public override bool HandleEvent(IEventMessage Message)
        {
            DisplayViewMessage viewMessage = (DisplayViewMessage)Message;
            string result = ViewEngine.Parse(viewMessage.ViewName + ".cshtml", viewMessage.ViewModel);
            ViewOutputMessage viewOutput = new ViewOutputMessage() { ViewOutput = result, SessionId=viewMessage.ToUserSession };
            //send message to user with this info, it should be output there somehow
            EventRoutingService.QueueEvent(viewOutput);
            return false; //continue processing
        }
    }
}
