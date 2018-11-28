using System;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Intrinsics;
namespace ConsoleApp35
{

    public unsafe class str
    {
        volatile string Str = "98765432";
        volatile byte* ptr;

        Vector128<int>[] mul0Array = new Vector128<int>[9];
        Vector128<int>[] mul1Array = new Vector128<int>[9];
        Vector128<int>[] cmp0Array = new Vector128<int>[9];
        Vector128<int>[] cmp1Array = new Vector128<int>[9];

        public str()
        {
            byte* arr = stackalloc byte[8];
            arr[0] = (byte)'9';
            arr[1] = (byte)'8';
            arr[2] = (byte)'7';
            arr[3] = (byte)'6';
            arr[4] = (byte)'5';
            arr[5] = (byte)'4';
            arr[6] = (byte)'3';
            arr[7] = (byte)'2';
            ptr = arr;

            mul0Array[0] = Sse2.SetVector128((int)0, 0, 0, 0);
            mul0Array[1] = Sse2.SetVector128((int)0, 0, 0, 1);
            mul0Array[2] = Sse2.SetVector128((int)0, 0, 1, 10);
            mul0Array[3] = Sse2.SetVector128((int)0, 1, 10, 100);
            mul0Array[4] = Sse2.SetVector128((int)1, 10, 100, 1000);
            mul0Array[5] = Sse2.SetVector128((int)10, 100, 1000, 10000);
            mul0Array[6] = Sse2.SetVector128((int)100, 1000, 10000, 100000);
            mul0Array[7] = Sse2.SetVector128((int)1000, 10000, 100000, 1000000);
            mul0Array[8] = Sse2.SetVector128((int)10000, 100000, 1000000, 10000000);


            mul1Array[0] = Sse2.SetVector128((int)0, 0, 0, 0);
            mul1Array[1] = Sse2.SetVector128((int)0, 0, 0, 0);
            mul1Array[2] = Sse2.SetVector128((int)0, 0, 0, 0);
            mul1Array[3] = Sse2.SetVector128((int)0, 0, 0, 0);
            mul1Array[4] = Sse2.SetVector128((int)0, 0, 0, 0);
            mul1Array[5] = Sse2.SetVector128((int)0, 0, 0, 1);
            mul1Array[6] = Sse2.SetVector128((int)0, 0, 1, 10);
            mul1Array[7] = Sse2.SetVector128((int)0, 1, 10, 100);
            mul1Array[8] = Sse2.SetVector128((int)1, 10, 100, 1000);

            cmp0Array[0] = Sse2.SetVector128((int)0, 0, 0, 0);
            cmp0Array[1] = Sse2.SetVector128((int)0, 0, 0, 1);
            cmp0Array[2] = Sse2.SetVector128((int)0, 0, 1, 1);
            cmp0Array[3] = Sse2.SetVector128((int)0, 1, 1, 1);
            cmp0Array[4] = Sse2.SetVector128((int)1, 1, 1, 1);
            cmp0Array[5] = Sse2.SetVector128((int)1, 1, 1, 1);
            cmp0Array[6] = Sse2.SetVector128((int)1, 1, 1, 1);
            cmp0Array[7] = Sse2.SetVector128((int)1, 1, 1, 1);


            cmp1Array[0] = Sse2.SetVector128((int)0, 0, 0, 0);
            cmp1Array[1] = Sse2.SetVector128((int)0, 0, 0, 0);
            cmp1Array[2] = Sse2.SetVector128((int)0, 0, 0, 0);
            cmp1Array[3] = Sse2.SetVector128((int)0, 0, 0, 0);
            cmp1Array[4] = Sse2.SetVector128((int)0, 0, 0, 0);
            cmp1Array[5] = Sse2.SetVector128((int)0, 0, 0, 1);
            cmp1Array[6] = Sse2.SetVector128((int)0, 0, 1, 1);
            cmp1Array[7] = Sse2.SetVector128((int)0, 1, 1, 1);
            cmp1Array[8] = Sse2.SetVector128((int)1, 1, 1, 1);
        }

        Vector128<sbyte> mask0 = Sse2.SetVector128(
        -1, -1, -1, 3,
        -1, -1, -1, 2,
        -1, -1, -1, 1,
        -1, -1, -1, 0
            );

        Vector128<sbyte> mask1 = Sse2.SetVector128(
        -1, -1, -1, 7,
        -1, -1, -1, 6,
        -1, -1, -1, 5,
        -1, -1, -1, 4
            );

