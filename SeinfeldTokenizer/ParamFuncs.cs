using System;

namespace BaseGameLibrary
{
    #region Interfaces

    #region Funcs
    
    public interface IParamFunc<TReturn>
    {        
        TReturn Invoke();
        TReturn Call();
    }
    public interface IParamFunc<T1, TReturn> : IParamFunc<TReturn>
    {
        TReturn Invoke(T1 par1);
        TReturn Call(T1 par1);
    }
    public interface IParamFunc<T1, T2, TReturn> : IParamFunc<T1, TReturn>
    {
        TReturn Invoke(T1 par1, T2 par2);
        TReturn Call(T1 par1, T2 par2);
    }
    public interface IParamFunc<T1, T2, T3, TReturn> : IParamFunc<T1, T2, TReturn>
    {
        TReturn Invoke(T1 par1, T2 par2, T3 par3);
        TReturn Call(T1 par1, T2 par2, T3 par3);
    }
    public interface IParamFunc<T1, T2, T3, T4, TReturn> : IParamFunc<T1, T2, T3, TReturn>
    {
        TReturn Invoke(T1 par1, T2 par2, T3 par3, T4 par4);
        TReturn Call(T1 par1, T2 par2, T3 par3, T4 par4);
    }
    public interface IParamFunc<T1, T2, T3, T4, T5, TReturn> : IParamFunc<T1, T2, T3, T4, TReturn>
    {
        TReturn Invoke(T1 par1, T2 par2, T3 par3, T4 par4, T5 par5);
        TReturn Call(T1 par1, T2 par2, T3 par3, T4 par4, T5 par5);
    }
    public interface IParamFunc<T1, T2, T3, T4, T5, T6, TReturn> : IParamFunc<T1, T2, T3, T4, T5, TReturn>
    {
        TReturn Invoke(T1 par1, T2 par2, T3 par3, T4 par4, T5 par5, T6 par6);
        TReturn Call(T1 par1, T2 par2, T3 par3, T4 par4, T5 par5, T6 par6);        
    }

    #endregion

    #region Actions
    public interface IParamAction
    {
        void Invoke();
        void Call();
    }
    public interface IParamAction<T1> : IParamAction
    {
        void Invoke(T1 par1);
        void Call(T1 par1);
    }
    public interface IParamAction<T1, T2> : IParamAction<T1>
    {
        void Invoke(T1 par1, T2 par2);
        void Call(T1 par1, T2 par2);
    }
    public interface IParamAction<T1, T2, T3> : IParamAction<T1, T2>
    {
        void Invoke(T1 par1, T2 par2, T3 par3);
        void Call(T1 par1, T2 par2, T3 par3);
    }
    public interface IParamAction<T1, T2, T3, T4> : IParamAction<T1, T2, T3>
    {
        void Invoke(T1 par1, T2 par2, T3 par3, T4 par4);
        void Call(T1 par1, T2 par2, T3 par3, T4 par4);
    }
    public interface IParamAction<T1, T2, T3, T4, T5> : IParamAction<T1, T2, T3, T4>
    {
        void Invoke(T1 par1, T2 par2, T3 par3, T4 par4, T5 par5);
        void Call(T1 par1, T2 par2, T3 par3, T4 par4, T5 par5);
    }
    public interface IParamAction<T1, T2, T3, T4, T5, T6> : IParamAction<T1, T2, T3, T4, T5>
    {
        void Invoke(T1 par1, T2 par2, T3 par3, T4 par4, T5 par5, T6 par6);
        void Call(T1 par1, T2 par2, T3 par3, T4 par4, T5 par5, T6 par6);
    }
    #endregion

    #endregion

    #region Funcs
    public abstract class ParamFuncBase<T1, TReturn> : IParamFunc<TReturn>, IParamAction<T1>, IParamFunc<T1, TReturn>
    {
        protected T1 parameter1;
        public TReturn Invoke() => Invoke(parameter1);
        public TReturn Call() => Invoke();
        void IParamAction.Call() => Call();
        void IParamAction.Invoke() => Invoke();

        public abstract TReturn Invoke(T1 par1);
        public TReturn Call(T1 par1) => Invoke(parameter1 = par1);

        void IParamAction<T1>.Call(T1 par1) => Call(par1);
        void IParamAction<T1>.Invoke(T1 par1) => Invoke(par1);

