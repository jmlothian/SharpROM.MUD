using SharpROM.MUD.Abstract;
using SharpROM.MUD.ConnectedStates;
using SharpROM.MUD.Models;
using SharpROM.MUD.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpROM.MUD.Commands
{
    public class CommandProcessor
    {
        Dictionary<string, IMUDCommand> CommandLookup { get; set; } = new Dictionary<string, IMUDCommand>();
        List<IMUDCommand> Commands { get; set; }

        public void DoCommand(string input, Entity entity, UserInfo user)
        {
            int pos = 0;
            string command = input.ReadWord(ref pos);
            //lookup command
            if (CommandLookup.ContainsKey(command))
            {
                List<string> args = new List<string>();
                //parse args
                if (pos < input.Length)
                    args = input.Substring(pos).Split(' ').ToList(); 
                //check min/max
                CommandLookup[command].HandleInput(args, entity, user);
            } else
            {
                //error, no command found
            }
        }
        public CommandProcessor(List<IMUDCommand> commands)
        {
            Initialize(commands);
        }

        public void Initialize(List<IMUDCommand> commands)
        {
            List<string> Aliases = new List<string>();
            Commands = commands.OrderBy(o => o.Command).ToList();

            //build a lookup of commands that allows partial entry
            //yes, we could handle this during command processing, but memory is cheaper than CPU cycles.
            //gather aliases
            foreach (IMUDCommand cmd in Commands)
            {
                foreach (string a in cmd.Aliases)
                {
                    Aliases.Add(a.ToLower());
                }
            }
            //add commands one at a time
            foreach (IMUDCommand cmd in Commands)
            {
                CommandLookup[cmd.Command.ToLower()] = cmd;
                //now, we add short versions
                for (int i = cmd.Command.Length - 1; i > 0; i--)
                {
                    string shortCmd = cmd.Command.Substring(0, i);
                    if (!Aliases.Contains(shortCmd) && !CommandLookup.ContainsKey(shortCmd))
                    {
                        CommandLookup[shortCmd] = cmd;
                    }
                    else
                    {
                        //break loop gracefully
                        i = 0;
                    }
                }
                foreach (string a in cmd.Aliases)
                {
                    CommandLookup[a] = cmd;
                }
            }
        }
    }
}
