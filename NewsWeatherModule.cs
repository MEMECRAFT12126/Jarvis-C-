using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

namespace Jarvis.Modules
{
    public class NewsWeatherModule : IModule
    {
        private Action<string> _transcriptUpdater;
        private HttpClient _httpClient;

        public string Name => "News & Weather Module";

        public void Initialize(Action<string> statusUpdater, Action<string> transcriptUpdater)
        {
            _transcriptUpdater = transcriptUpdater;
            _httpClient = new HttpClient();
            statusUpdater?.Invoke("🌤️ Módulo de noticias y clima cargado");
        }

        public bool ProcessCommand(string command)
        {
            if (command.Contains("clima") || command.Contains("tiempo"))
            {
                _transcriptUpdater?.Invoke("🌡️ Simulando clima: 22°C, soleado");
                return true;
            }

            if (command.Contains("noticias"))
            {
                _transcriptUpdater?.Invoke("📰 Noticias de hoy: [Simulación] - Para noticias reales, agrega API key");
                return true;
            }

            return false;
        }

        public Dictionary<string, Action<string>> GetCommands() => new Dictionary<string, Action<string>>();
    }
}