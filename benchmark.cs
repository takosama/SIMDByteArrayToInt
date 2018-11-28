using System;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Intrinsics;
namespace ConsoleApp35
{

 public   unsafe class str
    {
        volatile string Str = "98765432";
        volatile byte* ptr;
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
        Vector128<float> _9 = Sse.SetAllVector128((float)9);
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

            var ans = Sse.Add(Sse41.DotProduct(data0f, mul0, 0b11111000), Sse41.DotProduct(data1f, mul1, 0b11111000));

            return Sse41.Extract(Sse2.ConvertToVector128Int32(ans), 3);
        }
        volatile int n;
        volatile int m;
        [BenchmarkDotNet.Attributes.Benchmark]
        public unsafe void _TryParseSIMD()
        {
            n = TryParseSIMD(ptr,out var  _m);
            m = _m;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p">byte配列のポインタ</param>
        /// <param name="n">戻り値</param>
        /// <returns>-1または0 -1失敗</returns>
   public     int TryParseSIMD(byte* p,out int n)
        {
            var tmp = Sse2.LoadVector128(p);
            var tmp1 = Sse.StaticCast<byte, sbyte>(tmp);
            tmp1 = Sse2.Subtract(tmp1, subtmp);

            var data0 = Ssse3.Shuffle(tmp1, mask0);
            var data0f = Sse2.ConvertToVector128Single(Sse.StaticCast<sbyte, int>(data0));

            var data1 = Ssse3.Shuffle(tmp1, mask1);
            var data1f = Sse2.ConvertToVector128Single(Sse.StaticCast<sbyte, int>(data1));

            var ans = Sse2.Add(Sse2.ConvertToVector128Int32(Sse41.DotProduct(data0f, mul0, 0b11111000)), Sse2.ConvertToVector128Int32(Sse41.DotProduct(data1f, mul1, 0b11111000)));
            n = Sse41.Extract(ans, 3);

            var com0 = Sse.CompareGreaterThan(data0f,_9);
            var com1 = Sse.CompareGreaterThan(data1f,_9);

            return Sse41.Extract(Sse.StaticCast<float, int>(Sse41.DotProduct(com0, com1, 0b11111000)), 3);
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
           byte* arr = stackalloc byte[8];
           arr[0] = (byte)'9';
           arr[1] = (byte)'8';
           arr[2] = (byte)'7';
           arr[3] = (byte)'6';
           arr[4] = (byte)'5';
           arr[5] = (byte)'4';
           arr[6] = (byte)'3';
           arr[7] = (byte)'\n';
 
      var m = new str().TryParseSIMD(arr, out var n);

            Console.ReadLine();
        }
    }


}
