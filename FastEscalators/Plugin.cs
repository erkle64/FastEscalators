using C3.ModKit;
using HarmonyLib;
using Unfoundry;
using UnityEngine;

namespace FastEscalators
{
    [UnfoundryMod(GUID)]
    public class Plugin : UnfoundryPlugin
    {
        public const string
            MODNAME = "FastEscalators",
            AUTHOR = "erkle64",
            GUID = AUTHOR + "." + MODNAME,
            VERSION = "0.1.0";

        public static LogSource log;

        public static TypedConfigEntry<float> speedMultiplier;

        public Plugin()
        {
            log = new LogSource(MODNAME);

            new Config(GUID)
                .Group("Multipliers")
                    .Entry(out speedMultiplier, "speedMultiplier", 4.0f, "Multiplier for escalator speeds.")
                .EndGroup()
                .Load()
                .Save();
        }

        public override void Load(Mod mod)
        {
            log.Log($"Loading {MODNAME}");
        }

        [HarmonyPatch]
        public static class Patch
        {
            [HarmonyPatch(typeof(MovingPlatformGO), nameof(MovingPlatformGO.getMovementForce))]
            [HarmonyPostfix]
            public static void MovingPlatformGOgetMovementForce(ref Vector3 __result)
            {
                var speedMultiplier = Plugin.speedMultiplier.Get();
                if (speedMultiplier > 0.0f && speedMultiplier != 1.0f)
                {
                    __result *= speedMultiplier;
                }
            }
        }
    }
}


