using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;

namespace CommonAPI
{
    public class CustomDialogue
    {
        public string CharacterName = "";
        public string Dialogue = "";
        public Action OnDialogueBegin;
        public Action OnDialogueEnd;
        public bool EndSequenceOnFinish = false;
        public CustomDialogue NextDialogue = null;
        public DialogueBehaviour.ShowBarsType ShowBars = DialogueBehaviour.ShowBarsType.OnStartOnly;
        public bool AnsweredYes = false;
        public CustomDialogue(string characterName, string dialogue, CustomDialogue nextDialogue = null)
        {
            CharacterName = characterName;
            Dialogue = dialogue;
            NextDialogue = nextDialogue;
        }
    }
}
