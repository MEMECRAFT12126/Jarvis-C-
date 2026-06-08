using System;
using System.Speech.Recognition;
using System.Speech.Synthesis;

namespace Jarvis.Core
{
    public class VoiceEngine : IDisposable
    {
        private SpeechRecognitionEngine _recognizer;
        private SpeechSynthesizer _synthesizer;
        private Action<string> _transcriptUpdater;
        private Action<string> _voiceStatusUpdater;
        private bool _continuousListening = false;

        public event Action<string> OnSpeechRecognized;
        public event Action<bool> OnListeningStatusChanged;

        public VoiceEngine(Action<string> transcriptUpdater, Action<string> voiceStatusUpdater)
        {
            _transcriptUpdater = transcriptUpdater;
            _voiceStatusUpdater = voiceStatusUpdater;
            _synthesizer = new SpeechSynthesizer();
            _synthesizer.SetOutputToDefaultAudioDevice();
            _synthesizer.Rate = 1;
            _synthesizer.Volume = 100;
        }

        public void Initialize()
        {
            try
            {
                _recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("es-ES"));
                _voiceStatusUpdater?.Invoke("✅ Reconocimiento de voz en Español");
            }
            catch
            {
                try
                {
                    _recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));
                    _voiceStatusUpdater?.Invoke("⚠️ Usando reconocimiento en Inglés");
                }
                catch (Exception ex)
                {
                    _voiceStatusUpdater?.Invoke($"❌ Error: {ex.Message}");
                    return;
                }
            }

            _recognizer.SetInputToDefaultAudioDevice();

            var commands = new Choices();
            commands.Add(new string[] {
                "abrir", "abre", "google", "youtube", "reproduce", "música",
                "clima", "noticias", "calcula", "cuánto es", "pregunta",
                "apagar", "reiniciar", "silenciar", "salir", "chrome"
            });

            var gb = new GrammarBuilder(commands);
            var grammar = new Grammar(gb);
            _recognizer.LoadGrammar(grammar);

            try { _recognizer.LoadGrammar(new DictationGrammar()); } catch { }

            _recognizer.SpeechRecognized += (s, e) => OnSpeechRecognized?.Invoke(e.Result.Text);
            _recognizer.SpeechRecognitionRejected += (s, e) =>
                _voiceStatusUpdater?.Invoke("🔇 No entendí, intenta de nuevo");
        }

        public void StartContinuousRecognition()
        {
            if (_recognizer != null && !_continuousListening)
            {
                _continuousListening = true;
                OnListeningStatusChanged?.Invoke(true);
                _recognizer.RecognizeAsync(RecognizeMode.Multiple);
                _voiceStatusUpdater?.Invoke("🎤 Escucha continua activada");
            }
        }

        public void StopContinuousRecognition()
        {
            if (_recognizer != null && _continuousListening)
            {
                _continuousListening = false;
                OnListeningStatusChanged?.Invoke(false);
                _recognizer.RecognizeAsyncStop();
                _voiceStatusUpdater?.Invoke("🔇 Escucha continua desactivada");
            }
        }

        public void TestMicrophone()
        {
            try
            {
                OnListeningStatusChanged?.Invoke(true);
                _recognizer.RecognizeAsync(RecognizeMode.Single);
                _voiceStatusUpdater?.Invoke("🎙️ Probando - habla algo...");
            }
            catch (Exception ex)
            {
                _voiceStatusUpdater?.Invoke($"❌ Error: {ex.Message}");
            }
        }

        public void Speak(string text)
        {
            try
            {
                _synthesizer.SpeakAsync(text);
                _transcriptUpdater?.Invoke($"🤖 JARVIS: {text}");
            }
            catch (Exception ex)
            {
                _transcriptUpdater?.Invoke($"Error al hablar: {ex.Message}");
            }
        }

        public void Dispose()
        {
            _recognizer?.Dispose();
            _synthesizer?.Dispose();
        }
    }
}