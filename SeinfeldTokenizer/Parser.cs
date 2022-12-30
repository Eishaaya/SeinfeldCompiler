global using static SeinfeldCompiler.Extensions;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SeinfeldCompiler
{

    static partial class Extensions
    {
        public static void PrintColor(ConsoleColor color, string text)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void PrintColorLine(ConsoleColor color, string text)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    
    internal class Parser : Cry
    {
        public static List<State<TClassification>> Parse<TClassification>(in List<Token<TClassification>> tokens, in IEnumerable<TClassification> ignoredClassifications, StartState<TClassification> startState) where TClassification : Enum
        {
            int index = 0;
            int depth = 0;
            if (startState.TryBuildTree(tokens, ignoredClassifications.ToHashSet(), ref index, out _, out _))
            {
                Console.WriteLine("CST:");
                startState.Print();

                Console.WriteLine("AST:");

                startState.Compress(ref depth, out _);
                startState.Print();
                return startState.Children;
            }
            throw new PuertoRicoException("Parse failed :(");


            //static TerminalState<TClassification> Compress(State<TClassification> head)
            //{
            //    //throw new NotImplementedException("Parse good, I am lazy though");
            //    foreach (var child in head.Children)
            //    {

            //    }
            //}
        }
    }
}
