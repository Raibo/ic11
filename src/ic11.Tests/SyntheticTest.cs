namespace ic11.Tests;

[TestClass]
public sealed class SyntheticTest
{
    private readonly Emulator _emulator = new ();

    [TestMethod]
    public void TestSimpleMath()
    {
        var code = @"
            pin Src d0;
            pin Dst d1;

            void Main()
            {
                while(true)
                {
                    yield;
                    
                    var a = Src.A;
                    var b = Src.B;

                    var result = Process(a, b);

                    Dst.Res = result;
                    Dst.Done = 1;
                }
            }

            real Process(a, b)
            {
                return a + b;
            }
        ";

        var compileText = Program.CompileText(code);
        var program = compileText.Split("\n");

        _emulator.LoadProgram(program);
        
        var src = _emulator.Devices[0];
        var dst = _emulator.Devices[1];
        _emulator.Devices[1] = null;
        _emulator.Devices[4] = null;
        _emulator.Devices[2] = null;
        _emulator.Devices[5] = null;
        
        src.SetProperty("A", 1000);
        src.SetProperty("B", 1);
        
        var limit = 1000;
        while (dst.GetProperty("Done") != 1 && limit-- > 0)
            _emulator.Run(1);
        
        Assert.AreEqual(1001, dst.GetProperty("Res"));
        
        _emulator.PrintSummary();
    }
    
    
    [TestMethod]
    public void TestFibonacci()
    {
        var code = @"
            pin Src d0;
            pin Dst d1;

            void Main()
            {
                while(true)
                {
                    yield;
                    
                    var a = Src.A;

                    var result = Fib(a);

                    Dst.Res = result;
                    Dst.Done = 1;
                }
            }

            real Fib(a)
            {
                if (a <= 1)
                    return a;

                var prev1 = Fib(a - 1);
                var prev2 = Fib(a - 2);

                return prev1 + prev2;
            }
        ";

        var compileText = Program.CompileText(code);
        var program = compileText.Split("\n");

        _emulator.LoadProgram(program);
        
        var src = _emulator.Devices[0];
        var dst = _emulator.Devices[1];
        _emulator.Devices[1] = null;
        _emulator.Devices[4] = null;
        _emulator.Devices[2] = null;
        _emulator.Devices[5] = null;
        
        src.SetProperty("A", 25);
        
        var limit = 100000;
        while (dst.GetProperty("Done") != 1 && --limit > 0)
            _emulator.Run(1);
        
        Assert.AreNotEqual(0, limit);
        
        Assert.AreEqual(75025, dst.GetProperty("Res"));
        
        _emulator.PrintSummary();
    }
    
    
    [TestMethod]
    public void TestQuicksort()
    {
        var code = @"
            pin Src d0;
            pin Dst d1;

            void Main()
            {
                var array1 = { 666 };
                var array2 = { 5, 3, 8, 6, 2, 7, 4, 1 };

                QuickSort(array2, 0, 7);
                
                Dst.Done = 1;
                yield;
                Dst.Res = array2[0];
            }

            void QuickSort(arr, low, high)
            {
                yield;
                if (low < high)
                {
                    var pi = Partition(arr, low, high);

                    QuickSort(arr, low, pi - 1);
                    QuickSort(arr, pi + 1, high);
                }
            }

            real Partition(arr, low, high)
            {
                var pivot = arr[high];
                var i = (low - 1);

                for (var j = low; j < high; j = j + 1)
                {
                    if (arr[j] < pivot)
                    {
                        i = i + 1;
                        Swap(arr, i, j);
                    }
                }

                Swap(arr, i + 1, high);
                return i + 1;
            }

            void Swap(arr, i, j)
            {
                var temp = arr[i];
                arr[i] = arr[j];
                arr[j] = temp;
            }
        ";

        var compileText = Program.CompileText(code);
        //Console.WriteLine(compileText);
        var program = compileText.Split("\n");

        _emulator.LoadProgram(program);
        
        var src = _emulator.Devices[0];
        var dst = _emulator.Devices[1];
        _emulator.Devices[1] = null;
        _emulator.Devices[4] = null;
        _emulator.Devices[2] = null;
        _emulator.Devices[5] = null;
        
        var limit = 100000;
        while (dst.GetProperty("Done") != 1 && --limit > 0)
            _emulator.Run(1);
        
        Assert.AreNotEqual(0, limit);
        
        _emulator.PrintSummary();
        
        Assert.AreEqual(1, dst.GetProperty("Res"));
    }


    [TestMethod]
    public void TestActualQuicksort()
    {
        int[] arr = { 5, 3, 8, 6, 2, 7, 4, 1 };
        QuickSort(arr, 0, arr.Length - 1);
            
        Console.WriteLine(String.Join(", ", arr));
        Assert.AreEqual(1, arr[0]);
        
        void QuickSort(int[] arr, int low, int high)
        {
            if (low < high)
            {
                var pi = Partition(arr, low, high);

                QuickSort(arr, low, pi - 1);
                QuickSort(arr, pi + 1, high);
            }
        }

        int Partition(int[] arr, int low, int high)
        {
            var pivot = arr[high];
            var i = (low - 1);

            for (var j = low; j < high; j = j + 1)
            {
                if (arr[j] < pivot)
                {
                    i = i + 1;
                    Swap(arr, i, j);
                }
            }

            Swap(arr, i + 1, high);
            return i + 1;
        }

        void Swap(int[] arr, int i, int j)
        {
            // ReSharper disable once SwapViaDeconstruction
            var temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }
    }
}