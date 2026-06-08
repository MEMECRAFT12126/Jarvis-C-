namespace Jarvis
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));

            // Panel principal - Fondo oscuro estilo HUD
            this.pnlBackground = new System.Windows.Forms.Panel();
            this.pnlBackground.BackColor = System.Drawing.Color.FromArgb(10, 10, 30);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;

            // Label de estado (grande)
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.Cyan;
            this.lblStatus.Location = new System.Drawing.Point(50, 50);
            this.lblStatus.Size = new System.Drawing.Size(700, 50);
            this.lblStatus.Text = "🟢 JARVIS ACTIVO";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // Área de transcript (más grande)
            this.lblTranscript = new System.Windows.Forms.Label();
            this.lblTranscript.BackColor = System.Drawing.Color.FromArgb(20, 20, 40);
            this.lblTranscript.Font = new System.Drawing.Font("Consolas", 12F);
            this.lblTranscript.ForeColor = System.Drawing.Color.Lime;
            this.lblTranscript.Location = new System.Drawing.Point(50, 120);
            this.lblTranscript.Size = new System.Drawing.Size(700, 200);
            this.lblTranscript.Text = "🎤 Esperando comandos...\r\nPresiona Ctrl+Espacio para hablar\r\nEscribe o usa voz";

            // Panel de comandos rápidos (botones futuristas)
            this.flowButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.flowButtons.Location = new System.Drawing.Point(50, 340);
            this.flowButtons.Size = new System.Drawing.Size(700, 60);
            this.flowButtons.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;

            // Botones de comandos rápidos
            this.btnYoutube = this.CreateModernButton("🎬 YouTube", System.Drawing.Color.Red);
            this.btnGoogle = this.CreateModernButton("🔍 Google", System.Drawing.Color.Blue);
            this.btnMusic = this.CreateModernButton("🎵 Música", System.Drawing.Color.Green);
            this.btnWeather = this.CreateModernButton("🌤️ Clima", System.Drawing.Color.Orange);
            this.btnNews = this.CreateModernButton("📰 Noticias", System.Drawing.Color.Purple);
            this.btnAI = this.CreateModernButton("🤖 IA Chat", System.Drawing.Color.Gold);

            this.flowButtons.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.btnYoutube, this.btnGoogle, this.btnMusic,
                this.btnWeather, this.btnNews, this.btnAI
            });

            // Área de entrada de texto (grande)
            this.txtCommand = new System.Windows.Forms.TextBox();
            this.txtCommand.BackColor = System.Drawing.Color.FromArgb(30, 30, 50);
            this.txtCommand.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.txtCommand.ForeColor = System.Drawing.Color.White;
            this.txtCommand.Location = new System.Drawing.Point(50, 420);
            this.txtCommand.Size = new System.Drawing.Size(550, 35);
            this.txtCommand.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtCommand_KeyDown);

            // Botón enviar (grande)
            this.btnSend = new System.Windows.Forms.Button();
            this.btnSend.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSend.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnSend.ForeColor = System.Drawing.Color.White;
            this.btnSend.Location = new System.Drawing.Point(610, 420);
            this.btnSend.Size = new System.Drawing.Size(140, 35);
            this.btnSend.Text = "ENVIAR ➤";
            this.btnSend.Click += new System.EventHandler(this.BtnSend_Click);

            // Panel de estado de voz (animado)
            this.pnlVoiceStatus = new System.Windows.Forms.Panel();
            this.pnlVoiceStatus.BackColor = System.Drawing.Color.FromArgb(50, 50, 70);
            this.pnlVoiceStatus.Location = new System.Drawing.Point(50, 470);
            this.pnlVoiceStatus.Size = new System.Drawing.Size(700, 60);
            this.pnlVoiceStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            this.lblVoiceStatus = new System.Windows.Forms.Label();
            this.lblVoiceStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblVoiceStatus.ForeColor = System.Drawing.Color.LightGray;
            this.lblVoiceStatus.Location = new System.Drawing.Point(10, 10);
            this.lblVoiceStatus.Size = new System.Drawing.Size(680, 40);
            this.lblVoiceStatus.Text = "🎙️ Esperando activación... (Ctrl+Espacio)";

            this.pnlVoiceStatus.Controls.Add(this.lblVoiceStatus);

            // Botones de control
            this.btnContinuous = this.CreateModernButton("🎤 Modo Continuo", System.Drawing.Color.DarkCyan);
            this.btnContinuous.Location = new System.Drawing.Point(50, 550);
            this.btnContinuous.Size = new System.Drawing.Size(170, 40);
            this.btnContinuous.Click += new System.EventHandler(this.BtnContinuous_Click);

            this.btnTestMic = this.CreateModernButton("🎙️ Probar Mic", System.Drawing.Color.DarkOrange);
            this.btnTestMic.Location = new System.Drawing.Point(230, 550);
            this.btnTestMic.Size = new System.Drawing.Size(170, 40);
            this.btnTestMic.Click += new System.EventHandler(this.BtnTestMic_Click);

            this.btnSettings = this.CreateModernButton("⚙️ Configuración", System.Drawing.Color.Gray);
            this.btnSettings.Location = new System.Drawing.Point(410, 550);
            this.btnSettings.Size = new System.Drawing.Size(170, 40);
            this.btnSettings.Click += new System.EventHandler(this.BtnSettings_Click);

            // Configurar el formulario
            this.ClientSize = new System.Drawing.Size(800, 620);
            this.BackColor = System.Drawing.Color.FromArgb(10, 10, 30);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "J.A.R.V.I.S - Asistente Inteligente v3.0";
            this.TopMost = true;

            // Agregar todos los controles
            this.pnlBackground.Controls.Add(this.lblStatus);
            this.pnlBackground.Controls.Add(this.lblTranscript);
            this.pnlBackground.Controls.Add(this.flowButtons);
            this.pnlBackground.Controls.Add(this.txtCommand);
            this.pnlBackground.Controls.Add(this.btnSend);
            this.pnlBackground.Controls.Add(this.pnlVoiceStatus);
            this.pnlBackground.Controls.Add(this.btnContinuous);
            this.pnlBackground.Controls.Add(this.btnTestMic);
            this.pnlBackground.Controls.Add(this.btnSettings);

            this.Controls.Add(this.pnlBackground);

            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button CreateModernButton(string text, System.Drawing.Color color)
        {
            var btn = new System.Windows.Forms.Button();
            btn.Text = text;
            btn.BackColor = color;
            btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btn.ForeColor = System.Drawing.Color.White;
            btn.Size = new System.Drawing.Size(100, 50);
            btn.UseVisualStyleBackColor = false;
            return btn;
        }

        // Controles
        private System.Windows.Forms.Panel pnlBackground;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblTranscript;
        private System.Windows.Forms.FlowLayoutPanel flowButtons;
        private System.Windows.Forms.Button btnYoutube, btnGoogle, btnMusic, btnWeather, btnNews, btnAI;
        private System.Windows.Forms.TextBox txtCommand;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Panel pnlVoiceStatus;
        private System.Windows.Forms.Label lblVoiceStatus;
        private System.Windows.Forms.Button btnContinuous, btnTestMic, btnSettings;
    }
}