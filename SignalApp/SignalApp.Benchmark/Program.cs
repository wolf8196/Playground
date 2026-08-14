using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace SignalApp.Benchmark
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // new Test().BitConverterMethod();
            // new Test().ManualMethod();

            var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }

    public class Test
    {
        private readonly ReadOnlySequence<byte> sequence;

        public Test()
        {
            sequence = new ReadOnlySequence<byte>([1, 2, 3, 4]);
        }

        [Benchmark]
        public int BitConverterMethod()
        {
            var sliceSpan = sequence.Slice(0, 4).FirstSpan;
            var i = BinaryPrimitives.ReadInt32LittleEndian(sliceSpan);
            return i;
        }

        [Benchmark]
        public int ManualMethod()
        {
            var sliceSpan = sequence.Slice(0, 4).FirstSpan;
            var i = (int)(sliceSpan[0] | sliceSpan[1] << 8 | sliceSpan[2] << 16 | sliceSpan[3] << 24);
            return i;
        }

        [Benchmark]
        public int ManualAggressiveInliningMethod()
        {
            var sliceSpan = sequence.Slice(0, 4).FirstSpan;
            var i = ManualReadInt32LittleEndian(sliceSpan);
            return i;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ManualReadInt32LittleEndian(ReadOnlySpan<byte> sliceSpan)
        {
            return sliceSpan[0] | sliceSpan[1] << 8 | sliceSpan[2] << 16 | sliceSpan[3] << 24;
        }
    }
}