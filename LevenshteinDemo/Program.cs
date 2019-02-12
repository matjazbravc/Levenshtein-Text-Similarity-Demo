using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using Autofac;
using LevenshteinDemo.Services;

namespace LevenshteinDemo
{
    [ExcludeFromCodeCoverage]
    internal static class Program
    {
        private static IContainer Container()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<UnicodeNormalizer>().As<IUnicodeNormalizer>();
            builder.RegisterType<Levenshtein>().As<ILevenshtein>();
            return builder.Build();
        }
        
        private static void Main()
        {
            var execPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (execPath == null)
            {
                Console.WriteLine("Executing assembly location not founded.");
                return;
            }

            // Read Text 1
            var textFile1 = Path.Combine(execPath, @"TestData\TextFile1.txt");
            var text1 = File.ReadAllText(textFile1);

            // Read Text 2
            var textFile2 = Path.Combine(execPath, @"TestData\TextFile2.txt");
            var text2 = File.ReadAllText(textFile2);

            try
            {
                var levenshtein = Container().Resolve<ILevenshtein>();
                
                // Calculate text match percentage
                var startTime = DateTime.Now;
                var percentage = levenshtein.Distance(text1, text2);
                var stopTime = DateTime.Now;
                var duration = stopTime - startTime;

                Console.WriteLine($"Match: {percentage}%"); // 100% = Perfect match, 0% = Totaly different
                Console.WriteLine("Duration :" + duration);
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}