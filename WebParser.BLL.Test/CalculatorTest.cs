using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebParser.App;

namespace WebParser.App.Test
{
    [TestFixture]
    public class CalculatorTests
    {
        [TestCase]
        public void Sum_CheckResult()
        {
            // arrange
            var calc = new Calculator();

            // act
            var res = calc.Sum(2, 2);

            // assert
            Assert.AreEqual(4, res);
        }


    }

}
