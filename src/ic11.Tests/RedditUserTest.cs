namespace ic11.Tests;

using ic11.Emulator;

[TestClass]
public sealed class RedditUserTest
{
    private readonly Emulator _emulator = new();

    [TestMethod]
    public void TestAc_GeneratesSameOrShorterProgram()
    {
        var code = @"
            pin sensor d0;
            pin analyzer d1;

            const valve = -1280984102;
            const vent = -1129453144;
            const filtration = -348054045;
            const pump = -321403609;

            const hot = ""AC Valve Hot"";
            const cold = ""AC Valve Cold"";
            const co2 = ""Filtration CO2"";
            const pol = ""Filtration POL"";
            const acV = ""AC Vent"";
            const acV2 = ""AC Vent2"";

            const pressureMax = 105;
            const pressureMin = 95;
            const tempMax = 273.15 + 26;
            const tempMid = 273.15 + 25;
            const tempMin = 273.15 + 24;
            const ratioO2Min = 0.2;
            const ratioCO2Min = 0.20;
            const ratioPOLMax = 0.00001;
            const ratioNO2Max = 0.00001;

            const OUT = 0;
            const IN = 1;

            void Main()
            {
                while (true)
                {
                    yield;
                    var pres = sensor.Pressure;
                    var temp = sensor.Temperature;
                    var ratioO2 = sensor.RatioOxygen;
                    var ratioCO2 = sensor.RatioCarbonDioxide;
                    var ratioPOL = sensor.RatioPollutant;
                    var ratioNO2 = sensor.RatioNitrousOxide;

                    var tempGood = 0;

                    // Pump oxygen in if ratio is <20% or presssure is <90kPa
                    if (ratioO2 < ratioO2Min | pres < pressureMin)
                    {
                        DevicesOfType(vent).WithName(acV2).Mode = OUT;
                        DevicesOfType(vent).WithName(acV2).On = 1;
                    }

                    // Pump air out if presssure is >110kPa
                    if(pres > pressureMax)
                    {
                        DevicesOfType(pump).WithName(acV2).Mode = IN;
                        DevicesOfType(pump).WithName(acV2).On = 1;
                    }

                    if(pres < 101 & pres > 99 & ratioO2 > 0.2)
                    {
                        DevicesOfType(pump).WithName(acV2).On = 0;
                    }

                    // Pump CO2 in if ratio is <20%
                    if(ratioCO2 < ratioCO2Min)
                    {
                        DevicesOfType(filtration).WithName(co2).On = 1;
                    }
                    else
                    {
                        DevicesOfType(filtration).WithName(co2).On = 0;
                    }

                    // Pump POL and NO2 out if either ratio is >0.00001
                    if(ratioPOL > ratioPOLMax | ratioNO2 > ratioNO2Max)
                    {
                        DevicesOfType(filtration).WithName(pol).On = 1;
                    }
                    else
                    {
                        DevicesOfType(filtration).WithName(pol).On = 0;
                    }

                    // Conect radiator to cold if temp >27
                    if(temp > tempMax)
                    {
                        tempGood = 0;
                        DevicesOfType(valve).WithName(cold).On = 1;
                    }
                    else
                    {
                        if(temp < tempMid)
                        {
                            tempGood = 1;
                            DevicesOfType(valve).WithName(cold).On = 0;
                        }
                    }

                    // Conect ratiator to hot if temp <24
                    if(temp < tempMin)
                    {
                        tempGood = 0;
                        DevicesOfType(valve).WithName(hot).On = 1;
                    }
                    else
                    {
                        if(temp > tempMid)
                        {
                            tempGood = 1;
                            DevicesOfType(valve).WithName(hot).On = 0;
                        }
                    }

                    // If temp is ok radiator should be in vaccum
                    if(tempGood == 1)
                    {
                        if(analyzer.Pressure > 0)
                        {
                            DevicesOfType(pump).WithName(acV).On = 1;
                        }
                        else
                        {
                            DevicesOfType(pump).WithName(acV).On = 0;
                        }
                    }
                }
            }
        ";

        var compileText = Program.CompileText(code);

        var program = compileText.Split("\n");
        Console.WriteLine(compileText);

        Assert.IsTrue(program.Length <= 69, $"Program is too long: {program.Length} lines");
    }


    [TestMethod]
    public void TestHarvie_GeneratesSameOrShorterProgram()
    {
        var code = @"
            pin Light d0;
            pin Sun d1;

            const HARVIE = 958056199;
            const Harvie1 = ""Harvie1"";
            const Harvie2 = ""Harvie2"";

            const TRAY = -1841632400;
            const Tray1 = ""Tray1"";
            const Tray2 = ""Tray2"";

            const TimeThres = 115;

            void light()
            {
                var lght = Sun.SolarAngle < TimeThres ? 1 : 0;
                Light.On = lght;
            }

            void plant1()
            {
                var temp = 0;
                yield;
                temp = DevicesOfType(HARVIE).WithName(Harvie1).Slots[0].Occupied.Maximum;
                if(temp != 0)
                {
                    DevicesOfType(HARVIE).WithName(Harvie1).Plant = 1;
                    sleep(10);
                }
            }

            void plant2()
            {
                var temp = 0;
                yield;
                temp = DevicesOfType(HARVIE).WithName(Harvie2).Slots[0].Occupied.Maximum;
                if(temp != 0)
                {
                    DevicesOfType(HARVIE).WithName(Harvie2).Plant = 1;
                    sleep(10);
                }
            }

            void harvest1()
            {
                DevicesOfType(HARVIE).WithName(Harvie1).Harvest = 1;
                sleep(10);
            }

            void harvest2()
            {
                DevicesOfType(HARVIE).WithName(Harvie2).Harvest = 1;
                sleep(10);
            }

            real getMature(idx)
            {
                var name = idx == 1 ? Tray1 : Tray2;
                return DevicesOfType(TRAY).WithName(name).Slots[0].Mature.Maximum;
            }

            real getSeeding(idx)
            {
                var name = idx == 1 ? Tray1 : Tray2;
                return DevicesOfType(TRAY).WithName(name).Slots[0].Seeding.Maximum;
            }

            void Main()
            {
                while(true)
                {
                    yield;
                    light();

                    
                    if(getMature(1) == -1)
                    {
                        plant1();
                    }

                    if(getMature(2) == -1)
                    {
                        plant2();
                    }

                    if(getSeeding(1) > -1)
                    {
                        harvest1();
                    }

                    if(getSeeding(2) > -1)
                    {
                        harvest2();
                    }
                }
            }
        ";

        var compileText = Program.CompileText(code);

        var program = compileText.Split("\n");
        Console.WriteLine(compileText);

        Assert.IsTrue(program.Length <= 91, $"Program is too long: {program.Length} lines");
    }
}