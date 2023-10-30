using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using HarmonyLib;
using DG.Tweening;

namespace CommonAPI.Patches
{
    [HarmonyPatch(typeof(DialogueUI))]
    internal static class DialogueUIPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(DialogueUI.EndDialogue))]
        private static void EndDialogue_Prefix(DialogueUI __instance)
        {
            if (CustomSequenceHandler.instance == null)
                return;
            var customSequenceHandler = CustomSequenceHandler.instance;
            if (customSequenceHandler.CurrentDialogue == null)
                return;
            if (customSequenceHandler.CurrentDialogue.ShowBars == DialogueBehaviour.ShowBarsType.OnStartOnly && __instance.effectsUI != null)
                __instance.effectsUI.HideBars(0.25f, UpdateType.Manual);
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(DialogueUI.OnButtonPressed))]
        private static bool OnButtonPressed_Prefix(DialogueUI __instance, bool yesClicked)
        {
            if (CustomSequenceHandler.instance == null)
                return true;
            var customSequenceHandler = CustomSequenceHandler.instance;
            if (customSequenceHandler.CurrentDialogue == null)
                return true;
            customSequenceHandler.CurrentDialogue.AnsweredYes = yesClicked;
            if (__instance.IsShowingDialogue())
                __instance.EndDialogue();
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(DialogueUI.AbortDialogue))]
        private static void AbortDialogue_Prefix()
        {
            if (CustomSequenceHandler.instance == null)
                return;
            var customSequenceHandler = CustomSequenceHandler.instance;
            if (customSequenceHandler.CurrentDialogue == null)
                return;
            customSequenceHandler.CurrentDialogue.NextDialogue = null;
            customSequenceHandler.CurrentDialogue.OnDialogueEnd = null;
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(DialogueUI.EndDialogue))]
        private static void EndDialogue_Postfix(DialogueUI __instance)
        {
            if (CustomSequenceHandler.instance == null)
                return;
            var customSequenceHandler = CustomSequenceHandler.instance;
            var currentDialogue = customSequenceHandler.CurrentDialogue;
            if (currentDialogue == null)
                return;
            if (currentDialogue.NextDialogue != null)
                customSequenceHandler.StartDialogue(currentDialogue.NextDialogue);
            else
            {
                if (currentDialogue.EndSequenceOnFinish)
                    customSequenceHandler.ExitCurrentSequence();
                customSequenceHandler.CurrentDialogue = null;
            }
            if (currentDialogue.OnDialogueEnd != null)
                currentDialogue.OnDialogueEnd.Invoke();
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(DialogueUI.IsShowingDialogue))]
        private static bool IsShowingDialogue_Prefix(ref bool __result)
        {
            if (CustomSequenceHandler.instance == null)
                return true;
            if (CustomSequenceHandler.instance.CurrentDialogue != null)
            {
                __result = true;
                return false;
            }
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(nameof(DialogueUI.UpdateTextLabel))]
        private static bool UpdateTextLabel_Prefix(DialogueUI __instance, string line)
        {
            if (CustomSequenceHandler.instance == null)
                return true;
            if (CustomSequenceHandler.instance.CurrentDialogue == null)
                return true;
            var dialogue = CustomSequenceHandler.instance.CurrentDialogue;
            if (!string.IsNullOrEmpty(dialogue.CharacterName))
            {
                __instance.characterNameText.text = string.Format("{0}:", dialogue.CharacterName);
                __instance.characterNameTextContainer.SetActive(true);
            }
            else
            {
                __instance.characterNameText.text = string.Empty;
                __instance.characterNameTextContainer.SetActive(false);
            }
            __instance.textLabel.text = line;
            __instance.uiManager.SetLocalizationValueOnDialogueText(__instance.textLabel, __instance.gameTextLocalizer, Array.Empty<string>());
            return false;
        }
    }
}
