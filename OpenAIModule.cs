using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Jarvis.Modules
{
    public class OpenAIModule : IModule
    {
        private Action<string> _transcriptUpdater;
        private Action<string> _statusUpdater;
        private HttpClient _httpClient;
        private string _apiKey;
        private List<Dictionary<string, string>> _conversationHistory;

        public string Name => "OpenAI Chat Module";

        public void Initialize(Action<string> statusUpdater, Action<string> transcriptUpdater)
        {
            _statusUpdater = statusUpdater;
            _transcriptUpdater = transcriptUpdater;
            _httpClient = new HttpClient();
            _conversationHistory = new List<Dictionary<string, string>>();

            // Cargar API key desde config
            LoadApiKey();
            _statusUpdater?.Invoke("🤖 Módulo OpenAI cargado - Puedes chatear conmigo!");
        }

        private void LoadApiKey()
        {
            try
            {
                var config = System.IO.File.ReadAllText("config/api_keys.json");
                var json = JsonDocument.Parse(config);
                _apiKey = json.RootElement.GetProperty("openai_api_key").GetString();
            }
            catch
            {
                _apiKey = "sk-573ad196d8a24802ab0484e1898aba89"; // Tu API key
            }
        }

        public bool ProcessCommand(string command)
        {
            if (command.StartsWith("pregunta ") || command.StartsWith("chat ") ||
                command.StartsWith("dime ") || command.StartsWith("consulta "))
            {
                string question = command;
                foreach (var prefix in new[] { "pregunta ", "chat ", "dime ", "consulta " })
                {
                    if (command.StartsWith(prefix))
                        question = command.Substring(prefix.Length);
                }

                _transcriptUpdater?.Invoke($"🤔 Pensando...");
                Task.Run(() => AskAI(question));
                return true;
            }
            return false;
        }

        private async Task AskAI(string question)
        {
            try
            {
                _conversationHistory.Add(new Dictionary<string, string>
                {
                    { "role", "user" },
                    { "content", question }
                });

                var requestBody = new
                {
                    model = "gpt-3.5-turbo",
                    messages = _conversationHistory,
                    max_tokens = 500,
                    temperature = 0.7
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

                var response = await _httpClient.PostAsync(
                    "https://api.openai.com/v1/chat/completions",
                    content
                );

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonDocument.Parse(jsonResponse);
                var answer = result.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                _conversationHistory.Add(new Dictionary<string, string>
                {
                    { "role", "assistant" },
                    { "content", answer }
                });

                // Limitar historial
                if (_conversationHistory.Count > 20)
                    _conversationHistory.RemoveRange(0, 2);

                _transcriptUpdater?.Invoke($"🤖 IA: {answer}");

                // Usar síntesis de voz
                var speech = new System.Speech.Synthesis.SpeechSynthesizer();
                speech.SpeakAsync(answer);
            }
            catch (Exception ex)
            {
                _transcriptUpdater?.Invoke($"❌ Error en IA: {ex.Message}");
            }
        }

        public Dictionary<string, Action<string>> GetCommands()
        {
            return new Dictionary<string, Action<string>>();
        }
    }
}