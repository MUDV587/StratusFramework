using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stratus.Interfaces;

namespace Stratus.Gameplay
{
  [ExecuteInEditMode]
  [DisallowMultipleComponent]
  public class StratusTriggerSystem : StratusBehaviour, Validator, ValidatorAggregator
  {
    //------------------------------------------------------------------------/
    // Declarations
    //------------------------------------------------------------------------/
    public enum ConnectionDisplay
    {
      Selection,
      Grouping
    }

    public enum ConnectionStatus
    {
      Connected,
      Disconnected,
      Selected,
      Disjoint
    }

    //------------------------------------------------------------------------/
    // Fields
    //------------------------------------------------------------------------/
    public bool showDescriptions = true;
    public ConnectionDisplay connectionDisplay = ConnectionDisplay.Selection;
    public bool outlines = false;

    public List<Trigger> triggers = new List<Trigger>();
    public List<StratusTriggerable> triggerables = new List<StratusTriggerable>();
    public bool descriptionsWithLabel = false;
    private Dictionary<Trigger, bool> triggersInitialState = new Dictionary<Trigger, bool>();
    private Dictionary<StratusTriggerable, bool> triggerablesInitialState = new Dictionary<StratusTriggerable, bool>();

    //------------------------------------------------------------------------/
    // Properties
    //------------------------------------------------------------------------/
    /// <summary>
    /// Whether there no components in the system
    /// </summary>    
    public bool isEmpty => triggers.Empty() && triggerables.Empty();

    //------------------------------------------------------------------------/
    // Messages
    //------------------------------------------------------------------------/
    private void Awake()
    {
      RecordTriggerStates();
    }

    private void OnDestroy()
    {
      ShowComponents(true);
    }

    private void OnEnable()
    {
      Refresh();
    }

    private void Reset()
    {
      Refresh();
    }

    private void OnValidate()
    {
      //ToggleComponents(enabled);
      triggers.TrimNull();
      triggerables.TrimNull();
      ShowComponents(false);
    }

    ObjectValidation[] ValidatorAggregator.Validate()
    {
      var messages = new List<ObjectValidation>();
      messages.AddIfNotNull(ObjectValidation.Generate(this));
      messages.AddRange(ObjectValidation.Aggregate(triggers));
      messages.AddRange(ObjectValidation.Aggregate(triggerables));      
      return messages.ToArray();
    }

    ObjectValidation Validator.Validate()
    {
      return ValidateConnections();
    }

    //------------------------------------------------------------------------/
    // Methods
    //------------------------------------------------------------------------/
    private void RecordTriggerStates()
    {
      foreach (var trigger in triggers)
        triggersInitialState.Add(trigger, trigger.enabled);

      foreach (var triggerable in triggerables)
        triggerablesInitialState.Add(triggerable, triggerable.enabled);
    }

    /// <summary>
    /// Restars the state of all the triggers in this system to their intitial values
    /// </summary>
    public void Restart()
    {
      foreach (var trigger in triggers)
        trigger.Restart();

      foreach (var triggerable in triggerables)
        triggerable.Restart();
    }

    /// <summary>
    /// Toggles all eligible triggers in the system on/off
    /// </summary>
    public void ToggleTriggers(bool toggle)
    {
      foreach (var trigger in triggers)
      {
        // Skip triggers that were marked as not persistent and have been activated
        if (!trigger.persistent && trigger.activated)
          continue;

        trigger.enabled = toggle;
      }
    }
    
    /// <summary>
    /// Toggles all components in the system on/off
    /// </summary>
    /// <param name="toggle"></param>
    public void ToggleComponents(bool toggle)
    {
      foreach (var trigger in triggers)
      {
        if (!trigger.awoke)
          continue;

        trigger.enabled = toggle;
      }

      foreach (var triggerable in triggerables)
      {
        if (!triggerable.awoke)
          continue;

        triggerable.enabled = toggle;
      }
    }


    /// <summary>
    /// Adds a trigger to the system
    /// </summary>
    /// <param name="baseTrigger"></param>
    public void Add(StratusTriggerBase baseTrigger)
    {
      if (baseTrigger is Trigger)
        triggers.Add(baseTrigger as Trigger);
      else if (baseTrigger is StratusTriggerable)
        triggerables.Add(baseTrigger as StratusTriggerable);
    }

    /// <summary>
    /// Controls visibility for all the base trigger components
    /// </summary>
    /// <param name="show"></param>
    public void ShowComponents(bool show)
    {
      HideFlags flag = show ? HideFlags.None : HideFlags.HideInInspector;
      foreach (var trigger in triggers)
        trigger.hideFlags = flag;
      foreach (var triggerable in triggerables)
        triggerable.hideFlags = flag;
    }
    
    /// <summary>
    /// Refreshes the state of this TriggerSystem
    /// </summary>
    private void Refresh()
    {
      // Remove any invalid
      triggers.TrimNull();
      triggerables.TrimNull();

      // Add previously not found
      triggers.AddRangeUnique(GetComponents<Trigger>());
      triggerables.AddRangeUnique(GetComponents<StratusTriggerable>());

      // Hide any triggers managed by the system
      ShowComponents(false);

      // Validate triggers
      ValidateTriggers();
    }
    
    private void ValidateTriggers()
    {
      foreach (var trigger in triggers)
      {
        trigger.scope = Trigger.Scope.Component;
      }        
    }

    public static bool IsConnected(Trigger trigger, StratusTriggerable triggerable)
    {
      if (trigger.targets.Contains(triggerable))
        return true;
      return false;
    }

    public static bool IsConnected(Trigger trigger)
    {
      return trigger.targets.NotEmpty();
    }

    public ObjectValidation ValidateConnections()
    {
      List<StratusTriggerBase> disconnected = new List<StratusTriggerBase>();
      foreach (var t in triggers)
      {
        if (!IsConnected(t))
          disconnected.Add(t);
      }

      //foreach (var t in triggerables)
      //{
      //  if (!IsConnected(t))
      //    disconnected.Add(t);
      //}

      if (disconnected.Empty())
        return null;

      string msg = $"Triggers marked as disconnected ({disconnected.Count}):";
      foreach (var t in disconnected)
        msg += $"\n- {t.GetType().Name} : <i>{t.description}</i>";
      return new ObjectValidation(msg, ObjectValidation.Level.Warning, this);
    }   

  }

}