        public ParamFuncBase(T1 parameter)
        {
            // containedFunc = func;
            parameter1 = parameter;
        }
    }


    public abstract class ParamFuncBase<T1, T2, TReturn> : ParamFuncBase<T1, TReturn>, IParamAction<T1, T2>, IParamFunc<T1, T2, TReturn>
    {
        protected T2 parameter2;

        protected ParamFuncBase(T1 par1, T2 par2)
            : base(par1)
        {
            parameter2 = par2;
        }
        public override TReturn Invoke(T1 par1) => Invoke(par1, parameter2);
        public abstract TReturn Invoke(T1 par1, T2 Par2);
        public TReturn Call(T1 par1, T2 par2) => Call(par1, parameter2 = par2);
        void IParamAction<T1, T2>.Call(T1 par1, T2 par2) => Call(par1, par2);
        void IParamAction<T1, T2>.Invoke(T1 par1, T2 par2) => Invoke(par1, par2);

    }

    public abstract class ParamFuncBase<T1, T2, T3, TReturn> : ParamFuncBase<T1, T2, TReturn>, IParamAction<T1, T2, T3>, IParamFunc<T1, T2, T3, TReturn>
    {
        protected T3 parameter3;

        protected ParamFuncBase(T1 par1, T2 par2, T3 par3)
            : base(par1, par2)
        {
            parameter3 = par3;
        }

        public override TReturn Invoke(T1 par1, T2 Par2) => Invoke(par1, Par2, parameter3);
        public abstract TReturn Invoke(T1 par1, T2 par2, T3 par3);
        public TReturn Call(T1 par1, T2 par2, T3 par3) => Invoke(par1, par2, parameter3 = par3);
        void IParamAction<T1, T2, T3>.Call(T1 par1, T2 par2, T3 par3) => Call(par1, par2, par3);
        void IParamAction<T1, T2, T3>.Invoke(T1 par1, T2 par2, T3 par3) => Invoke();

    }

    public abstract class ParamFuncBase<T1, T2, T3, T4, TReturn> : ParamFuncBase<T1, T2, T3, TReturn>, IParamAction<T1, T2, T3, T4>, IParamFunc<T1, T2, T3, T4, TReturn>
    {
        protected T4 parameter4;

        protected ParamFuncBase(T1 par1, T2 par2, T3 par3, T4 par4)
            : base(par1, par2, par3)
        {
            parameter4 = par4;
        }

        public override TReturn Invoke(T1 par1, T2 par2, T3 par3) => Invoke(par1, par2, par3, parameter4);
        public abstract TReturn Invoke(T1 par1, T2 par2, T3 par3, T4 par4);
        public TReturn Call(T1 par1, T2 par2, T3 par3, T4 par4) => Invoke(parameter1 = par1, parameter2 = par2, parameter3 = par3, parameter4 = par4);
        void IParamAction<T1, T2, T3, T4>.Call(T1 par1, T2 par2, T3 par3, T4 par4) => Call(par1, par2, par3, par4);
        void IParamAction<T1, T2, T3, T4>.Invoke(T1 par1, T2 par2, T3 par3, T4 par4) => Invoke(par1, par2, par3, par4);
    }

    public abstract class ParamFuncBase<T1, T2, T3, T4, T5, TReturn> : ParamFuncBase<T1, T2, T3, T4, TReturn>, IParamAction<T1, T2, T3, T4, T5>, IParamFunc<T1, T2, T3, T4, T5, TReturn>
    {
        protected T5 parameter5;

        protected ParamFuncBase(T1 par1, T2 par2, T3 par3, T4 par4, T5 par5)
            : base(par1, par2, par3, par4)
        {
            parameter5 = par5;
        }
        public override TReturn Invoke(T1 par1, T2 par2, T3 par3, T4 par4) => Invoke(par1, par2, par3, par4, parameter5);
        public abstract TReturn Invoke(T1 par1, T2 par2, T3 par3, T4 par4, T5 par5);
        public TReturn Call(T1 par1, T2 par2, T3 par3, T4 par4, T5 par5) => Invoke(parameter1 = par1, parameter2 = par2, parameter3 = par3, parameter4 = par4, parameter5 = par5);
        void IParamAction<T1, T2, T3, T4, T5>.Call(T1 par1, T2 par2, T3 par3, T4 par4, T5 par5) => Call(par1, par2, par3, par4, par5);
        void IParamAction<T1, T2, T3, T4, T5>.Invoke(T1 par1, T2 par2, T3 par3, T4 par4, T5 par5) => Invoke(par1, par2, par3, par4, par5);
    }

