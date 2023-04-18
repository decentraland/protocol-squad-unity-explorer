using System;
using System.Collections.Generic;
using UnityEngine;

namespace TestUtils
{
    public class LogInterceptor : IDisposable
    {
        private List<string> _logs = new List<string>();
        public IReadOnlyList<string> Logs => _logs;

        public string Last => _logs.Count > 0 ? _logs[^1] : null;

        public LogInterceptor()
        {
            Application.logMessageReceived += ApplicationOnLogMessageReceived;
        }

        private void ApplicationOnLogMessageReceived(string message, string stacktrace, LogType type)
        {
            _logs.Add(message);
        }

        public void Dispose()
        {
            Application.logMessageReceived -= ApplicationOnLogMessageReceived;
        }
    }
}
