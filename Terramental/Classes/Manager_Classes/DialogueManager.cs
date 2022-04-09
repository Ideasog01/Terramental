using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Terramental
{
    public class DialogueManager
    {
        public static bool dialogueActive;

        private Dialogue _currentDialogue;
        private int _dialogueIndex;

        private string _dialogueText;
        private string _dialogueNameText;

        private SpriteFont _dialogueFont;

        private InterfaceComponent _dialoguePanel;
        private Button _dialogueNextButton;

        private GameManager _gameManager;

        public DialogueManager(GameManager gameManager, MenuManager menuManager)
        {
            _dialoguePanel = new InterfaceComponent(gameManager.playerCharacter, new Vector2(-180, 100), new Vector2(400, 200), gameManager.GetTexture("UserInterface/Dialogue/DialoguePanel"));
            _dialogueNextButton = new Button(GameManager.ButtonName.DialogueNextButton, menuManager);
            _dialogueNextButton.InitialiseMenuComponent(gameManager.GetTexture("UserInterface/Dialogue/DialogueNextButton"), new Vector2(gameManager.playerCharacter.SpritePosition.X, gameManager.playerCharacter.SpritePosition.Y), new Vector2(50, 50));

            _dialogueFont = gameManager.Content.Load<SpriteFont>("SpriteFont/CharacterName");

            _gameManager = gameManager;

            _dialogueNameText = "Bob";
            _dialogueText = "Hello, my name is Bob. \nHow do you do?";
        
        }

        public void UpdatePosition()
        {
            _dialoguePanel.FollowCamera();
            _dialogueNextButton.ComponentPosition = _dialoguePanel.ComponentPosition;
            _dialogueNextButton.ComponentRectangle = new Rectangle((int)_dialogueNextButton.ComponentPosition.X + 340, (int)_dialogueNextButton.ComponentPosition.Y + 140, (int)_dialogueNextButton.ComponentScale.X, (int)_dialogueNextButton.ComponentScale.Y);
        }

        public void DrawDialogueInterface(SpriteBatch spriteBatch)
        {
            _dialoguePanel.DrawComponent(spriteBatch);
            _dialogueNextButton.DrawMenuComponent(spriteBatch);

            spriteBatch.DrawString(_dialogueFont, _dialogueNameText, _dialoguePanel.ComponentPosition + new Vector2(10, 10), Color.Black);
            spriteBatch.DrawString(_dialogueFont, _dialogueText, _dialoguePanel.ComponentPosition + new Vector2(10, 40), Color.Black);

        }

        public void CheckButtonInteraction(Vector2 mousePos)
        {
            if(dialogueActive)
            {
                if(_dialogueNextButton.ComponentRectangle.Contains(mousePos))
                {
                    NextDialogue();
                }
            }
        }

        public void ActivateDialogue(Dialogue dialogue)
        {
            dialogueActive = true;
            _currentDialogue = dialogue;
            _dialogueIndex = -1;
            PlayerCharacter.disableMovement = true;

            _gameManager.IsMouseVisible = true;
            _dialogueNameText = _currentDialogue.DialogueName;

            NextDialogue();
        }

        public void NextDialogue()
        {
            _dialogueIndex++;

            if(_dialogueIndex >= _currentDialogue.DialogueContent.Length)
            {
                EndDialogue();
            }
            else
            {
                _dialogueText = _currentDialogue.DialogueContent[_dialogueIndex];
            }
        }

        public void EndDialogue()
        {
            dialogueActive = false;
            _currentDialogue = null;
            _gameManager.IsMouseVisible = false;
            PlayerCharacter.disableMovement = false;
        }

    }
}
