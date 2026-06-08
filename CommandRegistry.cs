using System;
using System.Collections.Generic;
using Jarvis.Commands;

namespace Jarvis.Modules
{
    public class CommandRegistry
    {
        private Dictionary<string, Action<string>> _commands;
        private Action<string> _transcriptUpdater;
        private Action<string> _statusUpdater;
        private OpenCommand _openCommand;

        public CommandRegistry(Action<string> transcriptUpdater, Action<string> statusUpdater)
        {
            _transcriptUpdater = transcriptUpdater;
            _statusUpdater = statusUpdater;
            _commands = new Dictionary<string, Action<string>>();
            _openCommand = new OpenCommand(transcriptUpdater, statusUpdater);
        }

        public void RegisterDefaultCommands()
        {
            Register("abrir", (args) => _openCommand.Execute(args));
            Register("abre", (args) => _openCommand.Execute(args));
            Register("open", (args) => _openCommand.Execute(args));
            Register("apagar", (args) => Shutdown());
            Register("shutdown", (args) => Shutdown());
            Register("reiniciar", (args) => Restart());
            Register("restart", (args) => Restart());
            Register("salir", (args) => Exit());
            Register("exit", (args) => Exit());
        }

        public void Register(string keyword, Action<string> action)
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

        private void Shutdown()
        {
            _transcriptUpdater?.Invoke("🔌 Apagando el sistema en 10 segundos...");
            System.Diagnostics.Process.Start("shutdown", "/s /t 10");
        }

        private void Restart()
        {
            _transcriptUpdater?.Invoke("🔄 Reiniciando el sistema en 10 segundos...");
            System.Diagnostics.Process.Start("shutdown", "/r /t 10");
        }

        private void Exit()
        {
            _transcriptUpdater?.Invoke("👋 Hasta luego!");
            System.Windows.Forms.Application.Exit();
        }
    }
}