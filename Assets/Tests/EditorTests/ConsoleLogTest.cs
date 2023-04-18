using System;
using System.Collections.Generic;
using DefaultNamespace;
using NUnit.Framework;
using TestUtils;
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

            using var logInterceptor = new LogInterceptor();
            new DefaultNamespace.JSContainer().Execute(code);
                
            Assert.That(logInterceptor.Logs, Is.EquivalentTo(new string[]{"Test Message from JS @123dj"}));
        }
    }
}