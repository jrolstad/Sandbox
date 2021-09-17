using System;
using Xunit;
using MyApp.Console;
using System.IO;

namespace MyApp.Console.Tests
{
    public class ProgramTests
    {
        [Fact]
        public void Run_NoInputs_WritesNothing()
        {
            var program = new Program();
            var writer = new StringWriter();

            program.Run(new string[0], writer);

            Assert.Empty(writer.ToString()); 
        }

        [Fact]
        public void Run_EmptyString_WritesEmptyLine()
        {
            var program = new Program();
            var writer = new StringWriter();

            program.Run(new[] { "" }, writer);

            Assert.Equal(Environment.NewLine, writer.ToString());
        }

        [Fact]
        public void Run_SingleInput_WritesReversedLine()
        {
            var program = new Program();
            var writer = new StringWriter();

            program.Run(new[] { "hola" }, writer);

            Assert.Equal($"aloh{Environment.NewLine}", writer.ToString());
        }

        [Fact]
        public void Run_MultipleInputs_WritesReversedLines()
        {
            var program = new Program();
            var writer = new StringWriter();

            program.Run(new[] { "hola","bonjour" }, writer);

            Assert.Equal($"aloh{Environment.NewLine}ruojnob{Environment.NewLine}", writer.ToString());
        }
    }
}
