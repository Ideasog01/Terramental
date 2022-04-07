using System;
using System.Collections.Generic;
using System.Text;

namespace Terramental
{
    public class DialogueManager
    {
        public static bool dialogueActive;

        private Dialogue _currentDialogue;
        private int _dialogueIndex;

        private string dialogueText;
        private string dialogueNameText;

        public void ActivateDialogue(Dialogue dialogue)
        {
            dialogueActive = true;
            _currentDialogue = dialogue;
            _dialogueIndex = -1;

            NextDialogue();
        }

        public void NextDialogue()
        {
            _dialogueIndex++;

            if(_dialogueIndex > _currentDialogue.DialogueContent.Length)
            {
                EndDialogue();
            }
            else
            {
                dialogueText = _currentDialogue.DialogueContent[_dialogueIndex];
                dialogueNameText = _currentDialogue.DialogueName[_dialogueIndex];
            }
        }

        public void EndDialogue()
        {
            dialogueActive = false;
            _currentDialogue = null;
        }

    }
}
