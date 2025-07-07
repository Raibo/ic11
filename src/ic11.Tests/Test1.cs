using ic11;

namespace ic11.Tests
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string code = @"
                pin Sensor d0;
                pin Vent d1;

                void Main()
                {
                    while(true)
                    {
                        yield;

                        Vent.Mode = 1;
                        Vent.On = Sensor.Pressure > 0 & Sensor.RatioOxygen <= 0;
                    }
                }
            ";

            var program = Program.CompileText(code);

            Console.WriteLine(program);

            Assert.IsNotNull(program, "Program should not be null");
        }
    }
}
