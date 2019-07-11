using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpExample.ActionAtrribute
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Assembly, AllowMultiple = true)]
    public class ConsoleActionAttribute : Attribute, ITestAction
    {
        private string _Message;
        public ConsoleActionAttribute(string message) { _Message = message; }
        
        public void BeforeTest(TestDetails details)
        {
            WriteToConsole("Before", details);
        }
       private void WriteToConsole(string eventMessage, TestDetails details)
        {
            Console.WriteLine("{0} {1}: {2}, from {3}.{4}.",
                eventMessage,
                details.IsSuite ? "Suite" : "Case",
                _Message,
                details.Fixture != null ? details.Fixture.GetType().Name : "{no fixture}",
                details.Method != null ? details.Method.Name : "{no method}");
        }

        public void AfterTest(TestDetails testDetails)
        {
            WriteToConsole("After", testDetails);
        }

        public ActionTargets Targets
        {
            get { return ActionTargets.Test | ActionTargets.Suite; }
        }
    }
}

