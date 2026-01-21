using UnityEngine;
using Newtonsoft.Json; // Use the powerful Newtonsoft library
using System.Collections.Generic;

public class AnatomyFactManager : MonoBehaviour
{
    // A public static instance to make it easily accessible from other scripts (Singleton pattern)
    public static AnatomyFactManager Instance { get; private set; }

    // Dictionaries to hold the loaded data
    private Dictionary<string, string> boneFacts;
    private Dictionary<string, string> muscleFacts;

    void Awake()
    {
        // --- Singleton Setup ---
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicates
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Keep this manager alive between scenes
        // -------------------------

        LoadAllFacts(); // Load the data when the game starts
    }

    private void LoadAllFacts()
    {
        // --- Load Anatomical/Bone Facts ---
        TextAsset anatomyFile = Resources.Load<TextAsset>("boneFacts");
        if (anatomyFile != null)
        {
            var loadedData = JsonConvert.DeserializeObject<boneFactCollection>(anatomyFile.text);
            boneFacts = loadedData.bone_facts;
            Debug.Log($"Successfully loaded {boneFacts.Count} anatomical facts.");
        }
        else
        {
            Debug.LogError("Failed to load anatomical_facts.txt from Resources folder!");
        }

        // --- Load Muscle Facts ---
        TextAsset muscleFile = Resources.Load<TextAsset>("muscleFacts");
        if (muscleFile != null)
        {
            var loadedData = JsonConvert.DeserializeObject<muscleFactCollection>(muscleFile.text);
            muscleFacts = loadedData.muscle_facts;
            Debug.Log($"Successfully loaded {muscleFacts.Count} muscle facts.");
        }
        else
        {
            Debug.LogError("Failed to load muscle_facts.txt from Resources folder!");
        }
    }

    /// <summary>
    /// Retrieves the fact for a given bone or anatomical part.
    /// </summary>
    /// <param name="name">The exact name of the part (e.g., "Left_femur").</param>
    /// <returns>The fact as a string, or an error message if not found.</returns>
    public string GetAnatomyFact(string name)
    {
        string fact;
        if (boneFacts != null && boneFacts.TryGetValue(name, out fact))
        {
            return fact;
        }
        else if (muscleFacts != null && muscleFacts.TryGetValue(name, out fact))
        {
            return fact;
        }
        return $"Fact for '{name}' not found.";
    }
}