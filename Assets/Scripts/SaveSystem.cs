using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

// For WebGL builds, we will save data as an obfuscated JSON string in PlayerPrefs instead of using file I/O, which is not supported in WebGL. This way, we can still have a simple save/load system that works across all platforms without needing separate code paths for each platform.
// public static class SaveSystem
// {
//     // Keeping your original path for PC/Android builds
//     public static string path = Application.persistentDataPath + "/savefile.dat";
//     private static string webGLKey = "MyPuzzleGame_SaveData";

//     public static void SaveData(GameManager gameManager)
//     {
//         Data dataToSave = new Data(gameManager);
        
//         // Convert your data object to a JSON string
//         string json = JsonUtility.ToJson(dataToSave);

//         #if UNITY_WEBGL && !UNITY_EDITOR
//             // 1. Obfuscate it by converting it to Base64 (looks like gibberish)
//             string encryptedJson = EncryptDecrypt(json);
            
//             // 2. Save it to PlayerPrefs (Unity automatically hooks this to WebGL localStorage)
//             PlayerPrefs.SetString(webGLKey, encryptedJson);
//             PlayerPrefs.Save(); 
//         #else
//             // Your original working code for Windows/Android
//             System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
//             FileStream stream = new FileStream(path, FileMode.Create);
//             formatter.Serialize(stream, dataToSave);
//             stream.Close();
//         #endif
//     }

//     public static Data LoadData()
//     {
//         #if UNITY_WEBGL && !UNITY_EDITOR
//             if (PlayerPrefs.HasKey(webGLKey))
//             {
//                 string encryptedJson = PlayerPrefs.GetString(webGLKey);
                
//                 // Decode the gibberish back to clean JSON
//                 string json = EncryptDecrypt(encryptedJson);
                
//                 // Convert JSON back to your Data class
//                 return JsonUtility.FromJson<Data>(json);
//             }
//             return null;
//         #else
//             // Your original working code for Windows/Android
//             if (File.Exists(path))
//             {
//                 System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
//                 FileStream stream = new FileStream(path, FileMode.Open);
//                 Data data = formatter.Deserialize(stream) as Data;
//                 stream.Close();
//                 return data;
//             }
//             return null;
//         #endif
//     }

//     // A simple XOR cipher or Base64 conversion to stop casual editing/cheating
//     private static string EncryptDecrypt(string textToEncrypt)
//     {
//         // Simple Base64 Encoding/Decoding (Stops 95% of casual cheaters)
//         // If it's already Base64, decode it; otherwise, encode it.
//         try {
//             byte[] base64EncodedBytes = System.Convert.FromBase64String(textToEncrypt);
//             return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
//         }
//         catch {
//             byte[] textAsBytes = System.Text.Encoding.UTF8.GetBytes(textToEncrypt);
//             return System.Convert.ToBase64String(textAsBytes);
//         }
//     }
// }

// This class is responsible for saving and loading the game data. It uses binary serialization for PC/Android builds and PlayerPrefs.
public static class SaveSystem{
    // A property ensures Application.persistentDataPath is read at runtime when needed
    private static string GetPath() 
    {
        return Path.Combine(Application.persistentDataPath, "savefile.dat");
    }

    public static void SaveData(GameManager gameManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(GetPath(), FileMode.Create); // Updated here

        Data dataToSave = new Data(gameManager);

        formatter.Serialize(stream, dataToSave);
        stream.Close();
    }

    public static Data LoadData()
    {
        string currentPath = GetPath(); // Updated here
        if (File.Exists(currentPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(currentPath, FileMode.Open);

            Data data = formatter.Deserialize(stream) as Data;
            stream.Close();
            return data;
        }
        else
        {
            return null;
        }
    }
}
