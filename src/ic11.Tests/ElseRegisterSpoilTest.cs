namespace ic11.Tests;

using ic11.Emulator;

[TestClass]
public sealed class ElseRegisterSpoilTest
{
    [TestMethod]
    public void TestElseBranch_Failing()
    {
        var code = @"
            void Main()
            {
                var array = {1, 2, 3}; // The length of the array doesn't matter

                while(true)
                {
                    if(false) // The condition doesn't matter 
                    {
                        Base.Setting = 1;
                    }
                    else
                    {
                        array[2] = 42;
                    }
                }
            }
        ";

        var compileText = Program.CompileText(code);
        Console.WriteLine(compileText);

        var program = compileText.Split("\n");

        Emulator emulator = new(1);
        emulator.LoadProgram(program);

        var dst = emulator.Devices[1];

        var limit = 1000;
        while (dst.GetProperty("Done") != 1 && --limit > 0)
        {
            emulator.Run(1);
            emulator.PrintSummary();
        }

        emulator.PrintSummary();

        Assert.AreEqual(45, emulator.Stack.Sum());
    }


    [TestMethod]
    public void TestElseBranch_Correct()
    {
        var code = @"
            void Main()
            {
                var array = {1, 2, 3}; // The length of the array doesn't matter

                while(true)
                {
                    if(false) // The condition doesn't matter 
                    {
                        
                    }
                    else
                    {
                        Base.Setting = 1;
                        array[2] = 42;
                    }
                }
            }
        ";

        var compileText = Program.CompileText(code);
        Console.WriteLine(compileText);

        var program = compileText.Split("\n");

        Emulator emulator = new(1);
        emulator.LoadProgram(program);

        var dst = emulator.Devices[1];

        var limit = 1000;
        while (dst.GetProperty("Done") != 1 && --limit > 0)
        {
            emulator.Run(1);
            emulator.PrintSummary();
        }

        emulator.PrintSummary();

        Assert.AreEqual(45, emulator.Stack.Sum());
    }


    [TestMethod]
    public void TestElseBranch_Reverse()
    {
        var code = @"
            void Main()
            {
                var array = {1, 2, 3}; // The length of the array doesn't matter

                while(true)
                {
                    if(true) // The condition doesn't matter 
                    {
                        array[2] = 42;
                    }
                    else
                    {
                        Base.Setting = 1;
                    }
                }
            }
        ";

        var compileText = Program.CompileText(code);
        Console.WriteLine(compileText);

        var program = compileText.Split("\n");

        Emulator emulator = new(1);
        emulator.LoadProgram(program);

        var dst = emulator.Devices[1];

        var limit = 1000;
        while (dst.GetProperty("Done") != 1 && --limit > 0)
        {
            emulator.Run(1);
            emulator.PrintSummary();
        }

        emulator.PrintSummary();

        Assert.AreEqual(45, emulator.Stack.Sum());
    }

    [TestMethod]
    public void TestElseBranch_ForLoop()
    {
        var code = @"
            void Main()
            {
                var array = {1, 2, 3}; // The length of the array doesn't matter

                for(;;)
                {
                    if(false) // The condition doesn't matter 
                    {
                        Base.Setting = 1;
                    }
                    else
                    {
                        array[2] = 42;
                    }
                }
            }
        ";

        var compileText = Program.CompileText(code);
        Console.WriteLine(compileText);

        var program = compileText.Split("\n");

        Emulator emulator = new(1);
        emulator.LoadProgram(program);

        var dst = emulator.Devices[1];

        var limit = 1000;
        while (dst.GetProperty("Done") != 1 && --limit > 0)
        {
            emulator.Run(1);
            emulator.PrintSummary();
        }

        emulator.PrintSummary();

        Assert.AreEqual(45, emulator.Stack.Sum());
    }
    
    [TestMethod]
    public void TestElseBranch_ReverseForLoop()
    {
        var code = @"
            void Main()
            {
                var array = {1, 2, 3}; // The length of the array doesn't matter

                for(;;)
                {
                    if(true) // The condition doesn't matter 
                    {
                        array[2] = 42;
                    }
                    else
                    {
                        Base.Setting = 1;
                    }
                }
            }
        ";

        var compileText = Program.CompileText(code);
        Console.WriteLine(compileText);

        var program = compileText.Split("\n");

        Emulator emulator = new(1);
        emulator.LoadProgram(program);

        var dst = emulator.Devices[1];

        var limit = 1000;
        while (dst.GetProperty("Done") != 1 && --limit > 0)
        {
            emulator.Run(1);
            emulator.PrintSummary();
        }

        emulator.PrintSummary();

        Assert.AreEqual(45, emulator.Stack.Sum());
    }
}