        Vector128<sbyte> subtmp = Sse2.SetAllVector128((sbyte)48);
        Vector128<int> _9 = Sse2.SetAllVector128((int)9);
        Vector128<float> mul0 = Sse.SetVector128(10000, 100000, 1000000, 10000000f);
        Vector128<float> mul1 = Sse.SetVector128(1, 10, 100, 1000f);


        [BenchmarkDotNet.Attributes.Benchmark]

        public int ParseStr()
        {
            return int.Parse(Str);
        }

        [BenchmarkDotNet.Attributes.Benchmark]
        public unsafe int ParseSIMD()
        {
            var tmp = Sse2.LoadVector128(ptr);
            var tmp1 = Sse.StaticCast<byte, sbyte>(tmp);
            tmp1 = Sse2.Subtract(tmp1, subtmp);

            var data0 = Ssse3.Shuffle(tmp1, mask0);
            var data0f = Sse2.ConvertToVector128Single(Sse.StaticCast<sbyte, int>(data0));

            var data1 = Ssse3.Shuffle(tmp1, mask1);
            var data1f = Sse2.ConvertToVector128Single(Sse.StaticCast<sbyte, int>(data1));

            var ans = Sse2.Add(Sse2.ConvertToVector128Int32(Sse41.DotProduct(data0f, mul0, 0b11111000)), Sse2.ConvertToVector128Int32(Sse41.DotProduct(data1f, mul1, 0b11111000)));
            return Sse41.Extract(ans, 3);

        }
        volatile int n;
        volatile int m;
    
        [BenchmarkDotNet.Attributes.Benchmark]
        public unsafe void _TryParseSIMDUseCount()
        {
            n = TryParseSIMDUseCount(ptr, 8, out var _m);
            m = _m;
        }





        public int TryParseSIMDUseCount(byte* p, int cnt, out int n)
        {
            var tmp = Sse2.LoadVector128(p);
            var tmp1 = Sse.StaticCast<byte, sbyte>(tmp);
            tmp1 = Sse2.Subtract(tmp1, subtmp);

            var data0 = Ssse3.Shuffle(tmp1, mask0);


            var data1 = Ssse3.Shuffle(tmp1, mask1);


            var mul0 = Sse41.MultiplyLow(Sse.StaticCast<sbyte, int>(data0), mul0Array[cnt]);
            var mul1 = Sse41.MultiplyLow(Sse.StaticCast<sbyte, int>(data1), mul1Array[cnt]);
            var x = Sse2.Add(mul0, mul1);
            x = Ssse3.HorizontalAdd(x, x);
            x = Ssse3.HorizontalAdd(x, x);

            n = Sse41.Extract(x, 3);



            var com0 = Sse2.CompareGreaterThan(Sse41.MultiplyLow(Sse.StaticCast<sbyte, int>(data0), cmp0Array[cnt]), _9);
            var com1 = Sse2.CompareGreaterThan(Sse41.MultiplyLow(Sse.StaticCast<sbyte, int>(data0), cmp1Array[cnt]), _9);

            var xx = Sse2.Add(com0, com1);
            xx = Ssse3.HorizontalAdd(xx, xx);
            xx = Ssse3.HorizontalAdd(xx, xx);

            return Sse41.Extract(xx, 3);
        }

        [BenchmarkDotNet.Attributes.Benchmark]

        public unsafe int ParseFor()
        {
            return 10000000 * (Str[0] - 48) +
             1000000 * (Str[1] - 48) +
             100000 * (Str[2] - 48) +
             10000 * (Str[3] - 48) +
             1000 * (Str[4] - 48) +
             100 * (Str[5] - 48) +
             10 * (Str[6] - 48) +
             1 * (Str[7] - 48);
        }
    }


    class Program
    {

        unsafe static void Main(string[] args)
        {
                BenchmarkDotNet.Running.BenchmarkRunner.Run<str>();
          //  byte* arr = stackalloc byte[8];
          //  arr[0] = (byte)'9';
          //  arr[1] = (byte)'8';
          //  arr[2] = (byte)'7';
          //  arr[3] = (byte)'6';
          //  arr[4] = (byte)'5';
          //  arr[5] = (byte)'4';
          //  arr[6] = (byte)'3';
          //  arr[7] = (byte)'A';
          //
          //  var m = new str().TryParseSIMDUseCount(arr, 8, out var n);
          //  //戻り値-1(エラー  n=適当な値
          //
          //  m = new str().TryParseSIMDUseCount(arr, 7, out n);
          //  //戻り値0(通過　n=9876543


            Console.ReadLine();
        }
    }
}
