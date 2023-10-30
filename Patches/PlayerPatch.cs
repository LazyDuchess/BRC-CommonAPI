using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using Reptile;

namespace CommonAPI.Patches
{
    [HarmonyPatch(typeof(Player))]
    internal static class PlayerPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(Player.Init))]
        private static void Init_Postfix(Player __instance)
        {
            CustomPlayerComponent.Attach(__instance);
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(Player.UpdateSprayCanShake))]
        private static void UpdateSprayCanShake_Prefix(Player __instance)
        {
            var customPlayerComponent = CustomPlayerComponent.Get(__instance);
            if (!customPlayerComponent)
                return;
            __instance.talkContextAvailable += customPlayerComponent.CustomInteractableContextAvailable;
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(Player.UpdateSprayCanShake))]
        private static void UpdateSprayCanShake_Postfix(Player __instance)
        {
            var customPlayerComponent = CustomPlayerComponent.Get(__instance);
            if (!customPlayerComponent)
                return;
            __instance.talkContextAvailable -= customPlayerComponent.CustomInteractableContextAvailable;
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(Player.ContextIconsUpdate))]
        private static void ContextIconsUpdate_Postfix(Player __instance)
        {
            if (__instance.graffitiContextAvailable > 0)
                return;
            if (__instance.talkContextAvailable > 0)
                return;
            if (__instance.breakChainContextAvailable > 0)
                return;
            var customPlayerComponent = CustomPlayerComponent.Get(__instance);
            if (!customPlayerComponent)
                return;
            customPlayerComponent.CustomInteractableContextAvailable = Mathf.Max(customPlayerComponent.CustomInteractableContextAvailable - 1, 0);
            var contextAvailable = customPlayerComponent.CustomInteractableContextAvailable > 0;
            var interactable = customPlayerComponent.CurrentCustomInteractable;
            if (contextAvailable && interactable)
            {
                __instance.ui.breakChainsContextIcon.gameObject.SetActive(false);
                __instance.ui.contextGraffitiIcon.gameObject.SetActive(false);
                __instance.ui.contextTalkIcon.gameObject.SetActive(false);
                customPlayerComponent.CustomContextIcon.gameObject.SetActive(false);
                __instance.ui.contextLabelUiButtonGlyphComponent.SetButtonTextGlyph(10);
                __instance.ui.ShowContextLabelUI();
                switch(interactable.Icon)
                {
                    default:
                    case InteractableIcon.Talk:
                        __instance.ui.contextTalkIcon.gameObject.SetActive(true);
                        break;
                    case InteractableIcon.Graffiti:
                        __instance.ui.contextGraffitiIcon.gameObject.SetActive(true);
                        break;
                    case InteractableIcon.Custom:
                        customPlayerComponent.CustomContextIcon.gameObject.SetActive(true);
                        customPlayerComponent.CustomContextIcon.sprite = interactable.CustomIcon;
                        break;
                }
            }
        }
    }
}
