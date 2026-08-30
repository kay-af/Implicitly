using System.Collections;
using UnityEngine;

namespace Implicitly
{
    public class CoroutineRunner : MonoBehaviour
    {
        private const string COROUTINE_RUNNER_NAME = "[Implicitly] Coroutine Runner";

        private static CoroutineRunner s_instance;

        public static Coroutine Run(IEnumerator routine) => s_instance.StartCoroutine(routine);

        public static void Stop(Coroutine routine) => s_instance.StopCoroutine(routine);

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnSubsystemRegistration() => s_instance = null;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad()
        {
            var runner = new GameObject(COROUTINE_RUNNER_NAME).AddComponent<CoroutineRunner>();
            DontDestroyOnLoad(runner);
            s_instance = runner;
        }
    }
}
