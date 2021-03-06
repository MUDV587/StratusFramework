﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stratus
{
  namespace Examples
  {
    /// <summary>
    /// A simple component showcasing the most useful types provided by the Stratus library
    /// </summary>
    public class TypesExample : StratusBehaviour
    {
      [Header("Stratus Field Types")]
      public InputBinding inputField = new InputBinding();
      public SceneField scene = new SceneField();
      public TagField tagField = new TagField();
      public FloatRange floatRange = new FloatRange();
      public IntegerRange intRange = new IntegerRange();
      public VariableAttribute variable = new VariableAttribute();
      public KeyCode enumDrawer;

      [InvokeMethodButton(typeof(TypesExample), "TryLayer")]
      public LayerField layer = new LayerField(); 

      void TryReadingInput()
      {
        var value = inputField.value;
        if (inputField.isPositive) {}
        if (inputField.isNegative) {}
        if (inputField.isNeutral) {}
        StratusDebug.Log(value);        
      }

      void TryLoadingScene()
      {
        scene.Load(UnityEngine.SceneManagement.LoadSceneMode.Single);
      }

      void TryTag()
      {        
        if (gameObject.CompareTag(tagField))
          StratusDebug.Log("The GameObject's tag and selected tag field match! (" + tagField + ")");
      }

      void TryLayer()
      {
        if (layer.Matches(this.gameObject))
          StratusDebug.Log("The GameObject's layer and selected layer field are a match! (" + layer + ")");
      }     
      

    } 
  }

}