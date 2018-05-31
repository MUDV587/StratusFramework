﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stratus
{
  /// <summary>
  /// A behaviour whose main messages are handled by an external manager for performance reasons.
  /// </summary>
  public abstract class ManagedBehaviour : StratusBehaviour
  {
    protected internal virtual void OnAwake() { }
    protected internal virtual void OnStart() { }
    protected internal virtual void OnUpdate() { }
    protected internal virtual void OnFixedUpdate() { }
    protected internal virtual void OnLateUpdate() { }

    /// <summary>
    /// Instantiates this behaviour at runtime, adding it to the managed behaviour system
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T Instantiate<T>() where T : ManagedBehaviour
    {
      T behaviour = Instantiate<T>();
      ManagedBehaviourSystem.Add(behaviour);
      return behaviour;
    }

    /// <summary>
    /// Destroys this behaviour, removing it from the managed behaviour system
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="behaviour"></param>
    public static void Destroy<T>(T behaviour) where T : ManagedBehaviour
    {
      ManagedBehaviourSystem.Remove(behaviour);
      Destroy(behaviour);
    }

  }

}