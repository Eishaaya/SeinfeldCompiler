using SeinfeldCompiler;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SeinfeldCompiler
{
    internal class MidState<TClassification> : State<TClassification> where TClassification : Enum //TODO: abstract :(
    {
        //    private protected TClassification[] requirements;
        public State<TClassification>[][] Possibilities { get; set; }
        int childIndex = -1;
        public int ChildIndex
        {
            internal get => childIndex;
            set
            {
                if (childIndex == value || (childIndex = value) >= Possibilities.Length) return;

                childIndex = value;
                Children.Clear();
                for (int i = 0; i < Possibilities[childIndex].Length; i++)
                {
                    Children.Add(Possibilities[childIndex][i].Clone());
                }
            }
        }
        public override MidState<TClassification> Clone() => new MidState<TClassification>(displayName, Possibilities);

        //DEPTH & BESTCHILDREN USED TO TRACK LIKELY MEANT FAILURE
        internal override bool TryBuildTree(List<Token<TClassification>> tokens, HashSet<TClassification> ignoredClassifications, ref int tokenIndex, out State<TClassification>? state, out int depth, string tab = "")
        {
            void IgnoreNoise(ref int tokenIndex) { while (tokenIndex < tokens.Count && ignoredClassifications.Contains(tokens[tokenIndex].Type)) tokenIndex++; }
            void PrintSet() { Console.Write($"{tab}\n{tab}Set {childIndex} of {this}: "); foreach (var kid in Children) Console.Write($"{kid}, "); Console.Write($"\n"); }

            var bestChildren = Children;
            // int bestDepth = 0;
            depth = 1;

            for (ChildIndex = 0; ChildIndex < Possibilities.Length; ChildIndex++)
            {
                IgnoreNoise(ref tokenIndex);
                var originalTokenIndex = tokenIndex;
                PrintSet();
                for (int i = 0; i < Children.Count; i++)
                {
                    Console.WriteLine($"{tab}\n{tab}Trying {Children[i]}");
                    if (!Children[i].TryBuildTree(tokens, ignoredClassifications, ref tokenIndex, out state, out var newDepth, tab + "    |"))
                    {
                        PrintColorLine(ConsoleColor.Red, $"{tab}In {this}, Set {childIndex} failed at {tokenIndex}, {Children[i]}):\n{tab}going back {tokenIndex - originalTokenIndex} tokens");
                        goto stanCry;
                    }
                    else
                    {
                        depth = Math.Max(depth, newDepth + 1);//TODO: reset depth on failure
                    }
                    IgnoreNoise(ref tokenIndex);
                }

                PrintColorLine(ConsoleColor.Green, $"{tab}Succeeded at index {tokenIndex} in {this}");
                state = this;
                return true;
            stanCry:;
                tokenIndex = originalTokenIndex;
                //  tokenIndex = origorigtokenIndex;
            }
            PrintColorLine(ConsoleColor.Red, $"{tab}No kids match at {this}");
            state = null;
            return false;
        }
        string displayName;
        public override string ToString()
        {
            return displayName;
        }

        public override TerminalState<TClassification>? Compress(ref int depth, out bool flat)
        {
          
            flat = false;
            if (Children.Count == 0) return null;
            depth++;

            int bestReplacementIndex = -1;
            int lowestDepth = int.MaxValue;
            int originalCount = Children.Count;
            for (int i = 0; i < originalCount; i++)
            {
                int currDepth = depth;

                var newChild = Children[i].Compress(ref currDepth, out var childFlat);
                if (newChild == null) 
                {
                    if (childFlat) Children.AddRange(Children[i].Children);
                    Children.RemoveAt(i);
                    continue;
                }
                if (lowestDepth > currDepth)
                {
                    lowestDepth = currDepth;
                    bestReplacementIndex = i;
                }
                Children[i] = newChild;
              
            }
            if (bestReplacementIndex == -1) return null;
            depth = lowestDepth;
            var replacement = Children[bestReplacementIndex];
            replacement.Children = Children;
            Children.RemoveAt(bestReplacementIndex);
            return (TerminalState<TClassification>)replacement;
        }

        public MidState(string displayName, State<TClassification>[][] possibilities)
            : base(new List<State<TClassification>>())
        {
            this.displayName = displayName;
            this.Possibilities = possibilities;
        }
#nullable disable
        public MidState(string displayName) : base() { this.displayName = displayName; }

#nullable enable

    }
}
