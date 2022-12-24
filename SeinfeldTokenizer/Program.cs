using System.Runtime.CompilerServices;

namespace SeinfeldTokenizer
{
    using static Extensions;

    using Yarn = ReadOnlyMemory<char>;
    internal class Program
    {
        [Flags]
        enum Classification : ushort
        {
            None,
            DoStuff,
            SaveStuff,
            BeginLine,
            BeginBody,
            WhiteSpace,
            OpenShrug,
            CloseShrug,
            PlzWhen,
            OrJust,
            Thanks,

            Thingy,
            Ownership,
            Divide,
            Multiply,
            Add,
            Subtract,
            FuncTM,
            TypeC,
            VariableR,
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
        static void Main(string[] args)
        {
            char[] input = File.ReadAllText("TestCode.txt").ToCharArray();

            (string, Classification)[] values =
            {
                (";", Classification.BeginLine),
                ("->", Classification.DoStuff),
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
                ("®", Classification.VariableR),
                ("xXx", Classification.Multiply),
                ("x:-x", Classification.Divide),
                ("x+*-1x", Classification.Subtract),
                //("xlx", Classification.d),
                ("x+x", Classification.Add),
                ("/", Classification.Also),
                ("'s", Classification.Ownership),
                ("_", Classification.Discard),
            };
            //enum Classification

            var specialInputs = new (Func<Yarn, bool, Result> isValid, Classification specialType)[]
            {
                (IsValidName, Classification.Thingy),
                (IsValidNumber, Classification.Number),
                //(IsValidText, Classification.Text | Classification.RequirementAfter | Classification.RequirementBefore),
                //(IsValidComment, Classification.Comment, Classification.RequirementBefore)
            };
            var cash = Tokenizer.ATM(input, values, specialInputs, Classification.WhiteSpace);
        }
    }
}