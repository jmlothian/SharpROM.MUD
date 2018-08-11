using SharpROM.Events.Abstract;
using SharpROM.Events.Messages;
using SharpROM.MUD.Commands;
using SharpROM.Net.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpROM.MUD.ConnectedStates
{
    public class ConnStatePlaying : BaseConnStateHandler
    {
        public override CONN_STATE ConnectedState { get => CONN_STATE.CONN_STATE_PLAYING; }

        public ConnStatePlaying(IEventRoutingService eventRoutingService, GameDataManager dataManager) :base(eventRoutingService, dataManager)
        {
            CmdProcessor = new CommandProcessor(new List<Abstract.IMUDCommand>());
        }

        public override void HandleInput(string input, UserInfo user)
        {
            
            CmdProcessor.DoCommand(input, user.EntityInfo, user);
            //check to see if the input is a backslash command
            /*
            if (input.Length > 0 && input[0] == '/')
            {
                HandleCommand(input.Substring(1), sessionID, descriptor);
            }
            else
            {
                string color = DataManager.GetUserColor(sessionID);
                string username = DataManager.GetUsername(sessionID);
                DataManager.BroadcastToChat(username + " " + color + ": " + input + "\r\n");
            }
            */

        }
        public string ReadWord(string data, ref Int32 position)
        {
            int start = position;
            int end = position;
            while (position < data.Length && (data[position] != ' '))
            {
                position++;
            }
            end = position;
            position++;
            if (start != end)
                return data.Substring(start, end - start);
            return "";
        }
        public void HandleCommand(string input, int sessionID, IDescriptorData descriptor)
        {
            int pos = 0;
            string command = ReadWord(input, ref pos);
            switch(command.ToLower())
            {
                case "color":
                    if (pos < input.Length)
                    {
                        string colorArg = ReadWord(input, ref pos);
                        switch (colorArg.ToLower())
                        {
                            case "red":
                            case "blue":
                            case "yellow":
                            case "green":
                            case "white":
                                {
                                    DataManager.SetUserColor(sessionID, colorArg.ToLower());
                                }
                                break;
                            default:
                                {
                                    OutMessage message = new OutMessage();
                                    message.Message = "Sorry, we didn't understand that color selection!\r\nSyntax: /color (red, blue, green, yellow, white):\r\n";
                                    message.Target = descriptor;
                                    EventRoutingService.QueueEvent(message);
                                }
                                break;
                        }
                    } else
                    {

                    }
                    break;
                default:
                    {
                        OutMessage message = new OutMessage();
                        message.Message = "Sorry, didn't understand that.\r\n Supported Commands:\r\n\r/color (red, blue, green, yellow, white)\r\n";
                        message.Target = descriptor;
                        EventRoutingService.QueueEvent(message);
                    }
                    break;
            }
        }

    }
}
