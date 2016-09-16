/******************************************************************************/
/*!
@file   ObjectTransformDispatcher.cs
@author Christian Sagel
@par    email: ckpsm@live.com
*/
/******************************************************************************/
using UnityEngine;
using Stratus;
using System;

namespace Stratus
{
  /**************************************************************************/
  /*!
  @class ObjectTransformDispatcher 
  */
  /**************************************************************************/
  public class ObjectTransformDispatcher : EventDispatcher
  { 
    public enum TransformType { Translate, Rotate, Scale }
    public TransformType Type;
    public float Duration = 1.0f;
    public Ease Ease;
    public Vector3 Value;
    Vector3 PreviousValue;

    /**************************************************************************/
    /*!
    @brief  Initializes the ObjectTransformDispatcher.
    */
    /**************************************************************************/
    protected override void OnInitialize()
    {
      
    }

    protected override void OnTrigger()
    {
      this.Transform(this.Value);
    }

    /// <summary>
    /// Interpolates to the specified transformation.
    /// </summary>
    public void Transform(Vector3 value)
    {
      var seq = Actions.Sequence(this);
      switch (Type)
      {
        case TransformType.Translate:
          PreviousValue = this.transform.localPosition;
          Actions.Property(seq, () => this.transform.localPosition, value, this.Duration, this.Ease);
          break;
        case TransformType.Rotate:
          PreviousValue = this.transform.rotation.eulerAngles;
          Actions.Property(seq, () => this.transform.rotation.eulerAngles, value, this.Duration, this.Ease);
          break;
        case TransformType.Scale:
          PreviousValue = this.transform.localScale;
          Actions.Property(seq, () => this.transform.localScale, value, this.Duration, this.Ease);
          break;
      }
    }

    /// <summary>
    /// Reverts to the previous transformation.
    /// </summary>
    public void Revert()
    {
      this.Transform(this.PreviousValue);
    }



  }

}