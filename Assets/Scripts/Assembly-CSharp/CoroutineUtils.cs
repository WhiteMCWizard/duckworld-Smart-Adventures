using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SLAM;
using UnityEngine;

public static class CoroutineUtils
{
    [CompilerGenerated]
    private sealed class _003CdoWaitForGameEvent_003Ec__Iterator128<T> : IEnumerator, IDisposable, IEnumerator<object> where T : class
    {
        internal bool _003CcanContinue_003E__0;

        internal Action<T> callback;

        internal int _0024PC;

        internal object _0024current;

        internal Action<T> _003C_0024_003Ecallback;

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return _0024current;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return _0024current;
            }
        }

        public bool MoveNext()
        {
            //Discarded unreachable code: IL_008e
            uint num = (uint)_0024PC;
            _0024PC = -1;
            switch (num)
            {
                case 0u:
                    _003CcanContinue_003E__0 = false;
                    callback = (Action<T>)Delegate.Combine(callback, new Action<T>(_003C_003Em__FA));
                    GameEvents.Subscribe(callback);
                    goto case 1u;
                case 1u:
                    if (!_003CcanContinue_003E__0)
                    {
                        _0024current = null;
                        _0024PC = 1;
                        return true;
                    }
                    GameEvents.Unsubscribe(callback);
                    _0024PC = -1;
                    break;
            }
            return false;
        }

        [DebuggerHidden]
        public void Dispose()
        {
            _0024PC = -1;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        internal void _003C_003Em__FA(T evt)
        {
            _003CcanContinue_003E__0 = true;
        }
    }

    public static Coroutine WaitForUnscaledSeconds(float time)
    {
        return StaticCoroutine.Start(doWaitForUnscaledSeconds(time));
    }

    public static Coroutine WaitForGameEvent<T>(Action<T> callback = null) where T : class
    {
        return StaticCoroutine.Start(doWaitForGameEvent(callback));
    }

    public static Coroutine WaitForObjectDestroyed(GameObject obj, Action callback)
    {
        return StaticCoroutine.Start(doWaitForObjectDestroyed(obj, callback));
    }

    private static IEnumerator doWaitForGameEvent<T>(Action<T> callback) where T : class
    {
        bool canContinue = false;
        callback = (Action<T>)Delegate.Combine(callback, (Action<T>)Delegate.CreateDelegate(typeof(Action<T>), evt => _003C_003Em__FA(evt)));
        GameEvents.Subscribe(callback);
        while (!canContinue)
        {
            yield return null;
        }
        GameEvents.Unsubscribe(callback);
    }
    private static IEnumerator doWaitForUnscaledSeconds(float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    private static IEnumerator doWaitForObjectDestroyed(GameObject obj, Action callback)
    {
        while (obj != null)
        {
            yield return null;
        }
        callback();
    }
}

