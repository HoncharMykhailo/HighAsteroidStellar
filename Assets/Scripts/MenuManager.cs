using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject systemSelectionPanel;
    public TextMeshProUGUI infoText;
    private bool isInfoVisible = false;

    public Transform systemListContent;
    public GameObject systemButtonTemplate; // Inactive template prefab
    public List<NewSystemData> availableSystems;

    void Start()
    {
        GenerateSystemList();
    }

     public void ToggleInfo()
    {
        isInfoVisible = !isInfoVisible;

        infoText.gameObject.SetActive(isInfoVisible);
        systemSelectionPanel.SetActive(!isInfoVisible);


        if (isInfoVisible)
        {
          //  infoText.text = "Created by Mykhailo \"MikeRoWave\" Honchar"; // будь-який текст
        }
    }

    void GenerateSystemList()
    {
        
        // Sample generation logic
        for(int i = 0;i<4;i++){
            NewSystemData newSystem = ScriptableObject.CreateInstance<NewSystemData>();
            newSystem.systemName = "System " + Random.Range(1000, 9999);
            newSystem.emptyAsteroidChance = Random.Range(0.5f, 0.9f);
            newSystem.oreType1 = Random.Range(1, 5); 
            newSystem.oreType2 = Random.Range(1, 5);

            newSystem.oreType1Chance = Random.Range(0.05f, 0.3f);
            newSystem.oreType2Chance = Random.Range(0.05f, 0.3f);

            availableSystems.Add(newSystem);
        }


        int yOffset = -100;
        int spacing = 60;

        foreach (var system in availableSystems)
        {
            GameObject newButton = Instantiate(systemButtonTemplate, systemListContent);
            newButton.SetActive(true);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text =
                $"{system.systemName} (Ores: {system.oreType1}, {system.oreType2})";

            Button btn = newButton.GetComponent<Button>();

            RectTransform rt = newButton.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(0, -yOffset);  // Stack downward
            yOffset += spacing;

            btn.onClick.AddListener(() => LoadGame(system));
        }
    }

    void LoadGame(NewSystemData selectedSystem)
    {
        // Store selected system for use in the game scene
        SystemStorage.selectedSystem = selectedSystem;

        // Load game scene
        SceneManager.LoadScene("SampleScene");
    }



    void PopulateSystemList()
    {
        int yOffset = -100;
        int spacing = 60;
        foreach (Transform child in systemListContent)
        {
            Destroy(child.gameObject);
        }
        foreach (NewSystemData system in availableSystems)
        {
            GameObject newButton = Instantiate(systemButtonTemplate, systemListContent);
            newButton.SetActive(true);

            RectTransform rt = newButton.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(0, -yOffset);  // Stack downward
            yOffset += spacing;
        }

    }


}
