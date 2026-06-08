using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Jarvis.Modules
{
    public class MusicModule : IModule
    {
        private Action<string> _transcriptUpdater;

        public string Name => "Music Module";

        public void Initialize(Action<string> statusUpdater, Action<string> transcriptUpdater)
        {
            _transcriptUpdater = transcriptUpdater;
            statusUpdater?.Invoke("🎵 Módulo de música cargado");
        }

        public bool ProcessCommand(string command)
        {
            if (command.StartsWith("reproduce ") || command.StartsWith("pon ") || command.StartsWith("play "))
            {
                string song = command.Substring(command.IndexOf(' ') + 1);
                string url = $"https://www.youtube.com/results?search_query={Uri.EscapeDataString(song)}";
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                _transcriptUpdater?.Invoke($"🎵 Reproduciendo: {song}");
                return true;
            }
            return false;
        }

        public Dictionary<string, Action<string>> GetCommands() => new Dictionary<string, Action<string>>();
    }
}