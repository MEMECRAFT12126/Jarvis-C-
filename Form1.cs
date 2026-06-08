using System;
using System.Drawing;
using System.Windows.Forms;
using Jarvis.Core;
using Jarvis.Modules;
using Jarvis.Listeners;  // <-- AGREGAR ESTO
using System.Threading.Tasks;

namespace Jarvis
{
    public partial class Form1 : Form
    {
        private JarvisEngine _engine;
        private bool _continuousMode = false;
        private Timer _animationTimer;
        private int _animationFrame = 0;

        public Form1()
        {
            InitializeComponent();
            InitializeJarvis();
            SetupAnimations();
        }

        private void InitializeJarvis()
        {
            _engine = new JarvisEngine(UpdateStatus, UpdateTranscript, UpdateVoiceStatus);
            _engine.Initialize();

            // Cargar todos los módulos
            _engine.LoadModule(new OpenAIModule());        // Chat con IA
            _engine.LoadModule(new MusicModule());          // Spotify/YouTube
            _engine.LoadModule(new GamingModule());         // Discord/juegos
            _engine.LoadModule(new EducationModule());      // Álgebra/gramática
            _engine.LoadModule(new HomeAutomationModule()); // Domótica
            _engine.LoadModule(new ProductivityModule());   // Recordatorios/correos
            _engine.LoadModule(new NewsWeatherModule());    // Noticias/clima

            UpdateStatus("✅ JARVIS v3.0 inicializado. Todos los módulos cargados.");
            AnimateStatus();
        }

        private void SetupAnimations()
        {
            _animationTimer = new Timer();
            _animationTimer.Interval = 100;
            _animationTimer.Tick += (s, e) => AnimateVoiceIndicator();
            _animationTimer.Start();
        }

        private void AnimateVoiceIndicator()
        {
            if (_engine.IsListening)
            {
                _animationFrame = (_animationFrame + 1) % 20;
                int intensity = 50 + (_animationFrame * 10);
                pnlVoiceStatus.BackColor = Color.FromArgb(intensity, 50, 70);
                lblVoiceStatus.ForeColor = Color.Cyan;
                lblVoiceStatus.Text = "🎤 ESCUCHANDO... " + new string('●', _animationFrame / 4);
            }
            else
            {
                pnlVoiceStatus.BackColor = Color.FromArgb(50, 50, 70);
                lblVoiceStatus.ForeColor = Color.LightGray;
                if (!_continuousMode)
                    lblVoiceStatus.Text = "🎙️ Presiona Ctrl+Espacio para hablar";
                else
                    lblVoiceStatus.Text = "🔴 MODO CONTINUO ACTIVO - Habla cuando quieras";
            }
        }

        private void AnimateStatus()
        {
            Task.Run(async () =>
            {
                string[] statusFrames = { "🟢 JARVIS", "🔵 JARVIS.", "🟣 JARVIS..", "🔴 JARVIS..." };
                for (int i = 0; i < 20; i++)
                {
                    this.Invoke(new Action(() =>
                    {
                        lblStatus.Text = statusFrames[i % statusFrames.Length];
                    }));
                    await Task.Delay(500);
                }
                this.Invoke(new Action(() =>
                {
                    lblStatus.Text = "🟢 JARVIS ACTIVO";
                }));
            });
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            string command = txtCommand.Text.Trim();
            if (!string.IsNullOrEmpty(command))
            {
                UpdateTranscript($"✏️ Tú: {command}");
                _engine.ProcessCommand(command);
                txtCommand.Clear();
            }
        }

        private void TxtCommand_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnSend_Click(sender, e);
                e.SuppressKeyPress = true;
            }
        }

        private void BtnContinuous_Click(object sender, EventArgs e)
        {
            _continuousMode = !_continuousMode;
            if (_continuousMode)
            {
                _engine.StartContinuousListening();
                btnContinuous.BackColor = Color.Green;
                UpdateStatus("🎤 Modo continuo activado - habla cuando quieras");
            }
            else
            {
                _engine.StopContinuousListening();
                btnContinuous.BackColor = Color.DarkCyan;
                UpdateStatus("🔇 Modo continuo desactivado - usa Ctrl+Espacio");
            }
        }

        private void BtnTestMic_Click(object sender, EventArgs e)
        {
            UpdateStatus("🎙️ Probando micrófono... di algo");
            _engine.TestMicrophone();
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            // Abrir ventana de configuración
            MessageBox.Show("Configuración:\n\n" +
                "1. Ve a config/api_keys.json para agregar tus API keys\n" +
                "2. Soporta: OpenAI, Spotify, Discord, NewsAPI, WeatherAPI\n" +
                "3. Puedes agregar comandos personalizados en modules/",
                "Configuración de JARVIS",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        public void UpdateStatus(string message)
        {
            if (lblStatus.InvokeRequired)
            {
                lblStatus.Invoke(new Action<string>(UpdateStatus), message);
                return;
            }
            lblStatus.Text = $"🟢 {message}";
        }

        public void UpdateTranscript(string message)
        {
            if (lblTranscript.InvokeRequired)
            {
                lblTranscript.Invoke(new Action<string>(UpdateTranscript), message);
                return;
            }
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            lblTranscript.Text = $"[{timestamp}] {message}\r\n" + lblTranscript.Text;
            if (lblTranscript.Text.Length > 5000)
                lblTranscript.Text = lblTranscript.Text.Substring(0, 4500);
        }

        public void UpdateVoiceStatus(string message)
        {
            if (lblVoiceStatus.InvokeRequired)
            {
                lblVoiceStatus.Invoke(new Action<string>(UpdateVoiceStatus), message);
                return;
            }
            lblVoiceStatus.Text = message;
        }
    }
}