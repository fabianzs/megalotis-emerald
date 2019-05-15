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
    }
}
