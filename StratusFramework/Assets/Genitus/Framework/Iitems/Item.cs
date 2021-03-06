using UnityEngine;
using Stratus;
using System;
using System.Collections.Generic;

namespace Genitus
{
  /// <summary>
  /// Base class for items.
  /// </summary>
  public abstract class Item : ScriptableObject
  {
    public enum Category { Consumable, Weapon, Armor, Accessory, Quest, Ingredient }
    public string Name;
    public string Description;
    public int Value;
    [Tooltip("Items that are unique cannot be duplicated")]
    public bool Unique;
    public Sprite Icon;
    public GameObject Model;
    public abstract Category type { get; }

    /// <summary>
    /// Provides a short description of this item.
    /// </summary>
    /// <returns></returns>
    public abstract string Describe();


  }

}