using System;
using System.Collections.Generic;
using System.Text;

namespace Terramental
{
    public class Dialogue
    {
        private string[] _dialogueContent;

        private string[] _dialogueName;

        public Dialogue(string[] content, string[] names)
        {
            _dialogueContent = content;
            _dialogueName = names;
        }

        public string[] DialogueContent
        {
            get { return _dialogueContent; }
        }

        public string[] DialogueName
        {
            get { return _dialogueName; }
        }
    }
}
