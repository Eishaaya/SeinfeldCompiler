using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeinfeldCompiler
{
    class StartState<TClassification> : State<TClassification> where TClassification : Enum
    {
        State<TClassification> starter;
        HashSet<TClassification> endingTokens;
        public StartState(State<TClassification> starter, HashSet<TClassification> endingTokens)
        {
            this.starter = starter;
            this.endingTokens = endingTokens;
        }

        public override StartState<TClassification> Clone() => new StartState<TClassification>(starter, endingTokens);
        public override string ToString() => "##STARTSTATE##";

        public override TerminalState<TClassification>? Compress(ref int depth, out bool flat)
        {
            flat = true;
            var origCount = Children.Count;
            for (int i = 0; i < origCount; i++)
            {
                var newKid = Children[i].Compress(ref depth, out var currFlat);
                if (newKid == null)
                {
                    if (flat)
                    {
                        Children.AddRange(Children[i].Children);
                    }
                    Children.RemoveAt(i);
                    continue;
                }
                Children[i] = newKid;
            }
            return null;
        }

        internal override bool TryBuildTree(List<Token<TClassification>> tokens, HashSet<TClassification> ignoredClassifications, ref int tokenIndex, out State<TClassification>? state, out int depth, string tab = "")
        {
            state = this;
            PrintColorLine(ConsoleColor.Blue, $"{tab}--Starting next line--");
            var origIndex = tokenIndex;
            while (starter.Clone().TryBuildTree(tokens, ignoredClassifications, ref tokenIndex, out var child, out depth, tab))
            {
                origIndex = tokenIndex;
#nullable disable
                Children.Add(child);
#nullable enable
                if (tokenIndex >= tokens.Count) return true;
                PrintColorLine(ConsoleColor.Blue, $"{tab}--Starting next line--");
            }
            if (tokenIndex >= tokens.Count) return true;
            if (endingTokens.Contains(tokens[tokenIndex].Type))
            {
                tokenIndex = origIndex;
                return true;
            }
            return false;
        }
    }
}
