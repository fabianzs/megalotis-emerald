using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ASP.NET_Core_Webapp.UnitTests
{
    public class EmptyTest
    {
        [Fact]
        public void Simpletest()
        {
            int n = 5;
            int result = 5;
            Assert.Equal(result, n);
        }

        [Fact]
        public void Simpletest2()
        {
            int n = 0;
            int result = 0;
            Assert.Equal(result, n);
        }

        [Fact]
        public void Simpletest3()
        {
            int n = 15;
            int result = 15;
            Assert.Equal(result, n);
        }

        [Fact]
        public void Simpletest4()
        {
            int n = 150;
            int result = 150;
            Assert.Equal(result, n);
        }
    }
}
