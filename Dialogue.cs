using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public DialogueStep[] dialogueSteps;

    [System.Serializable]
    public struct DialogueStep
    {
        public string nameTxt;
        public Sprite speakerName;
        public Sprite frame;
        public Sprite face;

        [TextArea(3, 20)]
        public string sentence;
    }
}
