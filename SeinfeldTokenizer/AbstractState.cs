using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeinfeldCompiler
{
    abstract class State<TClassification> where TClassification : Enum
    {
        public List<State<TClassification>> Children { get; internal set; } = new List<State<TClassification>>();
        public virtual bool Terminal => false;
        public abstract State<TClassification> Clone();
#nullable disable
        protected State() { }
#nullable enable
        public virtual void Print(string tab = "")
        {
            Console.WriteLine(tab + this);
            foreach (var kid in Children)
            {
                kid.Print($"{tab}|    ");
            }
        }
        protected State(List<State<TClassification>> children) { Children = children; }
        internal abstract bool TryBuildTree(List<Token<TClassification>> tokens, HashSet<TClassification> ignoredClassifications, ref int tokenIndex, out State<TClassification>? state, out int depth, string tab = "");
        public abstract TerminalState<TClassification>? Compress(ref int depth, out bool flat);
    }
}