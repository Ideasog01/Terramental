using Microsoft.Xna.Framework;

namespace Terramental
{
    class DialogueController
    {
        private Dialogue _dialogue;
        private PlayerCharacter _playerCharacter;
        private DialogueManager _dialogueManager;
        private Rectangle _rectangleTrigger;
        private bool _dialogueTriggered;

        public DialogueController(PlayerCharacter playerCharacter, Rectangle rectangle, DialogueManager dialogueManager, Dialogue dialogue)
        {
            _playerCharacter = playerCharacter;
            _rectangleTrigger = rectangle;
            _dialogueManager = dialogueManager;

            _dialogue = dialogue;
        }

        public void CheckDialogueCollision()
        {
            if(!_dialogueTriggered)
            {
                if (_playerCharacter.OnCollision(_rectangleTrigger))
                {
                    _dialogueManager.ActivateDialogue(_dialogue);
                    _dialogueTriggered = true;
                }
            }
        }
    }
}
