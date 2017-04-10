using System;
using System.Collections.Generic;
using System.Linq;
using FunctionParser.Logic;
using FunctionParser.Logic.FunctionTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FunctionParserTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var parser = FunctionParser<double>.CreateStandartDouble();

            parser.TwoParamFunctions.Add(new TwoParamFunction<double>("test", (d, d1) => d1 + d));
            parser.CustomFunctions.Add(new MultipleParamFunction<double>("tests", list => list.Aggregate((curr, prev) => curr + prev)));
            parser.CustomFunctions.Add(new FixedParamFunction<double>("sos", list => list[0] * list[1], 2));
            parser.CustomFunctions.Add(new ZeroParamFunction<double>("time", () => DateTime.Now.Second));

            HashSet<string> param;

            Dictionary<double, string> tests = new Dictionary<double, string>
            {
                { 299, "(a+b)*b"},
                { Math.PI * Math.PI, "pi*pi" },
                { 6, "sos(3,2)" },
                { 36, "6*sos(3,2)" },
                { -6, "-(((6)))" },
                { 11, "-(-(11))"},
                { -21, "-(-(-(-(-(((21)))))))" },
                { -1, "-1" },
                { 7, "tests(1,2,3,1)" },
                { 4, "test(1, 3)"},
                { 3, "3" },
                { 15, "sin(pi/2)+2^4-1-1" },
                { 40, "(8 - 1 + 3) * 6 - ((3 + 7) * 2)" },
                { 121963752, "462*823-61-263+518*490*479+851+276+13-208-418-537+486+476+15*227-274" }
            };

            string[] errorTests =
            {
                "(((-1"
            };

            foreach (var item in tests)
            {
                Assert.AreEqual(item.Key,
                    parser.Parse(item.Value, out param)
                        .Evaluate(new Dictionary<string, double> {{"a", 10}, {"b", 13}}));
            }

            parser.TwoParamMiddleFunctions.Remove(parser.TwoParamMiddleFunctions.First(f => f.Name == "-"));

            parser.TwoParamMiddleFunctions.Add(new MiddleFunction<double>('-', 1, (f, s) => f - s));

            foreach (var error in errorTests)
            {
                bool hasException = false;

                try
                {
                    parser.Parse(error);
                }
                catch (Exception)
                {
                    hasException = true;
                }

                Assert.AreEqual(true, hasException, $"Тест \"{error}\" не выдал ошибку");
            }
        }
    }
}
