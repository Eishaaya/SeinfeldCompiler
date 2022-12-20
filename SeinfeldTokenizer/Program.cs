using System.Text;
using System.Text.RegularExpressions;

namespace SeinfeldTokenizer
{

    using Yarn = ReadOnlyMemory<char>;
    enum Classification
    {
        DoStuff,
        BeginLine,
        BeginBody,
        WhiteSpace,
        OpenRegion,
        CloseRegion,
        PlzWhen,
        OrJust,
        Thanks,
        Comment,        
        Thingy,
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
    }



    internal class Program
    {
        public static List<Token<TClassification>> ATM<TClassification>(in char[] input, in (string text, TClassification type)[] expressionClasses, TClassification variableClassification, Func<Yarn, bool> isValidName) where TClassification : Enum
        {
            List<Token<TClassification>> arcadeMoney = new List<Token<TClassification>>();

            int[] originalResults = new int[expressionClasses.Length];
            List<int> possibleResults = new List<int>();
            for (int i = 0; possibleResults.Count < expressionClasses.Length; possibleResults.Add(i++)) ;
            possibleResults.CopyTo(originalResults);

            int currLength = 0;


            for (int end = 0; end < input.Length; end++)
            {
                char curr = input[end];
                for (int i = 0; i < possibleResults.Count; i++)
                {
                    if (expressionClasses[possibleResults[i]].text.Length <= currLength || expressionClasses[possibleResults[i]].text[currLength] != input[end])
                    {
                        possibleResults.RemoveAt(i--);
                    }
                }
                
                if (possibleResults.Count == 1 && expressionClasses[possibleResults[0]].text.Length <= ++currLength)
                {
                    arcadeMoney.Add(new Token<TClassification>(input, end, currLength, expressionClasses[possibleResults[0]].type));
                    possibleResults = new List<int>(originalResults);
                    currLength = 0;
                }
                else if (possibleResults.Count == 0)
                {
                    var tempYarn = new Yarn(input, end - currLength + 1, currLength);
                    if (isValidName(tempYarn))
                    {
                        arcadeMoney.Add(new Token<TClassification>(tempYarn, variableClassification));
                        possibleResults = new List<int>(originalResults);
                        currLength = 0;
                    }
                    else throw new PuertoRicoException($"That collection of characters is literally impossible: start: {end - currLength}, end: {end}");
                }
            }
            return arcadeMoney;
        }

        public static bool IsValidName(Yarn yarn)
        {
            var cry = yarn.Span;
            if ((cry[0] < 'A' || cry[0] > 'z' || (cry[0] > 'Z' && cry[0] < 'a')) && cry[0] != '$' && cry[0] != '_' && cry[0] != '@') return false;

            for (int i = 0; i < cry.Length; i++) if ((cry[0] < 'A' || cry[0] > 'z' || (cry[0] > 'Z' && cry[0] < 'a') || (cry[0] < '0' && cry[0] > '9')) && cry[0] != '$' && cry[0] != '_' && cry[0] != '@') return false;

            return true;
        }
        static void Main(string[] args)
        {
            char[] input = File.ReadAllText("TestCode.txt").ToCharArray();

            (string, Classification)[] values =
            {
                (";", Classification.BeginLine),
                ("->", Classification.DoStuff),
                ("''", Classification.OpenRegion),
                ("``", Classification.CloseRegion),
                ("//", Classification.Comment),
                ("^", Classification.BeginBody),
                ("plzWhen", Classification.PlzWhen),
                ("orJust", Classification.OrJust),
                ("thanks", Classification.Thanks),
                ("\n", Classification.WhiteSpace),
                ("\r", Classification.WhiteSpace),
                (" ", Classification.WhiteSpace),
                ("\t", Classification.WhiteSpace),
            };
            var cash = ATM(in input, values, Classification.Thingy, IsValidName); 
        }
    }
}