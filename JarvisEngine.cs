using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jarvis.Modules;

namespace Jarvis.Core
{
    public class JarvisEngine
    {
        private VoiceEngine _voice;
        private CommandParser _parser;
        private List<IModule> _modules;
        private Action<string> _statusUpdater;
        private Action<string> _transcriptUpdater;
        private Action<string> _voiceStatusUpdater;
        private bool _isListening = false;

        public bool IsListening => _isListening;

        // Constructor CORREGIDO - ahora acepta 3 argumentos
        public JarvisEngine(Action<string> statusUpdater, Action<string> transcriptUpdater, Action<string> voiceStatusUpdater)
        {
            _statusUpdater = statusUpdater;
            _transcriptUpdater = transcriptUpdater;
            _voiceStatusUpdater = voiceStatusUpdater;
            _modules = new List<IModule>();
            _parser = new CommandParser();
            _voice = new VoiceEngine(transcriptUpdater, voiceStatusUpdater);
        }

        public void Initialize()
        {
            _voice.Initialize();
            _voice.OnSpeechRecognized += OnSpeechRecognized;
            _voice.OnListeningStatusChanged += (listening) => _isListening = listening;
            _statusUpdater?.Invoke("JARVIS inicializado correctamente");
        }

        public void LoadModule(IModule module)
        {
            _modules.Add(module);
            module.Initialize(_statusUpdater, _transcriptUpdater);
            _statusUpdater?.Invoke($"✅ Módulo cargado: {module.Name}");

            foreach (var cmd in module.GetCommands())
            {
                _parser.RegisterCommand(cmd.Key, cmd.Value);
            }
        }

        private void OnSpeechRecognized(string text)
        {
            _transcriptUpdater?.Invoke($"🎤 Tú: {text}");
            ProcessCommand(text);
        }

        public void ProcessCommand(string commandText)
        {
            if (string.IsNullOrWhiteSpace(commandText))
                return;

            Task.Run(() =>
            {
                try
                {
                    bool executed = _parser.ExecuteCommand(commandText.ToLower());

                    if (!executed)
                    {
                        foreach (var module in _modules)
                        {
                            if (module.ProcessCommand(commandText))
                            {
                                executed = true;
                                break;
                            }
                        }
                    }

                    if (!executed)
                    {
                        string response = $"No entiendo: {commandText}";
                        _voice.Speak(response);
                        _transcriptUpdater?.Invoke($"❌ {response}");
                    }
                }
                catch (Exception ex)
                {
                    _statusUpdater?.Invoke($"Error: {ex.Message}");
                    _voice.Speak("Ocurrió un error");
                }
            });
        }

        public void StartContinuousListening() => _voice.StartContinuousRecognition();
        public void StopContinuousListening() => _voice.StopContinuousRecognition();
        public void TestMicrophone() => _voice.TestMicrophone();
        public void Shutdown() => _voice?.Dispose();
    }
}