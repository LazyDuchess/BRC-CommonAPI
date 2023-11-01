using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;

namespace CommonAPI
{
    /// <summary>
    /// Custom dialogue, for use with custom sequences.
    /// </summary>
    public class CustomDialogue
    {
        public string CharacterName = "";
        public string Dialogue = "";
        /// <summary>
        /// Called when the dialogue pops up.
        /// </summary>
        public Action OnDialogueBegin;
        /// <summary>
        /// Called when the dialogue ends, if the sequence wasn't skipped by the player.
        /// </summary>
        public Action OnDialogueEnd;
        /// <summary>
        /// Whether to end the sequence when this dialogue is over.
        /// </summary>
        public bool EndSequenceOnFinish = false;
        /// <summary>
        /// Dialogue to play immediately after this one.
        /// </summary>
        public CustomDialogue NextDialogue = null;
        public DialogueBehaviour.ShowBarsType ShowBars = DialogueBehaviour.ShowBarsType.OnStartOnly;
        /// <summary>
        /// If this dialogue had a Yes/No prompt, whether the user answered yes.
        /// </summary>
        public bool AnsweredYes = false;
        public CustomDialogue(string characterName, string dialogue, CustomDialogue nextDialogue = null)
        {
            CharacterName = characterName;
            Dialogue = dialogue;
            NextDialogue = nextDialogue;
        }
    }
}
