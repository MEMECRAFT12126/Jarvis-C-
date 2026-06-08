using System;
using System.Collections.Generic;

namespace Jarvis.Modules
{
    public interface IModule
    {
        string Name { get; }
        void Initialize(Action<string> statusUpdater, Action<string> transcriptUpdater);
        bool ProcessCommand(string command);
        Dictionary<string, Action<string>> GetCommands();
    }
}