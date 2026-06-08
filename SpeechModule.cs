using System;
using System.Globalization;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Windows.Forms;

namespace Jarvis.Modules
{
    public class SpeechModule : IDisposable
    {
        private SpeechRecognitionEngine _recognizer;
        private SpeechSynthesizer _synthesizer;
        private Action<string> _transcriptUpdater;
        private Action<string> _statusUpdater;
        private bool _isContinuousListening = false;

        public event Action<string> OnSpeechRecognized;

        public SpeechModule(Action<string> transcriptUpdater, Action<string> statusUpdater)
        {
            _transcriptUpdater = transcriptUpdater;
            _statusUpdater = statusUpdater;
            _synthesizer = new SpeechSynthesizer();
            _synthesizer.SetOutputToDefaultAudioDevice();

            // Configurar voz
            try
            {
                foreach (InstalledVoice voice in _synthesizer.GetInstalledVoices())
                {
                    VoiceInfo info = voice.VoiceInfo;
                    if (info.Culture.Name.StartsWith("es"))
                    {
                        _synthesizer.SelectVoice(info.Name);
                        _transcriptUpdater?.Invoke($"✅ Usando voz: {info.Description}");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                _transcriptUpdater?.Invoke($"⚠️ No se pudo configurar voz: {ex.Message}");
            }

            _synthesizer.Rate = 1;
            _synthesizer.Volume = 100;
        }

        public void Initialize()
        {
            try
            {
                // Configurar reconocimiento en español
                CultureInfo ci = new CultureInfo("es-ES");
                _recognizer = new SpeechRecognitionEngine(ci);

                // CRÍTICO: Configurar entrada de audio
                _recognizer.SetInputToDefaultAudioDevice();

                _statusUpdater?.Invoke("✅ Micrófono configurado correctamente");
            }
            catch (Exception ex)
            {
                _statusUpdater?.Invoke($"❌ Error al configurar micrófono: {ex.Message}");
                return;
            }

            // Comandos predefinidos
            Choices commands = new Choices();
            commands.Add(new string[] {
                "abrir", "abre", "navegador", "chrome", "google",
                "youtube", "calculadora", "bloc de notas", "notepad",
                "apagar", "reiniciar", "silenciar", "cerrar", "salir",
                "roblox", "minecraft", "word", "excel", "powerpoint",
                "whatsapp", "telegram", "discord", "spotify", "netflix"
            });

            GrammarBuilder gb = new GrammarBuilder();
            gb.Append(commands);
            Grammar grammar = new Grammar(gb);
            _recognizer.LoadGrammar(grammar);

            // Dictation libre
            try
            {
                _recognizer.LoadGrammar(new DictationGrammar());
            }
            catch { }

            _recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
            _recognizer.SpeechRecognitionRejected += (s, e) =>
                _transcriptUpdater?.Invoke("🔇 No entendí lo que dijiste");
        }

        public void TestMicrophone()
        {
            try
            {
                _statusUpdater?.Invoke("🎙️ Probando micrófono - di algo...");
                _recognizer?.RecognizeAsync(RecognizeMode.Single);
            }
            catch (Exception ex)
            {
                _statusUpdater?.Invoke($"❌ Error en prueba de micrófono: {ex.Message}");
            }
        }

        private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string recognizedText = e.Result.Text;
            OnSpeechRecognized?.Invoke(recognizedText);
        }

        public void StartRecognitionOnce()
        {
            try
            {
                if (_recognizer != null)
                {
                    if (_isContinuousListening)
                        StopContinuousRecognition();

                    _recognizer.RecognizeAsync(RecognizeMode.Single);
                }
            }
            catch (Exception ex)
            {
                _transcriptUpdater?.Invoke($"Error al iniciar reconocimiento: {ex.Message}");
            }
        }

        public void StartContinuousRecognition()
        {
            try
            {
                if (_recognizer != null && !_isContinuousListening)
                {
                    _isContinuousListening = true;
                    _recognizer.RecognizeAsync(RecognizeMode.Multiple);
                }
            }
            catch (Exception ex)
            {
                _transcriptUpdater?.Invoke($"Error en reconocimiento continuo: {ex.Message}");
            }
        }

        public void StopContinuousRecognition()
        {
            try
            {
                if (_recognizer != null && _isContinuousListening)
                {
                    _isContinuousListening = false;
                    _recognizer.RecognizeAsyncStop();
                }
            }
            catch (Exception ex)
            {
                _transcriptUpdater?.Invoke($"Error al detener reconocimiento: {ex.Message}");
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
            try
            {
                _recognizer?.Dispose();
                _synthesizer?.Dispose();
            }
            catch { }
        }
    }
}