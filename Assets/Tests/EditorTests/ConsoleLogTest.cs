using JSInterop;
using NUnit.Framework;
using TestUtils;

namespace EditorTests
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
            new JSContainer().Execute(code);

            Assert.That(logInterceptor.Logs, Is.EquivalentTo(new[] { "Test Message from JS @123dj" }));
        }
    }
}