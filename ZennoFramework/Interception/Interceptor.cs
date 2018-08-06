using System;
using ZennoFramework.Interception.Abstractions;

namespace ZennoFramework.Interception
{
    public sealed class Interceptor : IInterceptor
    {
        private readonly Action[] _actions;

        public Interceptor(Action[] actions)
        {
            _actions = actions;
            IsEnabled = true;
        }

        public bool IsEnabled { get; set; }

        public void Execute(object sender)
        {
            if (!IsEnabled)
                return;

            foreach (var action in _actions)
            {
                action?.Invoke();
            }
        }
    }
}