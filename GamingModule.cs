using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Jarvis.Modules
{
    public class GamingModule : IModule
    {
        private Action<string> _transcriptUpdater;

        public string Name => "Gaming Module";

        public void Initialize(Action<string> statusUpdater, Action<string> transcriptUpdater)
        {
            _transcriptUpdater = transcriptUpdater;
            statusUpdater?.Invoke("🎮 Módulo de gaming cargado");
        }

        public bool ProcessCommand(string command)
        {
            if (command.Contains("abrir") && (command.Contains("free fire") || command.Contains("discord")))
            {
                if (command.Contains("discord"))
                {
                    Process.Start(new ProcessStartInfo("https://discord.com/app") { UseShellExecute = true });
                    _transcriptUpdater?.Invoke("🎮 Abriendo Discord");
                }
                else if (command.Contains("free fire"))
                {
                    _transcriptUpdater?.Invoke("🔥 Free Fire - Puedes abrirlo desde Gameloop o Bluestacks");
                }
                return true;
            }
            return false;
        }

        public Dictionary<string, Action<string>> GetCommands() => new Dictionary<string, Action<string>>();
    }
}