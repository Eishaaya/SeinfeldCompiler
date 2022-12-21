using System.Text;
using System.Text.RegularExpressions;

namespace SeinfeldTokenizer
{
    using static Extensions;

    using Yarn = ReadOnlyMemory<char>;
    enum Classification
    {
        DoStuff,
        SaveStuff,
        BeginLine,
        BeginBody,
        WhiteSpace,
        OpenQuote,
        CloseQuote,
        OpenShrug,
        CloseShrug,
        PlzWhen,
        OrJust,
        Thanks,
        Comment,
        CommentRegionBegin,
        CommentRegionEnd,
        Thingy,
        Ownership,
        Multiply,
        Add,
        FuncTM,
        TypeC,
        VariableR,
        Also
    }


    class PuertoRicoException : ArgumentException
    {
        public PuertoRicoException(string message = "Not cool Kramer")
            : base(message) { }
    }

    class Token<TClassification> where TClassification : Enum
    {
        public Yarn Info { get; }
        public TClassification Type { get; }

        public Token(Yarn info, in TClassification type)
        {
            Info = info;
            Type = type;
        }
        public Token(in char[] data, in int end, in int length, in TClassification type)
            : this(new Yarn(data, end - length + 1, length), type) { }

        public override string ToString() => $"Lexim: {Info}, Class: {Type}";

    }



    internal class Program
    {
        public static List<Token<TClassification>> ATM<TClassification>(in char[] input, in (string text, TClassification type)[] expressionClasses, (Func<Yarn, bool, bool> isValid, TClassification specialType)[] specialClassifications) where TClassification : Enum
        {
            var expressionFunc = GetParamFunc(IsValidExpression, string.Empty, input, 0, 0);
            var specialFunc = GetParamFunc(IsSpecialExpression, specialClassifications[0].isValid, true, Yarn.Empty);
            List<Token<TClassification>> arcadeMoney = new List<Token<TClassification>>();

            int[] originalSpecials = new int[specialClassifications.Length];
            int[] originalResults = new int[expressionClasses.Length];
            List<int> possibleResults = new List<int>();
            for (int i = 0; possibleResults.Count < expressionClasses.Length; possibleResults.Add(i++)) ;
            possibleResults.CopyTo(originalResults);
            possibleResults.CopyTo(0, originalSpecials, 0, originalSpecials.Length);
            int currLength = 0;


            for (int end = 0; end < input.Length; end++)
            {
                char curr = input[end];
                for (int i = 0; i < possibleResults.Count; i++)
                {
                    if ()
                    {
                        possibleResults.RemoveAt(i--);
                    }
                }

                currLength++;
                if (possibleResults.Count == 1 && expressionClasses[possibleResults[0]].text.Length <= currLength)
                {
                    arcadeMoney.Add(new Token<TClassification>(input, end, currLength, expressionClasses[possibleResults[0]].type));
                    possibleResults = new List<int>(originalResults);
                    currLength = 0;
                }
                else if (possibleResults.Count == 0)
                {
                    possibleResults = new List<int>(originalSpecials);
                    var tempYarn = new Yarn(input, end - currLength + 1, currLength);
                    currLength = end - currLength;
                    bool start = true;

                    if (possibleResults.Count > 0)
                    {
                        do
                        {
                            end++;
                            tempYarn = new Yarn(input, end, 1);
                        } while (isValidName(tempYarn));
                        arcadeMoney.Add(new Token<TClassification>(new Yarn(input, end-- - (currLength = end - currLength), currLength), variableClassification));
                        possibleResults = new List<int>(originalResults);
                        currLength = 0;
                    }
                    else throw new PuertoRicoException($"That collection of characters is literally impossible: start: {end - currLength}, end: {end}");
                }
            }
            return arcadeMoney;
        }
        static void EliminateNegatives<T>(IParamFunc<T, bool> chooser, List<int> indices, T[] values)
        {
            for (int i = 0; i < indices.Count; i++)
            {
                if (!chooser.Invoke(values[indices[i]]))
                {
                    indices.RemoveAt(i);
                }
            }
        }
        public static bool IsValidExpression(string lexim, char[] input, int end, int currLength) => lexim.Length <= currLength || lexim[currLength] != input[end];
        public static bool IsSpecialExpression(Func<Yarn, bool, bool> tester, bool full, Yarn yarn) => tester(yarn, full);

        public static bool IsValidName(Yarn yarn, bool full)
        {
            var cry = yarn.Span;
            if (full && (cry[0] < 'A' || cry[0] > 'z' || (cry[0] > 'Z' && cry[0] < 'a')) && cry[0] != '$' && cry[0] != '_' && cry[0] != '@' && cry[0] != ' ' && cry[0] != '\r' && cry[0] != '\n') return false;

            for (int i = 0; i < cry.Length; i++) if ((cry[0] < 'A' || cry[0] > 'z' || (cry[0] > 'Z' && cry[0] < 'a') || (cry[0] < '0' && cry[0] > '9')) && cry[0] != '$' && cry[0] != '_' && cry[0] != '@' && cry[0] != ' ' && cry[0] != '\r' && cry[0] != '\n') return false;

            return true;
        }
        public static bool IsValidNumber(Yarn yarn, bool full)
        {

        }
        static void Main(string[] args)
        {
            char[] input = File.ReadAllText("TestCode.txt").ToCharArray();

            (string, Classification)[] values =
            {
                (";", Classification.BeginLine),
                ("->", Classification.DoStuff),
                ("<-", Classification.SaveStuff),
                ("''", Classification.OpenQuote),
                ("``", Classification.CloseQuote),
                ("//", Classification.Comment),
                ("^", Classification.BeginBody),
                ("plzWhen", Classification.PlzWhen),
                ("orJust", Classification.OrJust),
                ("thanks", Classification.Thanks),
                ("\n", Classification.WhiteSpace),
                ("\r", Classification.WhiteSpace),
                (" ", Classification.WhiteSpace),
                ("\t", Classification.WhiteSpace),
                ("/->", Classification.CommentRegionBegin),
                ("<-\\", Classification.CommentRegionEnd),
                ("¯\\_(", Classification.OpenShrug),
                (")_/¯", Classification.CloseShrug),
                ("™", Classification.FuncTM),
                ("©", Classification.TypeC),
                ("®", Classification.VariableR),
                ("xXx", Classification.Multiply),
                ("x+x", Classification.Add),
                ("/", Classification.Also)
            };
            //enum Classification
            //{
            //    OpenShrug,
            //    CloseShrug,
            //    PlzWhen,
            //    OrJust,
            //    Thanks,
            //    Comment,
            //    CommentRegionBegin,
            //    CommentRegionEnd,
            //    Thingy,
            //    Ownership,
            //    Multiply,
            //    Add,
            //    FuncTM,
            //    TypeC,
            //    VariableR
            //}
            var cash = ATM(in input, values, Classification.Thingy, IsValidName);
        }
    }
}