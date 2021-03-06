using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using Stratus.Modules.InkModule;

namespace Stratus
{
  namespace Examples
  {
    /// <summary>
    /// A simple ink story display that displays one line at a time
    /// </summary>
    public class SampleStoryDisplay : StoryDisplay
    {
      //------------------------------------------------------------------------------------------/
      // Fields
      //------------------------------------------------------------------------------------------/
      [Header("Dialog")]
      public Text speakerText;
      public Text messageText;

      [Header("Choices")]
      [SerializeField]
      public Button choicePrefab;
      public CanvasRenderer dialogPanel;
      public VerticalLayoutGroup choicesPanel;      

      //------------------------------------------------------------------------------------------/
      // Properties
      //------------------------------------------------------------------------------------------/
      public bool displayChoices
      {
        set
        {
          choicesPanel.gameObject.SetActive(value);
          dialogPanel.gameObject.SetActive(!value);
        }
      }

      public bool display
      {
        set
        {
          dialogPanel.gameObject.SetActive(value);
          choicesPanel.gameObject.SetActive(value);
        }
      }


      //------------------------------------------------------------------------------------------/
      // Methods
      //------------------------------------------------------------------------------------------/
      protected override void OnStart()
      {
        display = false;
      }

      protected override void OnStoryStarted()
      {
        display = true;
        displayChoices = false;
      }

      protected override void OnStoryEnded()
      {
        OnChoiceSelected();
        display = false;
      }

      protected override void OnStoryUpdate(ParsedLine line, bool visited)
      {       
        if (!line.isParsed)
        {
          speakerText.text = "";
          messageText.text = line.line;
        }
        else
        {
          Parse speaker = line.Find("Speaker");
          if (speaker != null)
            speakerText.text = speaker.value;

          Parse message = line.Find("Message");
          if (message != null)
            messageText.text = message.value;          
        }
      }
      
      protected override void OnPresentChoices(List<Choice> choices)
      {
        displayChoices = true;
        AddChoices(choices, choicePrefab, choicesPanel);
      }

      protected override void OnChoiceSelected()
      {
        displayChoices = false;
        RemoveChoices(choicesPanel);
      }
      


    }

  }
}