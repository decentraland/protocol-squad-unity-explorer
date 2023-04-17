using System;
using System.Collections.Generic;
using DefaultNamespace;
using NUnit.Framework;
using UnityEngine;
using Application = UnityEngine.Device.Application;

namespace Tests.EditorTests
{
    public class ConsoleLogTest
    {
        [Test]
        public void Log_TextFromJS_PrintToConsole()
        {
            var code = @"
                console.log('Test Message from JS @123dj')    
            ";

            var messages = new List<string>();
            void ApplicationOnlogMessageReceived(string logString, string stacktrace, LogType type)
            {
                messages.Add(logString);
            }
            
            Application.logMessageReceived += ApplicationOnlogMessageReceived;
            
            new DefaultNamespace.JSContainer().Execute(code);
            
            Application.logMessageReceived -= ApplicationOnlogMessageReceived;
            Assert.That(messages, Is.EquivalentTo(new string[]{"Test Message from JS @123dj"}));
        }
    }
}