    public abstract class ParamFuncBase<T1, T2, T3, T4, T5, T6, TReturn> : ParamFuncBase<T1, T2, T3, T4, T5, TReturn>, IParamAction<T1, T2, T3, T4, T5, T6>, IParamFunc<T1, T2, T3, T4, T5, T6, TReturn>
    {
        protected T6 parameter6;

        protected ParamFuncBase(T1 par1, T2 par2, T3 par3, T4 par4, T5 par5, T6 par6)
            : base(par1, par2, par3, par4, par5)
        {
            parameter6 = par6;
        }

        public override TReturn Invoke(T1 par1, T2 par2, T3 par3, T4 par4, T5 par5) => Invoke(par1, par2, par3, par4, par5, parameter6);
        public abstract TReturn Invoke(T1 par1, T2 par2, T3 par3, T4 par4, T5 par5, T6 par6);
        public TReturn Call(T1 par1, T2 par2, T3 par3, T4 par4, T5 par5, T6 par6) => Invoke(parameter1 = par1, parameter2 = par2, parameter3 = par3, parameter4 = par4, parameter5 = par5, parameter6 = par6);
        void IParamAction<T1, T2, T3, T4, T5, T6>.Call(T1 par1, T2 par2, T3 par3, T4 par4, T5 par5, T6 par6) => Call(par1, par2, par3, par4, par5, par6);
        void IParamAction<T1, T2, T3, T4, T5, T6>.Invoke(T1 par1, T2 par2, T3 par3, T4 par4, T5 par5, T6 par6) => Invoke(par1, par2, par3, par4, par5, par6);
    }
    #region Leaves
    public sealed class ParamFunc<T1, TReturn> : ParamFuncBase<T1, TReturn>
    {
        readonly Func<T1, TReturn> containedFunc;
        public override TReturn Invoke(T1 par1)
            => containedFunc(par1);
        public ParamFunc(Func<T1, TReturn> containedFunc, T1 par1)
            : base(par1)
        {
            this.containedFunc = containedFunc;
        }
    }

    public sealed class ParamFunc<T1, T2, TReturn> : ParamFuncBase<T1, T2, TReturn>
    {
        readonly Func<T1, T2, TReturn> containedFunc;
        public override TReturn Invoke(T1 par1, T2 par2)
            => containedFunc(par1, par2);
        public ParamFunc(Func<T1, T2, TReturn> containedFunc, T1 par1, T2 par2)
            : base(par1, par2)
        {
            this.containedFunc = containedFunc;
        }
    }

    public sealed class ParamFunc<T1, T2, T3, TReturn> : ParamFuncBase<T1, T2, T3, TReturn>
    {
        readonly Func<T1, T2, T3, TReturn> containedFunc;
        public override TReturn Invoke(T1 par1, T2 par2, T3 par3)
            => containedFunc(par1, par2, par3);
        public ParamFunc(Func<T1, T2, T3, TReturn> containedFunc, T1 par1, T2 par2, T3 par3)
            : base(par1, par2, par3)
        {
            this.containedFunc = containedFunc;
        }
    }

    public sealed class ParamFunc<T1, T2, T3, T4, TReturn> : ParamFuncBase<T1, T2, T3, T4, TReturn>
    {
        readonly Func<T1, T2, T3, T4, TReturn> containedFunc;
        public override TReturn Invoke(T1 par1, T2 par2, T3 par3, T4 par4)
            => containedFunc(par1, par2, par3, par4);
        public ParamFunc(Func<T1, T2, T3, T4, TReturn> containedFunc, T1 par1, T2 par2, T3 par3, T4 par4)
            : base(par1, par2, par3, par4)
        {
            this.containedFunc = containedFunc;
        }
    }
    public sealed class ParamFunc<T1, T2, T3, T4, T5, TReturn> : ParamFuncBase<T1, T2, T3, T4, T5, TReturn>
    {
        readonly Func<T1, T2, T3, T4, T5, TReturn> containedFunc;
        public override TReturn Invoke(T1 par1, T2 par2, T3 par3, T4 par4, T5 par5)
            => containedFunc(par1, par2, par3, par4, par5);
        public ParamFunc(Func<T1, T2, T3, T4, T5, TReturn> containedFunc, T1 par1, T2 par2, T3 par3, T4 par4, T5 par5)
            : base(par1, par2, par3, par4, par5)
        {
            this.containedFunc = containedFunc;
        }
    }

