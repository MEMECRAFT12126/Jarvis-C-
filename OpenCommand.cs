using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Jarvis.Commands
{
    public class OpenCommand
    {
        private Action<string> _transcriptUpdater;
        private Action<string> _statusUpdater;

        public OpenCommand(Action<string> transcriptUpdater, Action<string> statusUpdater)
        {
            _transcriptUpdater = transcriptUpdater;
            _statusUpdater = statusUpdater;
        }

        public void Execute(string argument)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                _transcriptUpdater?.Invoke("🤖 ¿Qué quieres abrir? Por ejemplo: abrir youtube");
                return;
            }

            argument = argument.ToLower().Trim();

            // Diccionario expandido de aplicaciones
            var apps = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                // Navegación web
                { "google", "https://www.google.com" },
                { "youtube", "https://www.youtube.com" },
                { "facebook", "https://www.facebook.com" },
                { "twitter", "https://twitter.com" },
                { "instagram", "https://www.instagram.com" },
                { "github", "https://github.com" },
                { "gmail", "https://mail.google.com" },
                { "reddit", "https://reddit.com" },
                { "netflix", "https://www.netflix.com" },
                { "twitch", "https://www.twitch.tv" },
                
                // Juegos
                { "roblox", GetRobloxPath() },
                { "minecraft", GetMinecraftPath() },
                
                // Aplicaciones del sistema
                { "calculadora", "calc" },
                { "bloc de notas", "notepad" },
                { "notepad", "notepad" },
                { "paint", "mspaint" },
                { "cmd", "cmd" },
                { "terminal", "cmd" },
                { "explorador", "explorer" },
                { "explorer", "explorer" },
                { "administrador de tareas", "taskmgr" },
                { "task manager", "taskmgr" },
                { "regedit", "regedit" },
                
                // Navegadores
                { "chrome", GetChromePath() },
                { "firefox", GetFirefoxPath() },
                { "edge", "microsoft-edge:" },
                { "brave", GetBravePath() },
                
                // Microsoft Office
                { "word", GetOfficePath("WINWORD.EXE") },
                { "excel", GetOfficePath("EXCEL.EXE") },
                { "powerpoint", GetOfficePath("POWERPNT.EXE") },
                { "outlook", GetOfficePath("OUTLOOK.EXE") },
                
                // Programas comunes
                { "vscode", GetVSCodePath() },
                { "visual studio", GetVisualStudioPath() },
                { "spotify", GetSpotifyPath() },
                { "discord", GetDiscordPath() },
                { "telegram", GetTelegramPath() },
                { "whatsapp", GetWhatsAppPath() },
                { "photoshop", GetPhotoshopPath() },
                { "premiere", GetPremierePath() }
            };

            // Si es URL directa
            if (argument.StartsWith("http://") || argument.StartsWith("https://"))
            {
                OpenUrl(argument);
                return;
            }

            // Buscar en diccionario
            if (apps.ContainsKey(argument))
            {
                string target = apps[argument];
                OpenTarget(argument, target);
                return;
            }

            // Intentar buscar ejecutable en PATH
            try
            {
                Process.Start(new ProcessStartInfo(argument) { UseShellExecute = true });
                _transcriptUpdater?.Invoke($"✅ Abriendo: {argument}");
            }
            catch
            {
                _transcriptUpdater?.Invoke($"❌ No sé cómo abrir: {argument}. Puedes agregarlo manualmente al diccionario.");
            }
        }

        private void OpenTarget(string name, string target)
        {
            try
            {
                if (target.StartsWith("http"))
                {
                    OpenUrl(target);
                }
                else if (File.Exists(target))
                {
                    Process.Start(target);
                    _transcriptUpdater?.Invoke($"✅ Abriendo: {name}");
                }
                else
                {
                    Process.Start(new ProcessStartInfo(target) { UseShellExecute = true });
                    _transcriptUpdater?.Invoke($"✅ Abriendo: {name}");
                }
            }
            catch (Exception ex)
            {
                _transcriptUpdater?.Invoke($"❌ Error al abrir {name}: {ex.Message}");
            }
        }

        private void OpenUrl(string url)
        {
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            _transcriptUpdater?.Invoke($"🌐 Abriendo: {url}");
        }

        // Métodos auxiliares para encontrar rutas dinámicamente
        private string GetRobloxPath()
        {
            string[] paths = {
                @"C:\Program Files (x86)\Roblox\Versions\RobloxPlayerLauncher.exe",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Roblox\Versions\RobloxPlayerLauncher.exe")
            };
            foreach (string path in paths)
                if (File.Exists(path)) return path;
            return "roblox-player:";
        }

        private string GetMinecraftPath()
        {
            string[] paths = {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @".minecraft\minecraftlauncher.exe"),
                @"C:\Program Files (x86)\Minecraft Launcher\MinecraftLauncher.exe",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Minecraft Launcher\MinecraftLauncher.exe")
            };
            foreach (string path in paths)
                if (File.Exists(path)) return path;
            return "minecraft://";
        }

        private string GetChromePath()
        {
            string[] paths = {
                @"C:\Program Files\Google\Chrome\Application\chrome.exe",
                @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"
            };
            foreach (string path in paths)
                if (File.Exists(path)) return path;
            return "chrome";
        }

        private string GetFirefoxPath()
        {
            string[] paths = {
                @"C:\Program Files\Mozilla Firefox\firefox.exe",
                @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe"
            };
            foreach (string path in paths)
                if (File.Exists(path)) return path;
            return "firefox";
        }

        private string GetBravePath()
        {
            string[] paths = {
                @"C:\Program Files\BraveSoftware\Brave-Browser\Application\brave.exe",
                @"C:\Program Files (x86)\BraveSoftware\Brave-Browser\Application\brave.exe"
            };
            foreach (string path in paths)
                if (File.Exists(path)) return path;
            return "brave";
        }

        private string GetOfficePath(string exeName)
        {
            string[] paths = {
                @"C:\Program Files\Microsoft Office\root\Office16\" + exeName,
                @"C:\Program Files (x86)\Microsoft Office\root\Office16\" + exeName,
                @"C:\Program Files\Microsoft Office\Office16\" + exeName,
                @"C:\Program Files (x86)\Microsoft Office\Office16\" + exeName
            };
            foreach (string path in paths)
                if (File.Exists(path)) return path;
            return exeName;
        }

        private string GetVSCodePath()
        {
            string[] paths = {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Programs\Microsoft VS Code\Code.exe"),
                @"C:\Program Files\Microsoft VS Code\Code.exe",
                @"C:\Program Files (x86)\Microsoft VS Code\Code.exe"
            };
            foreach (string path in paths)
                if (File.Exists(path)) return path;
            return "code";
        }

        private string GetVisualStudioPath()
        {
            string[] paths = {
                @"C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe",
                @"C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\IDE\devenv.exe",
                @"C:\Program Files\Microsoft Visual Studio\2019\Community\Common7\IDE\devenv.exe",
                @"C:\Program Files\Microsoft Visual Studio\2019\Professional\Common7\IDE\devenv.exe"
            };
            foreach (string path in paths)
                if (File.Exists(path)) return path;
            return "devenv";
        }

        private string GetSpotifyPath()
        {
            string[] paths = {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Spotify\Spotify.exe"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Spotify\Spotify.exe"),
                @"C:\Program Files\Spotify\Spotify.exe",
                @"C:\Program Files (x86)\Spotify\Spotify.exe"
            };
            foreach (string path in paths)
                if (File.Exists(path)) return path;
            return "spotify";
        }

        private string GetDiscordPath()
        {
            string[] paths = {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Discord\Discord.exe"),
                @"C:\Program Files\Discord\Discord.exe",
                @"C:\Program Files (x86)\Discord\Discord.exe"
            };
            foreach (string path in paths)
                if (File.Exists(path)) return path;
            return "discord";
        }

        private string GetTelegramPath()
        {
            string[] paths = {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Telegram Desktop\Telegram.exe"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Telegram Desktop\Telegram.exe")
            };
            foreach (string path in paths)
                if (File.Exists(path)) return path;
            return "telegram";
        }

        private string GetWhatsAppPath()
        {
            string[] paths = {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"WhatsApp\WhatsApp.exe"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"WhatsApp\WhatsApp.exe")
            };
            foreach (string path in paths)
                if (File.Exists(path)) return path;
            return "whatsapp";
        }

        private string GetPhotoshopPath()
        {
            string[] paths = {
                @"C:\Program Files\Adobe\Adobe Photoshop 2024\Photoshop.exe",
                @"C:\Program Files\Adobe\Adobe Photoshop 2023\Photoshop.exe",
                @"C:\Program Files\Adobe\Adobe Photoshop 2022\Photoshop.exe",
                @"C:\Program Files (x86)\Adobe\Adobe Photoshop 2024\Photoshop.exe",
                @"C:\Program Files (x86)\Adobe\Adobe Photoshop 2023\Photoshop.exe"
            };
            foreach (string path in paths)
                if (File.Exists(path)) return path;
            return "photoshop";
        }

        private string GetPremierePath()
        {
            string[] paths = {
                @"C:\Program Files\Adobe\Adobe Premiere Pro 2024\Premiere Pro.exe",
                @"C:\Program Files\Adobe\Adobe Premiere Pro 2023\Premiere Pro.exe",
                @"C:\Program Files (x86)\Adobe\Adobe Premiere Pro 2024\Premiere Pro.exe"
            };
            foreach (string path in paths)
                if (File.Exists(path)) return path;
            return "premiere";
        }
    }
}