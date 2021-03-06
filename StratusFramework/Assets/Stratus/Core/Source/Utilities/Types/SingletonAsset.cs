﻿using System;
using UnityEngine;
using System.Collections.Generic;
using Stratus.Utilities;

namespace Stratus
{
  /// <summary>
  /// An asset of which there is only a single instance of in the project. Mainly used
  /// for global configuration data.
  /// </summary>
  public abstract class SingletonAsset : ScriptableObject
  {
    /// <summary>
    /// A cached dictionary of all present/loaded singletons!
    /// </summary>
    public static Dictionary<Type, ScriptableObject> singletons { get; private set; } = new Dictionary<Type, ScriptableObject>();

    /// <summary>
    /// A required attribute that specifies the wanted folder path and name for a singleton asset
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class SingletonAssetAttribute : Attribute
    {
      string path { get; set; }
      string name { get; set; }
      public bool hidden { get; set; }

      /// <param name="relativePath">The relative path to the folder where you want the asset to be stored</param>
      /// <param name="name">The name of the asset</param>
      public SingletonAssetAttribute(string relativePath, string name)
      {
        this.path = relativePath;
        this.name = name;
      }

      public class MissingException : Exception
      {
        public MissingException(string className) : base("The class declaration for " + className + " is missing the [SingletonAsset] attribute, which provides the path information needed in order to construct the asset.")
        {
          // Fill later?
          this.HelpLink = "http://msdn.microsoft.com";
          this.Source = "Exception_Class_Samples";
        }
      }
    }
      }

  /// <summary>
  /// An asset of which there is only a single instance of in the project. Mainly used
  /// for global configuration data.
  /// </summary>
  public abstract class SingletonAsset<T> : SingletonAsset where T : ScriptableObject
  {
    /// <summary>
    /// The singular instance to the asset, after it has been loaded from memory
    /// </summary>
    private static T _instance { get; set; }

    /// <summary>
    /// Whether this singleton has been instantiated
    /// </summary>
    public static bool instantiated => _instance != null;

    /// <summary>
    /// Access the data of this object.
    /// </summary>
    public static T instance
    {
      get
      {
        if (_instance == null)
          _instance = LoadOrCreate();

        return _instance;
      }
    }

    //public static SingletonAsset<T> singleton
    //{ 
    //  get
    //  {
    //    return (SingletonAsset<T>)instance;
    //  }
    //}

#if UNITY_EDITOR
    /// <summary>
    /// Used for editing the properties of the asset in a generic way
    /// </summary>
    public static UnityEditor.SerializedObject serializedObject
    {
      get
      {
        if (serializedObject_ == null)
          LoadOrCreate();
        return serializedObject_;
      }
    }


    // Fields
    private static UnityEditor.SerializedObject serializedObject_;
#endif

    /// <summary>
    /// Creates an instance of the asset
    /// </summary>
    /// <returns></returns>
    protected static T LoadOrCreate()
    {
      var type = typeof(T);
      var settings = AttributeUtility.FindAttribute<SingletonAssetAttribute>(type);
      if (settings == null)
        throw new SingletonAssetAttribute.MissingException(type.Name);

      var path = settings.GetProperty<string>("path");
      var name = settings.GetProperty<string>("name");
      var hidden = settings.GetProperty<bool>("hidden");

      var folderPath = IO.GetFolderPath(path);
      if (folderPath == null)
        throw new NullReferenceException("The given folder path '" + path + "' to be used for the asset '" + name + "' could not be found!");

      var fullPath = folderPath + "/" + name + ".asset";

      // Now create the proper instance
      _instance = Assets.LoadOrCreateScriptableObject<T>(fullPath);


#if UNITY_EDITOR
      // Also create the serialized object      
      serializedObject_ = new UnityEditor.SerializedObject(_instance);
#endif

      if (hidden)
        _instance.hideFlags = HideFlags.HideInHierarchy;

      return _instance;
    }

    /// <summary>
    /// Saves this asset
    /// </summary>
    public static void Save()
    {
#if UNITY_EDITOR
      UnityEditor.EditorUtility.SetDirty(_instance);
      UnityEditor.AssetDatabase.SaveAssets();
#endif
    }

  }
}
