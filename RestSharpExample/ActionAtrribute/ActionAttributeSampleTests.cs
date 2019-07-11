using NUnit.Framework;
using RestSharpExample.ActionAtrribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpExample.ActionAtrribute
{
    [TestFixture]
    //[ConsoleAction("hola ")]
    public class ActionAttributeSampleTests
    {
        [Test]
        [ConsoleAction("Hello")]
        public void SimpleTest()
        {
            Console.WriteLine("Test ran.");
        }

        [Test]
        [ConsoleAction("Hello")]
        [ConsoleAction("Greetings")]
        public void SimpleTestGreetings()
        {
            Console.WriteLine("Test run.");
        }

        [Test]
        [ConsoleAction("Hello")]
        [TestCase("02")]
        [TestCase("01")]
        public void SimpleWithNumber(string number)
        {
            Console.WriteLine("Test run {0}.", number);
        }
        //pairwise The PairwiseAttribute is used on a test to specify that NUnit should generate test cases in such a way that all possible pairs of values are used
        [Test, Pairwise]
        public void MyTest(
             [Values("a", "b", "c")] string a,
            [Values("+", "-")] string b,
            [Values("x", "y")] string c)
        {
            Console.WriteLine("{0} {1} {2}", a, b, c);
        }

    }
}
