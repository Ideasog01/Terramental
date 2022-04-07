﻿using Microsoft.Xna.Framework;

namespace Terramental
{
    class DialogueController
    {
        private Dialogue _dialogue;
        private PlayerCharacter _playerCharacter;
        private DialogueManager _dialogueManager;
        private Rectangle _rectangleTrigger;

        public DialogueController(PlayerCharacter playerCharacter, Rectangle rectangle, DialogueManager dialogueManager)
        {
            _playerCharacter = playerCharacter;
            _rectangleTrigger = rectangle;
            _dialogueManager = dialogueManager;
        }

        public void CheckDialogueCollision()
        {
            if(_playerCharacter.OnCollision(_rectangleTrigger))
            {
                _dialogueManager.ActivateDialogue(_dialogue);
            }
        }
    }
}