    public sealed class ParamFunc<T1, T2, T3, T4, T5, T6, TReturn> : ParamFuncBase<T1, T2, T3, T4, T5, T6, TReturn>
    {
        readonly Func<T1, T2, T3, T4, T5, T6, TReturn> containedFunc;
        public override TReturn Invoke(T1 par1, T2 par2, T3 par3, T4 par4, T5 par5, T6 par6)
            => containedFunc(par1, par2, par3, par4, par5, par6);
        public ParamFunc(Func<T1, T2, T3, T4, T5, T6, TReturn> containedFunc, T1 par1, T2 par2, T3 par3, T4 par4, T5 par5, T6 par6)
            : base(par1, par2, par3, par4, par5, par6)
        {
            this.containedFunc = containedFunc;
        }
    }
    #endregion

    #endregion
    //TODO: BEEG FUNC

    #region Actions
    public abstract class ParamActionBase<T1> : IParamAction<T1>
    {
        protected T1 parameter1;

        public void Call()
        {
            Invoke();
        }
        //{
        //   return containedFunc(parameter1);
        //}

        public void Invoke()
        {
            Invoke(parameter1);
        }

        public abstract void Invoke(T1 par1);


        public void Call(T1 par1)
        {
            parameter1 = par1;
            Call();
        }

        public ParamActionBase(T1 parameter)
        {
            // containedFunc = func;
            parameter1 = parameter;
        }
    }


    public abstract class ParamActionBase<T1, T2> : ParamActionBase<T1>, IParamAction<T1, T2>
    {
        protected T2 parameter2;        

        protected ParamActionBase(T1 par1, T2 par2)
            : base(par1)
        {
            parameter2 = par2;
        }

        public sealed override void Invoke(T1 par1)
        {
            Invoke(par1, parameter2);
        }
        public abstract void Invoke(T1 par1, T2 par2);

        public void Call(T1 par1, T2 par2)
        {
            parameter2 = par2;
            Call(par1);
        }
    }

    public abstract class ParamActionBase<T1, T2, T3> : ParamActionBase<T1, T2>, IParamAction<T1, T2, T3>
    {
        protected T3 parameter3;

        protected ParamActionBase(T1 par1, T2 par2, T3 par3)
            : base(par1, par2)
        {
            parameter3 = par3;
        }

        public sealed override void Invoke(T1 par1, T2 par2)
        {
            Invoke(par1, par2, parameter3);
        }

        public abstract void Invoke(T1 par1, T2 par2, T3 par3);

        public void Call(T1 par1, T2 par2, T3 par3)
        {
            parameter3 = par3;
            Call(par1, par2);
        }
    }

    public abstract class ParamActionBase<T1, T2, T3, T4> : ParamActionBase<T1, T2, T3>, IParamAction<T1, T2, T3, T4>
    {
        protected T4 parameter4;

        protected ParamActionBase(T1 par1, T2 par2, T3 par3, T4 par4)
            : base(par1, par2, par3)
        {
            parameter4 = par4;
        }

        public sealed override void Invoke(T1 par1, T2 par2, T3 par3)
        {
            Invoke(par1, par2, par3, parameter4);
        }

        public abstract void Invoke(T1 par1, T2 par2, T3 par3, T4 par4);

        public void Call(T1 par1, T2 par2, T3 par3, T4 par4)
        {
            parameter4 = par4;
            Call(par1, par2, par3);
        }
    }

    public abstract class ParamActionBase<T1, T2, T3, T4, T5> : ParamActionBase<T1, T2, T3, T4>, IParamAction<T1, T2, T3, T4, T5>
    {
        protected T5 parameter5;

        protected ParamActionBase(T1 par1, T2 par2, T3 par3, T4 par4, T5 par5)
            : base(par1, par2, par3, par4)
        {
            parameter5 = par5;
        }

        public sealed override void Invoke(T1 par1, T2 par2, T3 par3, T4 par4)
        {
            Invoke(par1, par2, par3, par4, parameter5);
        }

        public abstract void Invoke(T1 par1, T2 par2, T3 par3, T4 par4, T5 par5);

