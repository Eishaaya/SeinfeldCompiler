
global using Yarn = System.ReadOnlyMemory<char>;
global using static SeinfeldTokenizer.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

namespace SeinfeldTokenizer
{




    enum Result : byte
    {
        Nuetral,
        Fail,
        Success
    }

    enum Requirements : ushort
    {
        Garbage = 8191,
        WeakSauce = 4096,
        Impartial = 8192,
        Before = 16384,
        After = 32768,
        Important = Impartial | Before | After,
        All = Important | WeakSauce
    }

   
    class PuertoRicoException : ArgumentException
    {
        public PuertoRicoException(string message = "Not cool Kramer")
            : base(message) { }
    }

    class Token<TClassification> where TClassification : Enum
    {
        //IMemoryOwner, for extra sadness
        public int StartIndex { get; }
        public Yarn Info { get; set; }
        public TClassification Type { get; set; }

        public Token(Yarn info, int startIndex, in TClassification type)
        {
            StartIndex = startIndex;
            Info = info;
            Type = type;
        }
        public Token(in char[] data, in int end, in int length, in TClassification type)
            : this(new Yarn(data, end - length + 1, length), end - length + 1, type) { }

        public override string ToString() => $"Lexim: {Info}, Class: {Type}";

    }



    internal class Tokenizer
    {
        /// <summary>
        /// What's the deal with that Seinfeld tokenization? *Applause*
        /// </summary>
        /// <typeparam name="TClassification">Order in terms of priority (larger number = larger value), match the requirements enum</typeparam>
        /// <param name="input">the text to tokenize</param>
        /// <param name="expressionClasses">keywords, symbols and operators</param>
        /// <param name="specialClassifications">Delegates to detect special lexims, like names and literals</param>
        /// <returns></returns>
        /// <exception cref="PuertoRicoException">If your code isn't good, you pulled a Kramer</exception>
        public static List<Token<TClassification>> ATM<TClassification>(char[] input, (string text, TClassification type)[] expressionClasses, (Func<Yarn, bool, Result> isValid, TClassification specialType)[] specialClassifications, TClassification endPriority /*, Dictionary<Classification, Func<char, bool>> properEnds*/) where TClassification : Enum, IConvertible
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

