using System.Collections.Generic;
using UnityEngine;

namespace Implicitly
{
    public static class YieldInstructionCache
    {
        private static readonly Dictionary<float, WaitForSeconds> s_waitForSecondsCache = new();

        public static YieldInstruction WaitForSeconds(float seconds)
        {
            if (s_waitForSecondsCache.TryGetValue(seconds, out var waitForSeconds))
            {
                return waitForSeconds;
            }

            var instruction = new WaitForSeconds(seconds);
            s_waitForSecondsCache.Add(seconds, instruction);
            return instruction;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnSubsystemRegistration() => s_waitForSecondsCache.Clear();
    }
}
