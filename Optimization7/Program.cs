using BenchmarkDotNet.Running;
using Optimization7;


_ = BenchmarkRunner.Run<EmailMaskingBenchmark>();


// EmailMaskingBenchmark.MaskEmail_V1(input, 2, '*');
// input -> salihcantekin@gmail.com
// output -> sa***********@gmail.com