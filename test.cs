using System;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Intrinsics;
namespace ConsoleApp35
{

    class str
    {
        Vector128<sbyte> mask0 = Sse2.SetVector128(
        -1,-1,-1,3,
        -1,-1,-1,2,
        -1,-1,-1,1,
        -1,-1,-1,0
            );

        Vector128<sbyte> mask1 = Sse2.SetVector128(
        -1, -1, -1, 7,
        -1, -1, -1, 6,
        -1, -1, -1, 5,
        -1, -1, -1, 4
            );

        Vector128<sbyte> subtmp = Sse2.SetAllVector128((sbyte)48);
        Vector128<float> mul0 = Sse.SetVector128(10000,100000,1000000,10000000f);
        Vector128<float> mul1 = Sse.SetVector128(1,10,100,1000f);


        public unsafe int Parse(byte* p)
        {
            var tmp = Sse2.LoadVector128(p);
            var tmp1 = Sse.StaticCast<byte, sbyte>(tmp);
            tmp1 = Sse2.Subtract(tmp1, subtmp);

            var data0 = Ssse3.Shuffle(tmp1, mask0);
            var data0f = Sse2.ConvertToVector128Single(Sse.StaticCast<sbyte, int>(data0));

            var data1 = Ssse3.Shuffle(tmp1, mask1);
            var data1f = Sse2.ConvertToVector128Single(Sse.StaticCast<sbyte, int>(data1));

            var ans = Sse.Add(Sse41.DotProduct(data0f, mul0, 0b11111000), Sse41.DotProduct(data1f, mul1, 0b11111000));

            return Sse41.Extract(Sse2.ConvertToVector128Int32(ans), 3);
        }
    }

    
    class Program
    {
     
     unsafe   static void Main(string[] args)
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
            int ans =new str().Parse((byte*)arr);

            Console.WriteLine(ans);
            Console.ReadLine();
        }
    }
   
 
}
