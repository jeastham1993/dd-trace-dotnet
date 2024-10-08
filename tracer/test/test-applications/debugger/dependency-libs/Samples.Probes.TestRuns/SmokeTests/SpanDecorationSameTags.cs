using System;
using System.Runtime.CompilerServices;

namespace Samples.Probes.TestRuns.SmokeTests
{
    public class SpanDecorationSameTags : IRun
    {
        private const string Json = @"
{
   ""Decorations"":[
      {
         ""When"":{
            ""Str"":null,
            ""Dsl"":null,
            ""Json"":{
                ""gt"":[
                    {
                        ""ref"": ""intLocal""
                    },
                    {
                        ""ref"": ""intArg""
                    }
                ]
            }
         },
         ""Tags"":[
            {
               ""Name"":""SpanDecorationSameTags"",
               ""Value"":{
                  ""Template"":null,
                  ""Segments"":[
                    {
                        ""Str"":null,
                        ""Dsl"":null,
                        ""Json"":{
                            ""ref"": ""arg""
                        }
                    }
                  ]
               }
            },
            {
               ""Name"":""SpanDecorationSameTags"",
               ""Value"":{
                  ""Template"":null,
                  ""Segments"":[
                    {
                        ""Str"":null,
                        ""Dsl"":null,
                        ""Json"":{
                            ""ref"": ""intArg""
                        }
                     }
                  ]
               }
            }
         ]
      }
   ]
}
";

        [MethodImpl(MethodImplOptions.NoInlining)]
        [SpanOnMethodProbeTestData]
        public void Run()
        {
            Console.WriteLine(Annotate(nameof(Run), nameof(Run).Length));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        string Annotate(string arg, int intArg)
        {
            return Method(arg, intArg);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [SpanDecorationMethodProbeTestData(decorationsJson: Json)]
        string Method(string arg, int intArg)
        {
            var intLocal = nameof(Method).Length * 2;
            if (intLocal > intArg)
            {
                Console.WriteLine(intLocal);
            }

            return $"{arg} : {intLocal.ToString()}";
        }
    }
}
