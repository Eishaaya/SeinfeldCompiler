using System;

namespace BaseGameLibrary
{
    public partial class Extensions
    {
        /// <summary>
        /// Shorthand for ParamAction constructor for readability purposes
        /// </summary>
        /// <typeparam name="T1">the type of your parameter</typeparam>
        /// <param name="func">the func</param>
        /// <param name="par1">the paramateter</param>
        /// <returns></returns>
        public static ParamAction<T1> GetParamAction<T1>(Action<T1> func, T1 par1) => new ParamAction<T1>(func, par1);
        public static ParamAction<T1, T2> GetParamAction<T1, T2>(Action<T1, T2> func, T1 par1, T2 par2) => new ParamAction<T1, T2>(func, par1, par2); 
        public static ParamAction<T1, T2, T3> GetParamAction<T1, T2, T3>(Action<T1, T2, T3> func, T1 par1, T2 par2, T3 par3) => new ParamAction<T1, T2, T3>(func, par1, par2, par3);
        public static ParamAction<T1, T2, T3, T4> GetParamAction<T1, T2, T3, T4>(Action<T1, T2, T3, T4> func, T1 par1, T2 par2, T3 par3, T4 par4) => new ParamAction<T1, T2, T3, T4>(func, par1, par2, par3, par4);
        public static ParamAction<T1, T2, T3, T4, T5> GetParamAction<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> func, T1 par1, T2 par2, T3 par3, T4 par4, T5 par5) => new ParamAction<T1, T2, T3, T4, T5>(func, par1, par2, par3, par4, par5);
        public static ParamAction<T1, T2, T3, T4, T5, T6> GetParamAction<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> func, T1 par1, T2 par2, T3 par3, T4 par4, T5 par5, T6 par6) => new ParamAction<T1, T2, T3, T4, T5, T6>(func, par1, par2, par3, par4, par5, par6);

        public static ParamFunc<T1, TReturn> GetParamFunc<T1, TReturn>(Func<T1, TReturn> func, T1 par1) => new ParamFunc<T1, TReturn>(func, par1);
        public static ParamFunc<T1, T2, TReturn> GetParamFunc<T1, T2, TReturn>(Func<T1, T2, TReturn> func, T1 par1, T2 par2) => new ParamFunc<T1, T2, TReturn>(func, par1, par2);
        public static ParamFunc<T1, T2, T3, TReturn> GetParamFunc<T1, T2, T3, TReturn>(Func<T1, T2, T3, TReturn> func, T1 par1, T2 par2, T3 par3) => new ParamFunc<T1, T2, T3, TReturn>(func, par1, par2, par3);
        public static ParamFunc<T1, T2, T3, T4, TReturn> GetParamFunc<T1, T2, T3, T4, TReturn>(Func<T1, T2, T3, T4, TReturn> func, T1 par1, T2 par2, T3 par3, T4 par4) => new ParamFunc<T1, T2, T3, T4, TReturn>(func, par1, par2, par3, par4);
        public static ParamFunc<T1, T2, T3, T4, T5, TReturn> GetParamFunc<T1, T2, T3, T4, T5, TReturn>(Func<T1, T2, T3, T4, T5, TReturn> func, T1 par1, T2 par2, T3 par3, T4 par4, T5 par5) => new ParamFunc<T1, T2, T3, T4, T5, TReturn>(func, par1, par2, par3, par4, par5);
        public static ParamFunc<T1, T2, T3, T4, T5, T6, TReturn> GetParamFunc<T1, T2, T3, T4, T5, T6, TReturn>(Func<T1, T2, T3, T4, T5, T6, TReturn> func, T1 par1, T2 par2, T3 par3, T4 par4, T5 par5, T6 par6) => new ParamFunc<T1, T2, T3, T4, T5, T6, TReturn>(func, par1, par2, par3, par4, par5, par6);
    }
}
