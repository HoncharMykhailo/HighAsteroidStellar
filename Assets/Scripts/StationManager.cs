using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public static class SystemStorage
{
    public static NewSystemData selectedSystem;
}


public class StationManager : MonoBehaviour
{
    public bool transferComplete = false;

    public StationAnim stationAnim;  // Reference to the docking script
    public GameObject player;
    public InventoryManager inventoryManager;

    public TextMeshProUGUI goldText;
    public TextMeshProUGUI gemText;
    public TextMeshProUGUI hydroText;
    public TextMeshProUGUI platinumText;
    public GameObject victoryText;
    public AsteroidFieldGenerator asteroidFieldGenerator;
    public int[] resourses = new int[4];

    public List<NewSystemData> availableSystems;


    [Header("UI Elements")]
    public GameObject stationUI; // The UI panel to show systems
    public GameObject systemSelectionPanel;
    public Transform systemListContent; // Parent for system buttons
    public GameObject systemButtonTemplate; // Prefab for system buttons
   // public GameObject systemButtonTemplateContent;

   float yOffset = -100;
    float spacing = 60;

    void Start()
    {
        stationAnim = GetComponent<StationAnim>();
        inventoryManager = player.GetComponent<InventoryManager>();
        goldText = inventoryManager.goldText;
        gemText = inventoryManager.gemText;
        hydroText = inventoryManager.hydroText;
        platinumText = inventoryManager.platinumText;



        NewSystemData system = SystemStorage.selectedSystem;
        if (system != null)
        {
            Debug.Log("Game started with: " + system.systemName);
            asteroidFieldGenerator.GenerateAsteroidFieldWithSystem(system);
        }
        else
        {
            Debug.LogWarning("No system selected! Returning to menu.");
            SceneManager.LoadScene("MenuScene");
        }



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
    }

    // Update is called once per frame
    void Update()
    {
        if (stationAnim.isDocked)
        {
            if(!transferComplete)
            {
                TransferResources();
                transferComplete = true;
                inventoryManager.currentInventory = 0;

                bool victory = false;

                for(int i = 0;i<resourses.Length;i++)
                {
                    if(resourses[i]>50) {victory = true;Debug.Log(resourses[i]+" "+victory);}
                    else {victory = false; break;}
                }
                
                if(victory){Debug.Log("Victory");victoryText.SetActive(true);}


                
                Debug.Log("UI");
                

            }
        }
    }

    public void TransferResources()
    {
        for(int i = 0;i<resourses.Length;i++)
        {
            resourses[i]+=inventoryManager.resourses[i];
            inventoryManager.resourses[i] = 0;
            UpdateUI();
        }
    }

    public void ToggleSystemSelection()
    {
        bool isActive = systemSelectionPanel.activeSelf;
        systemSelectionPanel.SetActive(!isActive);

        if (!isActive)
        {
            PopulateSystemList();
        }
    }


    

    public void UpdateUI()
    {
        goldText.text = resourses[0].ToString();
        gemText.text = resourses[1].ToString();
        hydroText.text = resourses[2].ToString();
        platinumText.text = resourses[3].ToString();
    }


    public void ShowStationUI()
    {
        stationUI.SetActive(true);
        PopulateSystemList();
    }

    public void HideStationUI()
    {
        stationUI.SetActive(false);
        yOffset = -100;
    }

    void PopulateSystemList()
    {
        yOffset = -100;
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

            TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = system.systemName + $" (Ores: {system.oreType1}, {system.oreType2})";

            Button button = newButton.GetComponent<Button>();
            button.onClick.AddListener(() => SelectSystem(system));
        }

    }

    void SelectSystem(NewSystemData system)
    {
        asteroidFieldGenerator.GenerateAsteroidFieldWithSystem(system);
        Debug.Log($"Warping to: {system.systemName}");
        systemSelectionPanel.SetActive(false);
    }

}
