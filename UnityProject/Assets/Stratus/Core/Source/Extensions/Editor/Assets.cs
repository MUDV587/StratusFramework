using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Stratus
{
  namespace Utilities
  {
    public static partial class Assets
    {

      //public static string GetAssetPath()
      //{
      //
      //}

      /// <summary>
      /// Returns a string of the folder's path that this script is on
      /// </summary>
      /// <param name="obj"></param>
      /// <returns></returns>
      public static string GetFolder(ScriptableObject obj)
      {
        var ms = MonoScript.FromScriptableObject(obj);
        var path = AssetDatabase.GetAssetPath(ms);
        var fi = new FileInfo(path);

        var folder = fi.Directory.ToString();
        folder = folder.Replace('\\', '/');
        return MakeRelative(folder);
      }

      /// <summary>
      /// Returns a relative path of an asset
      /// </summary>
      /// <param name="path"></param>
      /// <returns></returns>
      public static string MakeRelative(string path)
      {
        //if (path.StartsWith(Application.dataPath))
        //{
        var relativePath = "Assets" + path.Substring(Application.dataPath.Length);
        relativePath = relativePath.Replace("\\", "/");
        return relativePath;
        //}
        //return path;
      }

      /// <summary>
      /// Returns the relative path of a given folder name (if found within the application's assets folder)
      /// </summary>
      /// <param name="folderName"></param>
      /// <returns></returns>
      public static string GetFolderPath(string folderName)
      {
        //var dirInfo = new DirectoryInfo(Application.dataPath);
        var dirs = GetDirectories(Application.dataPath);
        var folderPath = dirs.Find(x => x.Contains(folderName));
        return folderPath;
      }

      static List<string> GetDirectories(string path)
      {
        List<string> dirs = new List<string>();
        ProcessDirectory(path, dirs);
        return dirs;
      }

      static void ProcessDirectory(string dirPath, List<string> dirs)
      {
        // Add this directory
        dirs.Add(MakeRelative(dirPath));
        // Now look for any subdirectories
        var subDirectoryEntries = Directory.GetDirectories(dirPath);
        foreach(var dir in subDirectoryEntries)
        {
          ProcessDirectory(dir, dirs);
        }          
      }

      /// <summary>
      /// Creates the asset and any directories that are missing along its path.
      /// </summary>
      /// <param name="unityObject">UnityObject to create an asset for.</param>
      /// <param name="unityFilePath">Unity file path (e.g. "Assets/Resources/MyFile.asset".</param>
      public static void CreateAssetAndDirectories(UnityEngine.Object unityObject, string unityFilePath)
      {
        var dir = Path.GetDirectoryName(unityFilePath);
        var pathDirectory = dir + UnityDirectorySeparator;

        bool hasFolder = AssetDatabase.IsValidFolder(dir);
        if (!hasFolder)
          CreateDirectoriesInPath(pathDirectory);

        AssetDatabase.CreateAsset(unityObject, unityFilePath);
      }


      private static void CreateDirectoriesInPath(string unityDirectoryPath)
      {
        // Check that last character is a directory separator
        if (unityDirectoryPath[unityDirectoryPath.Length - 1] != UnityDirectorySeparator)
        {
          var warningMessage = string.Format(
                                   "Path supplied to CreateDirectoriesInPath that does not include a DirectorySeparator " +
                                   "as the last character." +
                                   "\nSupplied Path: {0}, Filename: {1}",
                                   unityDirectoryPath);
          Debug.LogWarning(warningMessage);
        }

        // Warn and strip filenames
        var filename = Path.GetFileName(unityDirectoryPath);
        if (!string.IsNullOrEmpty(filename))
        {
          var warningMessage = string.Format(
                                   "Path supplied to CreateDirectoriesInPath that appears to include a filename. It will be " +
                                   "stripped. A path that ends with a DirectorySeparate should be supplied. " +
                                   "\nSupplied Path: {0}, Filename: {1}",
                                   unityDirectoryPath,
                                   filename);
          Debug.LogWarning(warningMessage);

          unityDirectoryPath = unityDirectoryPath.Replace(filename, string.Empty);
        }

        var folders = unityDirectoryPath.Split(UnityDirectorySeparator);

        // Error if path does NOT start from Assets
        if (folders.Length > 0 && folders[0] != "Assets")
        {
          var exceptionMessage = "AssetDatabaseUtility CreateDirectoriesInPath expects full Unity path, including 'Assets\\\". " +
                                 "Adding Assets to path.";
          throw new UnityException(exceptionMessage);
        }

        string pathToFolder = string.Empty;
        foreach (var folder in folders)
        {
          // Don't check for or create empty folders
          if (string.IsNullOrEmpty(folder))
          {
            continue;
          }

          // Create folders that don't exist
          pathToFolder = string.Concat(pathToFolder, folder);
          if (!UnityEditor.AssetDatabase.IsValidFolder(pathToFolder))
          {
            var pathToParent = System.IO.Directory.GetParent(pathToFolder).ToString();
            AssetDatabase.CreateFolder(pathToParent, folder);
            AssetDatabase.Refresh();
          }

          pathToFolder = string.Concat(pathToFolder, UnityDirectorySeparator);
        }
      }

      /// <summary>
      /// Gets the asset of type T at the given path
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="path"></param>
      /// <returns></returns>
      public static T[] GetAtPath<T>(string path)
      {

        ArrayList al = new ArrayList();
        string[] fileEntries = Directory.GetFiles(Application.dataPath + "/" + path);

        foreach (string fileName in fileEntries)
        {
          int assetPathIndex = fileName.IndexOf("Assets");
          string localPath = fileName.Substring(assetPathIndex);

          Object t = AssetDatabase.LoadAssetAtPath(localPath, typeof(T));

          if (t != null)
            al.Add(t);
        }
        T[] result = new T[al.Count];
        for (int i = 0; i < al.Count; i++)
          result[i] = (T)al[i];

        return result;
      }



    }
  }
}