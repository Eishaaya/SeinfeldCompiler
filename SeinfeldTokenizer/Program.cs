using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace SeinfeldCompiler
{
    using Terminal = TerminalState<Program.Classification>;
    using Mid = MidState<Program.Classification>;
    using AbstractState = State<Program.Classification>;
    using static Extensions;

    using Yarn = ReadOnlyMemory<char>;

    static partial class Extensions
    {
        internal static Terminal GetTerminal(this Program.Classification me) => new Terminal(me);
    }

    internal class Program
    {
        [Flags]
        internal enum Classification : ushort
        {
            None,


            SaveStuff,
            BeginLine,
            BeginBody,
            WhiteSpace,

            FuncTM,
            TypeC,
            DataType,

            DoStuff,

            OpenShrug,
            CloseShrug,
            PlzWhen,
            Loop,
            OrJust,
            Thanks,

            Thingy,
            Ownership,
            Divide,
            Multiply,
            Add,
            Subtract,
            IsSame,

            Also,
            Number,
            Discard,
            //Hex,
            //Binary,

            Text,

            Comment,
            Garbage = Requirements.Garbage,
            WeakSauce = Requirements.WeakSauce,
            Impartial = Requirements.Impartial,
            RequirementBefore = Requirements.Before,
            RequirementAfter = Requirements.After,
        }

        public static Result IsValidText(Yarn yarn, bool full)
        {
            foreach (var thread in yarn.Span) if (thread == '`' || thread == '\'' || thread == '\n' || thread == '\r') return Result.Fail;
            return Result.Nuetral;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IsNum(char cry) => cry >= '0' & cry <= '9';
        static bool IsHexNum(char cry) => IsNum(cry) || (cry >= 'A' && cry <= 'F') || (cry >= 'a' && cry <= 'f');
        static bool IsLetter(char cry) => (cry >= 'a' & cry <= 'z') || (cry >= 'A' & cry <= 'Z');
        public static Result IsValidName(Yarn yarn, bool full)
        {
            var cry = yarn.Span;
            if (full && !IsLetter(cry[0]) && cry[0] != '$' && cry[0] != '_' && cry[0] != '@') return Result.Fail;

            for (int i = 0; i < cry.Length; i++) if (!IsLetter(cry[i]) && !IsNum(cry[i]) && cry[i] != '$' && cry[i] != '_' && cry[i] != '@') return Result.Fail;

            return Result.Nuetral;
        }
        public static Result IsValidNumber(Yarn yarn, bool full)
        {
            var cry = yarn.Span;
            //if (full && !IsNum(cry[0]) && cry[0] != 'x' || (yarn.Length > 1 && cry[0] != 'b')) return Result.Fail;

            foreach (var supaCry in cry) if (!IsNum(supaCry)) return Result.Fail;

            return Result.Nuetral;
        }
        static Mid NewMid(string name) => new Mid(name);
        static Mid NewMid(string name, params AbstractState[] states)
        {
            var returner = new Mid(name) { Possibilities = new AbstractState[states.Length][] };
            for (int i = -1; ++i < states.Length;)
            {
                returner.Possibilities[i] = new AbstractState[] { states[i] };
            }
            return returner;
        }
        static void Main(string[] args)
        {
            string sample = "TestCode.txt";
            char[] input = File.ReadAllText(sample).ToCharArray();

            FileInfo info = new FileInfo(sample);
            var fileAge = (DateTime.Now - info.LastWriteTime).TotalMinutes;
            if (fileAge <= .5)
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
            }
            else if (fileAge >= 120)
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Black;
            }
            Console.Clear();

            #region tokenSetup

            (string, Classification)[] values =
            {
                (";", Classification.BeginLine),
                ("->", Classification.DoStuff | Classification.RequirementBefore | Classification.Impartial),
                ("<-", Classification.SaveStuff),
                ("''", Classification.Text | Classification.RequirementAfter),
                ("``", Classification.Text | Classification.RequirementBefore),
                ("//", Classification.Comment | Classification.RequirementAfter | Classification.Impartial | Classification.WeakSauce),
                ("^", Classification.BeginBody),
                ("plzWhen", Classification.PlzWhen),
                ("orJust", Classification.OrJust),
                ("thanks", Classification.Thanks),
                ("\n", Classification.WhiteSpace),
                ("\r", Classification.WhiteSpace | Classification.RequirementBefore | Classification.Impartial),
                (" ", Classification.WhiteSpace),
                ("\t", Classification.WhiteSpace),
                ("/->", Classification.Comment | Classification.RequirementAfter | Classification.Impartial),
                ("<-\\", Classification.Comment | Classification.RequirementBefore),
                ("¯\\_(", Classification.OpenShrug),
                (")_/¯", Classification.CloseShrug),
                ("™", Classification.FuncTM),
                ("©", Classification.TypeC),
                ("®", Classification.DataType | Classification.RequirementAfter),
                ("#X#", Classification.Multiply),
                ("#:-#", Classification.Divide),
                ("#+*-1#", Classification.Subtract),
                //("xlx", Classification.d),
                ("#+#", Classification.Add),
                ("/", Classification.Also),
                ("'s", Classification.Ownership),
                ("_", Classification.Discard),
                ("???", Classification.IsSame),
                ("(╯°□°)╯︵ ┻━┻", Classification.Loop),
            };
            //enum Classification

            var specialInputs = new (Func<Yarn, bool, Result> isValid, Classification specialType)[]
            {
                (IsValidName, Classification.Thingy),
                (IsValidNumber, Classification.Number),
                //(IsValidText, Classification.Text | Classification.RequirementAfter | Classification.RequirementBefore),
                //(IsValidComment, Classification.Comment, Classification.RequirementBefore)
            };
            #endregion
            var cash = Tokenizer.ATM(input, values, specialInputs, Classification.WhiteSpace);

            //  var whiteState = new Terminal(Classification.WhiteSpace);
            var beginlnState = new Terminal(Classification.BeginLine);
            var IDstate = new Terminal(Classification.Thingy);
            var NumberState = new Terminal(Classification.Number);
            var addState = new Terminal(Classification.Add);
            var multiplyState = new Terminal(Classification.Multiply);
            var subtractState = new Terminal(Classification.Subtract);
            var divideState = new Terminal(Classification.Divide);
            var saveStuffArrow = Classification.SaveStuff.GetTerminal();
            var typeState = Classification.DataType.GetTerminal();
            Terminal doStuffArrow = Classification.DoStuff;
            Terminal thanksState = Classification.Thanks;
            Terminal orJustState = Classification.OrJust;
            Terminal plzWhenState = Classification.PlzWhen;
            Terminal textState = Classification.Text;
            Terminal checkEquality = Classification.IsSame;
            Terminal tableFlipState = Classification.Loop;
            Terminal leftShrug = Classification.OpenShrug;
            Terminal rightShrug = Classification.CloseShrug;

            var expressionState = NewMid("expression");
            var valueState = NewMid("value");
            var lowPriorityMath = NewMid("lowMath");
            var midPriorityMath = NewMid("midMath");
            var highPriorityMath = NewMid("HighMath");
            var lineState = NewMid("line");
            var setState = NewMid("setting");
            var startingState = new StartState<Classification>(lineState, new HashSet<Classification>());
            var bodyState = new StartState<Classification>(lineState, new HashSet<Classification>() { Classification.Thanks, Classification.OrJust });
            var conditionalEnding = NewMid("conditionalEnd");
            var body = NewMid("body");
            var midMathSymbol = NewMid("highSymbol", multiplyState, divideState);
            var lowMathSymbol = NewMid("lowSymbol", addState, subtractState);
            var creationTailState = NewMid("creationTail", setState, IDstate);
            var blank = new Mid("blank") { Possibilities = new AbstractState[][] { new AbstractState[0] } };
            var lowMathHelper = NewMid("lowHelper");
            var highMathHelper = NewMid("highHelper");
            var conditional = NewMid("conditional", tableFlipState, plzWhenState);
                       
            lowPriorityMath.Possibilities = new AbstractState[][]
            {
                new AbstractState[] { midPriorityMath, lowMathHelper },
            };
            lowMathHelper.Possibilities = new AbstractState[][]
            {
                new AbstractState[] {lowMathSymbol, lowPriorityMath},
                new AbstractState[] { blank }
            };
            var midMathHelper = NewMid("midHelper");
            midMathHelper.Possibilities = new AbstractState[][]
            {
                 new AbstractState[] { midMathSymbol, midPriorityMath},
                new AbstractState[] { blank }
            };
            midPriorityMath.Possibilities = new AbstractState[][]
            {
               new AbstractState[] { highPriorityMath, midMathHelper}
            };
            highPriorityMath.Possibilities = new AbstractState[][] { new AbstractState[] { valueState, highMathHelper } };
            highMathHelper.Possibilities = new AbstractState[][]
            {
                new AbstractState[] { checkEquality, highPriorityMath},
                new AbstractState[] { blank }
            };


            conditionalEnding.Possibilities = new AbstractState[][]
            {
                new AbstractState[] { thanksState },
                new AbstractState[] { orJustState, body }
            };
            body.Possibilities = new AbstractState[][]
            {
                new AbstractState[] { bodyState, conditionalEnding }
            };
            lineState.Possibilities = new AbstractState[][]
            {
                new AbstractState[] { beginlnState, typeState, doStuffArrow, creationTailState },
                new AbstractState[] { beginlnState, setState },
                new AbstractState[] { conditional, doStuffArrow, expressionState, body}
            };
            setState.Possibilities = new AbstractState[][]
            {
                new AbstractState[] { IDstate, saveStuffArrow, expressionState}
            };
            expressionState.Possibilities = new AbstractState[][]
            {
                new AbstractState[] { lowPriorityMath },
                //  new AbstractState[] { valueState }
            };
            valueState.Possibilities = new AbstractState[][]
            {
                new AbstractState[] { leftShrug, expressionState, rightShrug },
                new AbstractState[] { setState },
                new AbstractState[] { IDstate },
                new AbstractState[] { NumberState },
                new AbstractState[] { textState },
               // new AbstractState[] { leftShrug, expressionState, rightShrug }
            };


            Parser.Parse(cash, new Classification[] { Classification.WhiteSpace, Classification.Comment }, startingState);
        }
    }
}