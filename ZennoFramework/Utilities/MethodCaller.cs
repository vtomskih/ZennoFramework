using System;

namespace ZennoFramework.Utilities
{
    public class MethodCaller
    {
        public Action BeforeCall { get; set; }
        public Action AfterCall { get; set; }

        public void Call(Action action)
        {
            BeforeCall?.Invoke();
            action();
            AfterCall?.Invoke();
        }

        public bool TryCall(Action action)
        {
            try
            {
                Call(action);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}