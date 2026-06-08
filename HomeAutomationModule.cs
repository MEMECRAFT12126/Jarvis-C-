using System;
using System.Collections.Generic;

namespace Jarvis.Modules
{
    public class HomeAutomationModule : IModule
    {
        private Action<string> _transcriptUpdater;

        public string Name => "Home Automation Module";

        public void Initialize(Action<string> statusUpdater, Action<string> transcriptUpdater)
        {
            _transcriptUpdater = transcriptUpdater;
            statusUpdater?.Invoke("🏠 Módulo de domótica cargado - Compatible con Arduino/Raspberry Pi");
        }

        public bool ProcessCommand(string command)
        {
            if (command.Contains("luces") || command.Contains("encender") || command.Contains("apagar"))
            {
                if (command.Contains("luces"))
                {
                    _transcriptUpdater?.Invoke("💡 Comando de luces recibido. Conecta Arduino para control real");
                }
                return true;
            }
            return false;
        }

        public Dictionary<string, Action<string>> GetCommands() => new Dictionary<string, Action<string>>();
    }
}