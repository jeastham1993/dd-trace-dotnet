using System;
using System.Runtime.CompilerServices;

namespace Samples.Probes.TestRuns.ExpressionTests
{
    [LineProbeTestData(27, templateDsl: Dsl, templateJson: Json, templateStr: "Result is: ", captureSnapshot: false)]
    internal class LineTemplatePartialSnapshot : IRun
    {
        private const string Dsl = @"{
  ""dsl"": ""Result is: {ref arg}""
}";

        private const string Json = @"{
        ""ref"": ""arg""
}";

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Run()
        {
            var result = Method(TimeSpan.FromSeconds(1).TotalSeconds);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        string Method(double arg)
        {
            var local = arg + GetInt(arg);
            Console.WriteLine(local);
            return $"Result is: {arg} + {local}";
        }

        int GetInt(double d)
        {
            return (int)(d + 1);
        }
    }
}
