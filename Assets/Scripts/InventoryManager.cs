using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int maxInventory = 30;
    public int currentInventory = 0;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI gemText;
    public TextMeshProUGUI hydroText;
    public TextMeshProUGUI platinumText;
    public int[] resourses = new int[4];

    

    public void UpdateUI()
    {
        goldText.text = resourses[0].ToString();
        gemText.text = resourses[1].ToString();
        hydroText.text = resourses[2].ToString();
        platinumText.text = resourses[3].ToString();
    }
}
