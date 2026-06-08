using System;
using System.Collections.Generic;
using System.IO;

namespace Jarvis.Modules
{
    public class ProductivityModule : IModule
    {
        private Action<string> _transcriptUpdater;
        private List<string> _reminders;

        public string Name => "Productivity Module";

        public void Initialize(Action<string> statusUpdater, Action<string> transcriptUpdater)
        {
            _transcriptUpdater = transcriptUpdater;
            _reminders = new List<string>();
            LoadReminders();
            statusUpdater?.Invoke("📋 Módulo de productividad cargado");
        }

        private void LoadReminders()
        {
            try
            {
                if (File.Exists("reminders.txt"))
                    _reminders.AddRange(File.ReadAllLines("reminders.txt"));
            }
            catch { }
        }

        private void SaveReminders()
        {
            File.WriteAllLines("reminders.txt", _reminders);
        }

        public bool ProcessCommand(string command)
        {
            if (command.StartsWith("recordar "))
            {
                string reminder = command.Substring(9);
                _reminders.Add(reminder);
                SaveReminders();
                _transcriptUpdater?.Invoke($"📝 Recordatorio guardado: {reminder}");
                return true;
            }

            if (command.Contains("recordatorios") || command.Contains("que tengo que hacer"))
            {
                if (_reminders.Count == 0)
                    _transcriptUpdater?.Invoke("📭 No tienes recordatorios pendientes");
                else
                    _transcriptUpdater?.Invoke($"📋 Tus recordatorios: {string.Join(", ", _reminders)}");
                return true;
            }

            return false;
        }

        public Dictionary<string, Action<string>> GetCommands() => new Dictionary<string, Action<string>>();
    }
}