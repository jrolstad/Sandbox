using System;
using Xunit;

namespace MyApp.Tests
{
    public class StringServiceTests
    {
        //https://xunit.net/docs/getting-started/netfx/visual-studio 

        [Fact]
        public void Reverse_null_ReturnsNull()
        {
            var service = new StringService();

            string input = null;
            var result = service.Reverse(input);

            Assert.Null(result);
        }

        [Fact]
        public void Reverse_Empty_ReturnsEmpty()
        {
            var service = new StringService();

            string input = "";
            var result = service.Reverse(input);

            Assert.Equal("",result);
        }

        [Fact]
        public void Reverse_ValidValue_ReturnsReversedString()
        {
            var service = new StringService();

            string input = "hello";
            var result = service.Reverse(input);

            Assert.Equal("olleh", result);
        }

        [Theory] // See https://spr.com/keeping-xunit-tests-clean-and-dry-using-theory/
        [InlineData(null,null)]
        [InlineData("","")]
        [InlineData("stephen","nehpets")]
        [InlineData("data","atad")]
        [InlineData("tacocat","tacocat")]
        public void Reverse_ReversesString(string input, string expected)
        {
            var service = new StringService();

            var result = service.Reverse(input);

            Assert.Equal(expected, result);
        }
    }
}
