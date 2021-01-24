using AlmostThere;
using AlmostThere.Storage;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace SearchAndDestroy.Harmony
{
    [HarmonyPatch(typeof(Caravan), "get_NightResting")]
    class Caravan_get_NightResting
    {
        private static float nHours = 5f;
        static bool Prefix(Caravan __instance, ref bool __result)
        {

            if(Base.Instance.GetExtendedDataStorage() is ExtendedDataStorage store)
            {
                if (store.GetExtendedDataFor(__instance) is ExtendedCaravanData data)
                {
                    if (data.forceRest)
                    {
                        __result = true;
                        return false;
                    }
                    if (data.fullyIgnoreRest)
                    {
                        return false;
                    }
                    if (data.almostThere)
                    {
                        var estimatedTicks = (float)CaravanArrivalTimeEstimator.EstimatedTicksToArrive(__instance, allowCaching: false);
                        var restTicksLeft = CaravanNightRestUtility.LeftRestTicksAt(__instance.Tile, Find.TickManager.TicksAbs);
                        estimatedTicks -= restTicksLeft;
                        if (estimatedTicks/GenDate.TicksPerHour < Base.almostThereHours.Value)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;

        }
    }


    [HarmonyPatch(typeof(Caravan), "GetGizmos")]
    class Caravan_GetGizmos
    {
        static void Postfix(Caravan __instance, ref IEnumerable<Gizmo> __result)
        {
            var gizmoList = __result.ToList();
            var store = Base.Instance.GetExtendedDataStorage();
            var caravanData = store.GetExtendedDataFor(__instance);
            if(__instance.Faction == Faction.OfPlayer)
            {
                if(caravanData != null)
                {
                    gizmoList.Add(CreateAlmostThereCommand(__instance, caravanData));
                    gizmoList.Add(CreateIgnoreRestCommand(__instance, caravanData));
                    gizmoList.Add(CreateForceRestCommand(__instance, caravanData));
                }
            }
            __result = gizmoList;
        }

        private static Command_Toggle CreateIgnoreRestCommand(Caravan __instance, ExtendedCaravanData caravanData)
        {
            Command_Toggle command_Toggle = new Command_Toggle();
            command_Toggle.isActive = (() => caravanData.fullyIgnoreRest);
            command_Toggle.toggleAction = delegate
            {
                caravanData.fullyIgnoreRest = !caravanData.fullyIgnoreRest;
                if (caravanData.fullyIgnoreRest)
                {
                    caravanData.forceRest = false;
                    caravanData.almostThere = false;
                }
            };
            command_Toggle.defaultDesc = "AT_Command_DontRest_Description".Translate();
            command_Toggle.icon = ContentFinder<Texture2D>.Get(("UI/" + "DontRest"), true);
            command_Toggle.defaultLabel = "AT_Command_DontRest_Label".Translate();
            return command_Toggle;
        }
        private static Command_Toggle CreateAlmostThereCommand(Caravan __instance, ExtendedCaravanData caravanData)
        {
            Command_Toggle command_Toggle = new Command_Toggle();
            command_Toggle.isActive = (() => caravanData.almostThere);
            command_Toggle.toggleAction = delegate
            {
                caravanData.almostThere = !caravanData.almostThere;
                if (caravanData.almostThere)
                {
                    caravanData.forceRest = false;
                    caravanData.fullyIgnoreRest = false;
                }
            };
            command_Toggle.defaultDesc = "AT_Command_AlmostThere_Description".Translate(Base.almostThereHours.Value);
            command_Toggle.icon = ContentFinder<Texture2D>.Get(("UI/" + "AlmostThere"), true);
            command_Toggle.defaultLabel = "AT_Command_AlmostThere_Label".Translate();
            return command_Toggle;
        }

        private static Command_Toggle CreateForceRestCommand(Caravan __instance, ExtendedCaravanData caravanData)
        {
            Command_Toggle command_Toggle = new Command_Toggle();
            command_Toggle.isActive = (() => caravanData.forceRest);
            command_Toggle.toggleAction = delegate
            {
                caravanData.forceRest = !caravanData.forceRest;
                if (caravanData.forceRest)
                {
                    caravanData.almostThere = false;
                    caravanData.fullyIgnoreRest = false;
                }
            };
            command_Toggle.defaultDesc = "AT_Command_ForceRest_Description".Translate();
            command_Toggle.icon = ContentFinder<Texture2D>.Get(("UI/" + "ForceRest"), true);
            command_Toggle.defaultLabel = "AT_Command_ForceRest_Title".Translate();
            return command_Toggle;
        }
    }

}
