using System;
using System.Collections.Generic;
using UnityEngine;

namespace TestUtils
{
    public class LogInterceptor : IDisposable
    {
        private readonly List<string> _logs = new();

        public LogInterceptor()
        {
            Application.logMessageReceived += ApplicationOnLogMessageReceived;
        }

        public IReadOnlyList<string> Logs => _logs;

        public string Last => _logs.Count > 0 ? _logs[^1] : null;

        public void Dispose()
        {
            Application.logMessageReceived -= ApplicationOnLogMessageReceived;
        }

        private void ApplicationOnLogMessageReceived(string message, string stacktrace, LogType type)
        {
            _logs.Add(message);
        }
    }
}