            int success = -1;
            // int prevSuccess;
            for (int end = 0; end < input.Length; end++)
            {

                void AddToken(TClassification newType)
                {
                    void Reset()
                    {
                        possibleResults = new List<int>(originalResults);
                        success = -1;
                        currLength = 0;
                    }
                    //var newToken = new Token<TClassification>(input, end, currLength, type);

#nullable disable //cry
                    TClassification oldType = arcadeMoney.Count > 0 ? arcadeMoney[arcadeMoney.Count - 1].Type : default;

                    if (newType.BetterHasFlag(Requirements.Before))
                    {
                        ushort standardNew = newType.Cut(Requirements.Important);
                        ushort standardClass = oldType.Cut(Requirements.Important);
                        if (newType.BetterHasFlag(Requirements.Impartial) && (standardNew >= standardClass || !oldType.BetterHasFlag(Requirements.After)))
                        {
                            arcadeMoney[arcadeMoney.Count - 1].Type = (TClassification)(object)standardClass;
                            arcadeMoney.Add(new Token<TClassification>(input, end, currLength, (TClassification)(object)standardNew));
                            Reset();
                            return;
                        }
                        if (!oldType.BetterHasFlag(Requirements.Impartial) || standardNew >= standardClass || oldType.BetterHasFlag(Requirements.WeakSauce))
                        {
                            if (!newType.BetterHasFlag(Requirements.Impartial) && (!oldType.BetterHasFlag(Requirements.After) || standardClass != standardNew))
                            {
                                arcadeMoney[arcadeMoney.Count - 1].Type = (TClassification)(object)Requirements.Garbage;
                                Console.WriteLine("Cannot close region that does not exist!");
                            }
                            else arcadeMoney[arcadeMoney.Count - 1].Type = (TClassification)(object)((TClassification)(object)standardClass).Cut(Requirements.WeakSauce); //Stan Cry (Tears of joy)
                        }
                        //cry
                        var oldInfo = arcadeMoney[arcadeMoney.Count - 1];
                        arcadeMoney[arcadeMoney.Count - 1].Info = new Yarn(input, oldInfo.StartIndex, end - oldInfo.StartIndex + 1);
                    }
                    //    neededPrev.HasFlag(Classification.RequirementBefore) && !arcadeMoney[arcadeMoney.Count - 1].Type.Equals(neededPrev)) throw new PuertoRicoException($"Failed to complete: End: {end}, Length: {currLength}");
                    else if (oldType.BetterHasFlag(Requirements.After))
                    {
                        //if (oldType.Cut(Requirements.After) != newType.Cut(Requirements.Before)) throw new PuertoRicoException("Missing required info");

                        var oldInfo = arcadeMoney[arcadeMoney.Count - 1];
                        arcadeMoney[arcadeMoney.Count - 1].Info = new Yarn(input, oldInfo.StartIndex, end - oldInfo.StartIndex + 1);
                    }
                    else
                    {
                        if (((Requirements)(object)newType).Equals(Requirements.Garbage)) Console.WriteLine("Garbage was added");
                        arcadeMoney.Add(new Token<TClassification>(input, end, currLength, newType));
                    }

                    Reset();

#nullable enable
                }

                char curr = input[end];
                expressionFunc.parameter3 = end;
                expressionFunc.parameter4 = currLength;
                EliminateNegatives(expressionFunc, possibleResults, expressionClasses, out var newSuccess);


                //if (possibleResults.Count == 1 && expressionClasses[possibleResults[0]].text.Length <= currLength)
                //{
                //    AddToken(expressionClasses[possibleResults[0]].type);
                //}
                currLength++;
                if (possibleResults.Count == 0)
                {
                    if (currLength > 1)
                    { end--; currLength--; }

                    var tempYarn = new Yarn(input, end - currLength + 1, currLength);
                    possibleResults = new List<int>(originalSpecials);

                    var origLength = end - currLength;
                    specialFunc.parameter3 = tempYarn;
                    specialFunc.parameter2 = true;
                    EliminateNegatives(specialFunc, possibleResults, specialClassifications, out _);
                    specialFunc.parameter2 = false;
                    TClassification foundClass;
                    if (possibleResults.Count > 0)
                    {
                        do
                        {
                            specialFunc.parameter3 = new Yarn(input, end, 1);
                            foundClass = specialClassifications[possibleResults[0]].specialType;
                            EliminateNegatives(specialFunc, possibleResults, specialClassifications, out _);



                            success = newSuccess;
                        } while (possibleResults.Count > 0 && ++end < input.Length); //small death here

                        //arcadeMoney.Add(new Token<TClassification>(new Yarn(input, end-- - (currLength = end - currLength), currLength), foundClass));
                        end--;
                        currLength = end - origLength;
                        AddToken(foundClass);
                    }
                    else if (success != -1)
                    {

                        // arcadeMoney.Add(new Token<TClassification>(new Yarn(input, end-- - currLength + 1, currLength - 1), in expressionClasses[success].type));
                        AddToken(expressionClasses[success].type);
                        //Catch keyword containment case
                        continue;
                    }
                    else
                    {

                        //end--;
                        AddToken((TClassification)(object)Requirements.Garbage);
                    }
                }
                success = newSuccess;// == -1 ? success : newSuccess;
            }

            var lastType = arcadeMoney[arcadeMoney.Count - 1].Type;
            if (lastType.BetterHasFlag(Requirements.After) && !lastType.BetterHasFlag(Requirements.WeakSauce) && lastType.CompareTo(endPriority) > 0)
            {
                Console.WriteLine($"Cannot finish with opened {arcadeMoney[arcadeMoney.Count - 1].Type.Cut(Requirements.After)}");
                arcadeMoney[arcadeMoney.Count - 1].Type = (TClassification)(object)Requirements.Garbage;
            }
            else { arcadeMoney[arcadeMoney.Count - 1].Type = (TClassification)(object)lastType.Cut(Requirements.All); }

            return arcadeMoney;
        }



        static void EliminateNegatives<T, TClass>(IParamFunc<T, Result> chooser, List<int> indices, (T, TClass)[] values, out int success)
        {
            success = -1;
            for (int i = 0; i < indices.Count; i++)
            {
                Result res = chooser.Invoke(values[indices[i]].Item1);
                if (res == Result.Fail)
                {
                    indices.RemoveAt(i--);
                }
                else if (res == Result.Success)
                {
                    success = indices[i];
                }
            }
        }
        public static Result IsValidExpression(string lexim, char[] input, int end, int currLength)
        {


            if (lexim.Length <= currLength || lexim[currLength] != input[end]) return Result.Fail;
            if (lexim.Length == currLength + 1) return Result.Success;
            return Result.Nuetral;
        }
        public static Result IsSpecialExpression(Func<Yarn, bool, Result> tester, bool full, Yarn yarn) => tester(yarn, full);
        //public static Result IsValidHex(Yarn yarn, bool full)
        //{

        //}
    }
}
