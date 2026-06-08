using System;
using System.Collections.Generic;
using System.Data;

namespace Jarvis.Modules
{
    public class EducationModule : IModule
    {
        private Action<string> _transcriptUpdater;

        public string Name => "Education Module";

        public void Initialize(Action<string> statusUpdater, Action<string> transcriptUpdater)
        {
            _transcriptUpdater = transcriptUpdater;
            statusUpdater?.Invoke("📚 Módulo educativo cargado");
        }

        public bool ProcessCommand(string command)
        {
            if (command.Contains("cuánto es") || command.Contains("calcula"))
            {
                var parts = command.Split(' ');
                foreach (var part in parts)
                {
                    if (part.Contains("+") || part.Contains("-") || part.Contains("*") || part.Contains("/"))
                    {
                        try
                        {
                            var result = new DataTable().Compute(part, null);
                            _transcriptUpdater?.Invoke($"🧮 El resultado es: {result}");
                            return true;
                        }
                        catch { }
                    }
                }
            }
            return false;
        }

        public Dictionary<string, Action<string>> GetCommands() => new Dictionary<string, Action<string>>();
    }
}