using System.Collections;

namespace CodeBase.Coroutine
{
    public interface ICoroutineRunner
    {
        UnityEngine.Coroutine StartCoroutine(IEnumerator coroutine);
        void StopCoroutine(UnityEngine.Coroutine coroutine);
    }
}