        public void Call(T1 par1, T2 par2, T3 par3, T4 par4, T5 par5)
        {
            parameter5 = par5;
            Call(par1, par2, par3, par4);
        }
    }

    public abstract class ParamActionBase<T1, T2, T3, T4, T5, T6> : ParamActionBase<T1, T2, T3, T4, T5>, IParamAction<T1, T2, T3, T4, T5, T6>
    {
        protected T6 parameter6;

        protected ParamActionBase(T1 par1, T2 par2, T3 par3, T4 par4, T5 par5, T6 par6)
            : base(par1, par2, par3, par4, par5)
        {
            parameter6 = par6;
        }

        public sealed override void Invoke(T1 par1, T2 par2, T3 par3, T4 par4, T5 par5)
        {
            Invoke(par1, par2, par3, par4, par5, parameter6);
        }

        public abstract void Invoke(T1 par1, T2 par2, T3 par3, T4 par4, T5 par5, T6 par6);

        public void Call(T1 par1, T2 par2, T3 par3, T4 par4, T5 par5, T6 par6)
        {
            parameter6 = par6;
            Call(par1, par2, par3, par4, par5);
        }
    }
    #region Leaves
    public sealed class ParamAction<T1> : ParamActionBase<T1>
    {
        readonly Action<T1> containedAction;
        public override void Invoke(T1 par1)
             => containedAction(par1);
        public ParamAction(Action<T1> containedAction, T1 par1)
            : base(par1)
        {
            this.containedAction = containedAction;
        }
    }

    public sealed class ParamAction<T1, T2> : ParamActionBase<T1, T2>
    {
        readonly Action<T1, T2> containedAction;
        public override void Invoke(T1 par1, T2 par2)
           => containedAction(par1, par2);
        public ParamAction(Action<T1, T2> containedAction, T1 par1, T2 par2)
            : base(par1, par2)
        {
            this.containedAction = containedAction;
        }
    }

    public sealed class ParamAction<T1, T2, T3> : ParamActionBase<T1, T2, T3>
    {
        readonly Action<T1, T2, T3> containedAction;
        public override void Invoke(T1 par1, T2 par2, T3 par3)
            => containedAction(par1, par2, par3);
        public ParamAction(Action<T1, T2, T3> containedAction, T1 par1, T2 par2, T3 par3)
            : base(par1, par2, par3)
        {
            this.containedAction = containedAction;
        }
    }

    public sealed class ParamAction<T1, T2, T3, T4> : ParamActionBase<T1, T2, T3, T4>
    {
        readonly Action<T1, T2, T3, T4> containedAction;
        public override void Invoke(T1 par1, T2 par2, T3 par3, T4 par4)
            => containedAction(par1, par2, par3, par4);
        public ParamAction(Action<T1, T2, T3, T4> containedAction, T1 par1, T2 par2, T3 par3, T4 par4)
            : base(par1, par2, par3, par4)
        {
            this.containedAction = containedAction;
        }
    }
    public sealed class ParamAction<T1, T2, T3, T4, T5> : ParamActionBase<T1, T2, T3, T4, T5>
    {
        readonly Action<T1, T2, T3, T4, T5> containedAction;
        public override void Invoke(T1 par1, T2 par2, T3 par3, T4 par4, T5 par5)
            => containedAction(par1, par2, par3, par4, par5);
        public ParamAction(Action<T1, T2, T3, T4, T5> containedAction, T1 par1, T2 par2, T3 par3, T4 par4, T5 par5)
            : base(par1, par2, par3, par4, par5)
        {
            this.containedAction = containedAction;
        }
    }

    public sealed class ParamAction<T1, T2, T3, T4, T5, T6> : ParamActionBase<T1, T2, T3, T4, T5, T6>
    {
        readonly Action<T1, T2, T3, T4, T5, T6> containedAction;
        public override void Invoke(T1 par1, T2 par2, T3 par3, T4 par4, T5 par5, T6 par6)
            => containedAction(par1, par2, par3, par4, par5, par6);
        public ParamAction(Action<T1, T2, T3, T4, T5, T6> containedAction, T1 par1, T2 par2, T3 par3, T4 par4, T5 par5, T6 par6)
            : base(par1, par2, par3, par4, par5, par6)
        {
            this.containedAction = containedAction;
        }
    }
    #endregion

    #endregion

}
