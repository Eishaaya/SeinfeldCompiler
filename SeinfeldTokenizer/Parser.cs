using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeinfeldTokenizer
{
    class TerminalState<TClassification> : State<TClassification> where TClassification : Enum
    {
        public override TerminalState<TClassification> Clone() => new TerminalState<TClassification>(value);
        public override bool Terminal => true;
        public TerminalState(TClassification value)
        {
            this.value = value;

            //children = new List<ParserNode<TClassification>>();
        }
        private TerminalState() { }
        TClassification value { get; set; }
    }

    abstract class State<TClassification> where TClassification : Enum
    {
        public List<State<TClassification>> Children { get; protected set; }
        public virtual bool Terminal => false;
        public abstract State<TClassification> Clone();
        protected State() { }
        protected State(List<State<TClassification>> children) { Children = children; }

    }

    class MidState<TClassification> : State<TClassification> where TClassification : Enum
    {
        //    private protected TClassification[] requirements;
        public ImmutableArray<State<TClassification>[]> Possibilities { get; private set; }
        int childIndex = 0;

        public override MidState<TClassification> Clone()
        {
            var temp = new MidState<TClassification>();
            temp.Possibilities = Possibilities;
            return temp;
        }

        public int ChildIndex
        {
            protected get => childIndex;
            set
            {
                if (childIndex == value) return;

                childIndex = value;
                Children.Clear();
                for (int i = 0; i < Possibilities[childIndex].Length; i++)
                {
                    Children.Add(Possibilities[childIndex][i].Clone());
                }
            }
        }

        public MidState(State<TClassification>[][] possibilities)
            :base(new List<State<TClassification>>())
        {
            Possibilities = ImmutableArray.Create(possibilities);
            
        }
#nullable disable
        protected MidState() : base() { }
#nullable enable

    }
    internal class Parser : Cry
    {
        public static TerminalState<TClassification> Parse<TClassification>(in List<Token<TClassification>> tokens, MidState<TClassification> startState) where TClassification : Enum
        {
            return Compress(BuildTree(tokens, startState, 0));




            static State<TClassification> BuildTree(List<Token<TClassification>> tokens, MidState<TClassification> currentState, int index)
            {

            }



            static TerminalState<TClassification> Compress(State<TClassification> head)
            {
                throw new NotImplementedException();
            }
        }
        

    }
}
