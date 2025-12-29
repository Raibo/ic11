namespace ic11.Tests;

using ic11.Tests.Utils;

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

        var emulator = EmulatorHelper.Run(code, 1);
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

        var emulator = EmulatorHelper.Run(code, 1);
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

        var emulator = EmulatorHelper.Run(code, 1);
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

        var emulator = EmulatorHelper.Run(code, 1);
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

        var emulator = EmulatorHelper.Run(code, 1);
        Assert.AreEqual(45, emulator.Stack.Sum());
    }
}