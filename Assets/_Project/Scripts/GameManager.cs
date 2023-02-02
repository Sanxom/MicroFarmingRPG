using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public event UnityAction OnNewDay;

    public CropData selectedCropToPlant;
    public int currentDay;
    
    [SerializeField] private TextMeshProUGUI statsText;
    [SerializeField] private int money;
    [SerializeField] private int cropInventory;

    private void Awake()
    {
        if(instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        UpdateStatsText();
    }

    private void OnEnable()
    {
        Crop.OnPlantCrop += OnPlantCrop;
        Crop.OnHarvestCrop += OnHarvestCrop;
    }

    private void OnDisable()
    {
        Crop.OnPlantCrop -= OnPlantCrop;
        Crop.OnHarvestCrop -= OnHarvestCrop;
    }

    public void SetNextDay()
    {
        currentDay++;
        OnNewDay?.Invoke();
        UpdateStatsText();
    }

    public void OnPlantCrop(CropData crop)
    {
        cropInventory--;
        UpdateStatsText();
    }

    public void OnHarvestCrop(CropData crop)
    {
        money += crop.sellPrice;
        UpdateStatsText();
    }

    public void PurchaseCrop(CropData crop)
    {
        money -= crop.purchasePrice;
        cropInventory++;
        UpdateStatsText();
    }

    public void OnBuyCropButton(CropData crop)
    {
        if(money >= crop.purchasePrice)
            PurchaseCrop(crop);
    }

    public bool CanPlantCrop()
    {
        return cropInventory > 0;
    }

    private void UpdateStatsText()
    {
        statsText.text = $"Day: {currentDay}\nMoney: ${money}\nCrop Inventory: {cropInventory}";
    }
}