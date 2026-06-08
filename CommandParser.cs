using System;
using System.Collections.Generic;

namespace Jarvis.Core
{
    public class CommandParser
    {
        private Dictionary<string, Action<string>> _commands;

        public CommandParser()
        {
            _commands = new Dictionary<string, Action<string>>();
        }

        public void RegisterCommand(string keyword, Action<string> action)
        {
            if (!_commands.ContainsKey(keyword.ToLower()))
            {
                _commands.Add(keyword.ToLower(), action);
            }
        }

        public bool ExecuteCommand(string fullCommand)
        {
            foreach (var cmd in _commands)
            {
                if (fullCommand.StartsWith(cmd.Key))
                {
                    string args = fullCommand.Substring(cmd.Key.Length).Trim();
                    cmd.Value(args);
                    return true;
                }
            }
            return false;
        }
    }
}