using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Stratus
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof(StratusBehaviour), true)]
  public class StratusBehaviourEditor : BehaviourEditor<StratusBehaviour>
  {
    protected override void OnBaseEditorEnable()
    {      
    }
  }

}