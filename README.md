# 📋 JARVIS - Asistente Inteligente con Reconocimiento de Voz

## 🎯 ¿Qué es JARVIS?

JARVIS es un asistente virtual inteligente desarrollado en **C# (.NET Framework 4.7.2)** con interfaz gráfica estilo HUD futurista. Combina **reconocimiento de voz**, **comandos por texto**, **módulos extensibles** y **conexión a IA (OpenAI)** para crear una experiencia interactiva similar a un asistente personal.

---

## ✨ Características Principales

### 🎤 **Reconocimiento de Voz**
- Escucha comandos por micrófono
- Soporte para español e inglés
- Modo continuo (escucha siempre activa)
- Hotkey global: **Ctrl + Espacio** para activar

### 🧠 **IA Integrada (OpenAI)**
- Chat inteligente usando GPT-3.5-turbo
- Mantiene contexto de conversación
- Responde preguntas y ejecuta tareas

### 🚀 **Ejecución de Comandos**
- Abre programas y URLs (Google, YouTube, Discord, etc.)
- Control del sistema (apagar, reiniciar)
- Gestión de recordatorios

### 🔌 **Sistema Modular**
Arquitectura extensible con módulos independientes:

| Módulo | Función |
|--------|---------|
| 🎵 **MusicModule** | Reproduce música en YouTube |
| 🤖 **OpenAIModule** | Chat con IA |
| 📚 **EducationModule** | Cálculos matemáticos |
| 🎮 **GamingModule** | Abre Discord, Roblox, Minecraft |
| 🏠 **HomeAutomationModule** | Control de luces (Arduino/RPi) |
| 📰 **NewsWeatherModule** | Noticias y clima |
| 📋 **ProductivityModule** | Recordatorios |

### 🖥️ **Interfaz Moderna**
- Diseño estilo HUD futurista
- Animaciones en tiempo real
- Indicador visual de voz
- Botones de acceso rápido
- Logs de transcripción

---

## 🛠️ Tecnologías Utilizadas

- **Lenguaje**: C# (.NET Framework 4.7.2)
- **UI**: Windows Forms
- **Reconocimiento de voz**: `System.Speech`
- **API IA**: OpenAI GPT-3.5-turbo
- **HTTP Client**: `System.Net.Http`
- **JSON**: `System.Text.Json`

---

## 📦 Estructura del Proyecto

```
Jarvis/
├── Form1.cs                 # Interfaz principal y UI
├── Form1.Designer.cs        # Diseño visual
├── JarvisEngine.cs          # Motor central
├── Program.cs               # Punto de entrada
├── CommandRegistry.cs       # Registro de comandos
├── GlobalHotkeyListener.cs  # Hotkey global (Ctrl+Espacio)
│
├── Modules/                 # Módulos extensibles
│   ├── IModule.cs          # Interfaz base
│   ├── OpenAIModule.cs     # Chat con IA
│   ├── MusicModule.cs      # Reproducción música
│   ├── EducationModule.cs  # Cálculos
│   ├── GamingModule.cs     # Juegos
│   ├── ProductivityModule.cs # Recordatorios
│   ├── NewsWeatherModule.cs  # Clima/noticias
│   └── HomeAutomationModule.cs # Domótica
│
├── Core/
│   ├── VoiceEngine.cs      # Motor de voz
│   └── CommandParser.cs    # Parseo de comandos
│
├── Commands/
│   └── OpenCommand.cs      # Comando "abrir" (soporta 30+ apps)
│
└── config/
    └── api_keys.json       # Configuración de APIs (OpenAI, etc.)
```

---

## 🎮 Comandos Soportados

### Por Voz o Texto

| Categoría | Ejemplos |
|-----------|----------|
| 🌐 **Web** | "abrir youtube", "abrir google", "abrir gmail" |
| 🎮 **Juegos** | "abrir roblox", "abrir minecraft", "abrir discord" |
| 📝 **Apps** | "abrir word", "abrir excel", "abrir calculadora" |
| 🎵 **Música** | "reproduce Bohemian Rhapsody", "pon Bad Bunny" |
| 🤖 **IA** | "pregunta qué es la IA", "dime el clima" |
| 🧮 **Matemáticas** | "cuánto es 25 + 17", "calcula 100 * 3" |
| 📋 **Recordatorios** | "recordar comprar leche", "qué tengo que hacer" |
| ⚙️ **Sistema** | "apagar", "reiniciar", "salir" |

---

## 🚀 Instalación y Ejecución

### Requisitos
- Windows 7 o superior
- .NET Framework 4.7.2
- Micrófono (para comandos de voz)
- (Opcional) API Key de OpenAI

### Pasos

1. **Clonar o descargar** el proyecto
2. **Abrir** `Jarvis.csproj` en Visual Studio 2019/2022
3. **Restaurar paquetes NuGet** (automático o manual)
4. **Compilar** (Ctrl+Shift+B)
5. **Ejecutar como Administrador** (necesario para hotkey global)

### Configurar OpenAI (opcional)
Crear archivo `config/api_keys.json`:
```json
{
  "openai_api_key": "tu-api-key-aqui"
}
```

---

## 🎯 Comandos Disponibles (Ejemplos)

```
✅ "abrir youtube"        → Abre YouTube en navegador
✅ "abrir discord"        → Abre Discord
✅ "reproduce Imagine Dragons" → Busca en YouTube
✅ "pregunta qué es la gravedad" → Consulta a IA
✅ "cuánto es 15 * 8"     → Calcula resultado
✅ "recordar llamar a mamá" → Guarda recordatorio
✅ "qué tengo que hacer"   → Muestra recordatorios
✅ "apagar"               → Apaga PC en 10s
✅ "abrir roblox"         → Abre Roblox Player
```

---

## 🧩 Arquitectura Modular

El sistema permite **agregar nuevos módulos fácilmente**:

```csharp
public class MiModulo : IModule
{
    public string Name => "Mi Módulo";
    
    public void Initialize(Action<string> status, Action<string> transcript) { }
    
    public bool ProcessCommand(string command)
    {
        if (command.Contains("mi comando"))
        {
            // Hacer algo
            return true;
        }
        return false;
    }
    
    public Dictionary<string, Action<string>> GetCommands() => new();
}
```

Luego en `Form1.cs`:
```csharp
_engine.LoadModule(new MiModulo());
```

---

## 🔧 Troubleshooting

| Problema | Solución |
|----------|----------|
| **No reconoce voz** | Verificar micrófono, ejecutar como Admin |
| **No abre programas** | Revisar rutas en `OpenCommand.cs` |
| **Error OpenAI** | Verificar API key en `config/api_keys.json` |
| **Hotkey no funciona** | Ejecutar como Administrador |

---

## 📝 Licencia

Proyecto educativo - Libre para uso personal y modificaciones.

---

## 👨‍💻 Autor

Desarrollado como asistente personal inteligente con arquitectura modular y reconocimiento de voz.

---

**✨ ¡Disfruta de tu asistente personal JARVIS! ✨**
