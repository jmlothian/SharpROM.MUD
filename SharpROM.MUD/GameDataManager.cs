using Newtonsoft.Json;
using SharpROM.Events;
using SharpROM.Events.Abstract;
using SharpROM.Events.Messages;
using SharpROM.MUD.Models;
using SharpROM.Net.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharpROM.MUD
{
    public class GameDataManager
    {
        protected IEventRoutingService EventRoutingService { get; set; }

        private GameData Data { get; set; }
        public GameDataManager(GameData data, IEventRoutingService eventRoutingService)
        {
            Data = data;
            EventRoutingService = eventRoutingService;

        }
        public void SetUserState(int SessionID, CONN_STATE state)
        {
            lock (Data.Users)
            {
                lock (Data.Users[SessionID].SyncRoot)
                {
                    Data.Users[SessionID].ConnectedState = state;
                    if (state == CONN_STATE.CONN_STATE_CONNECTED)
                    {
                        //session Ids can be recycled, so clear info on new connections, rather than re-allocating
                        //this needs to be done on disconnect, to free up user names
                        Data.Users[SessionID].Username = "";
                        Data.Users[SessionID].Color = "";
                    }
                }
            }
        }

        public void LoadAreas()
        {
            foreach (string f in Directory.GetFiles(@".\Areas", "*.json"))
            {
                Console.WriteLine("Loading Area -> " + f);
                Area a = JsonConvert.DeserializeObject<Area>(File.ReadAllText(f));
                Data.Areas[a.Name] = a;
                Console.WriteLine("\t... indexing...");
                foreach (Room r in a.Rooms)
                {
                    Data.RoomAreaIndex[r.VNUM] = a;
                    Data.RoomIndex[r.VNUM] = r;
                }
            }
        }
        public CONN_STATE GetConnectedState(int SessionID)
        {
            return Data.Users[SessionID].ConnectedState;
        }
        public bool UsernameAvailable(string username)
        {
            bool avail = true;
            lock (Data.Users)
            {
                foreach (KeyValuePair<int, UserInfo> user in Data.Users)
                {
                    if (user.Value.Username.ToLower() == username.ToLower())
                    {
                        avail = false;
                        break;
                    }
                }
            }
            return avail;
        }
        public void RegisterUser(int sessionId, IDescriptorData descriptor)
        {
            UserInfo ui = new UserInfo();
            lock (Data.Users)
            {
                lock (ui.SyncRoot)
                {
                    Data.Users[sessionId] = new UserInfo();
                    Data.Users[sessionId].SessionID = sessionId;
                    Data.Users[sessionId].Username = "";
                    Data.Users[sessionId].descriptorData = descriptor;
                    Data.Users[sessionId].ConnectedState = CONN_STATE.CONN_STATE_CONNECTED;
                }
            }
        }
        public void UnregisterUser(int sessionId)
        {
            lock (Data.Users)
            {
                lock (Data.Users[sessionId].SyncRoot)
                {
                    Data.Users[sessionId].Username = "";
                    Data.Users[sessionId].Color = "";
                    Data.Users[sessionId].descriptorData = null;
                    Data.Users[sessionId].ConnectedState = CONN_STATE.CONN_STATE_DISCONNECTED;
                }
            }
        }
        public void RegisterUsername(string username, int sessionId)
        {
            lock (Data.Users)
            {
                lock (Data.Users[sessionId].SyncRoot)
                {
                    Data.Users[sessionId].Username = username;
                }
            }
        }
        public void SetUserColor(int sessionId, string color)
        {
            Data.Users[sessionId].Color = color;
        }
        public string GetUserColor(int sessionId)
        {
            char esc = (char)27;
            string color = Data.Users[sessionId].Color;
            switch (color)
            {
                case "red":
                    color = "1;31";
                    break;
                case "blue":
                    color = "1;34";
                    break;
                case "yellow":
                    color = "1;33";
                    break;
                case "green":
                    color = "1;32";
                    break;
                case "white":
                    color = "1;37";
                    break;
            }
            return esc + "[" + color + "m";
            //return "(" + Users[sessionId].Color + ")";
        }
        public string GetUsername(int sessionId)
        {
            return Data.Users[sessionId].Username;
        }
        public void OutputToUser(string output, int sessionId)
        {
            if(Data.Users.ContainsKey(sessionId) 
                && Data.Users[sessionId].ConnectedState == CONN_STATE.CONN_STATE_PLAYING)
            {
                OutMessage message = new OutMessage();
                message.Message = "\r\n" + output
                    + (char)27 + "[0m" //append color reset 
                    + "\r\n";          //cleanup newline at the end

                message.Target = Data.Users[sessionId].descriptorData;
                EventRoutingService.QueueEvent(message);
            }
        }
        public Room GetRoom(string vnum)
        {
            return Data.RoomIndex[vnum];
        }
        public UserInfo GetUser(int sessionId)
        {
            return Data.Users[sessionId];
        }
        public ExitInfo GetExitFromDir(string currentVNUM, string direction)
        {
            ExitInfo exit = null;
            switch(direction)
            {
                case "north":
                    exit = Data.RoomIndex[currentVNUM].Exits.North;
                    break;
                case "east":
                    exit = Data.RoomIndex[currentVNUM].Exits.East;
                    break;
                case "south":
                    exit = Data.RoomIndex[currentVNUM].Exits.South;
                    break;
                case "west":
                    exit = Data.RoomIndex[currentVNUM].Exits.West;
                    break;
                case "up":
                    exit = Data.RoomIndex[currentVNUM].Exits.Up;
                    break;
                case "down":
                    exit = Data.RoomIndex[currentVNUM].Exits.Down;
                    break;
            }
            return exit;
        }
        public void BroadcastToChat(string mesg)
        {
            foreach (KeyValuePair<int, UserInfo> user in Data.Users)
            {
                if (user.Value.ConnectedState == CONN_STATE.CONN_STATE_PLAYING)
                {
                    OutMessage message = new OutMessage();
                    message.Message = mesg
                        + (char)27 + "[0m"; //append color reset
                    message.Target = user.Value.descriptorData;
                    EventRoutingService.QueueEvent(message);
                }
            }
        }

    }
}
