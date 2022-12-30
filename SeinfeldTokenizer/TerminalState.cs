using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeinfeldCompiler
{
    class TerminalState<TClassification> : State<TClassification> where TClassification : Enum
    {
        public static implicit operator TerminalState<TClassification>(TClassification value) => new TerminalState<TClassification>(value);
        public override TerminalState<TClassification> Clone() => new TerminalState<TClassification>(Value);
        public override bool Terminal => true;
        internal TClassification Value { get; set; }
        public TerminalState(TClassification value)
        {
            this.Value = value;

            //children = new List<ParserNode<TClassification>>();
        }
        public override string ToString()
        {
            return Value.ToString();
        }
#nullable disable
        private TerminalState() { }
#nullable enable

        internal override bool TryBuildTree(List<Token<TClassification>> tokens, HashSet<TClassification> ignoredClassifications, ref int tokenIndex, out State<TClassification>? state, out int depth, string tab = "")
        {
            depth = 1;
            state = this;
            if (tokenIndex >= tokens.Count) { PrintColorLine(ConsoleColor.Magenta, $"{tab}Ran out of Tokens on {this}"); return false; } if(!tokens[tokenIndex].Type.Equals(Value)) return false;
            PrintColorLine(ConsoleColor.Green, $"{tab}{this} SUCCEEDED! at {tokenIndex}");
            tokenIndex++;
            return true;
        }

        public override TerminalState<TClassification> Compress(ref int depth, out bool flat) { flat = false; return this; }
    }
}
