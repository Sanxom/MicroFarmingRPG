using UnityEngine;
using UnityEngine.Events;

public class Crop : MonoBehaviour
{
    public static event UnityAction<CropData> OnPlantCrop;
    public static event UnityAction<CropData> OnHarvestCrop;

    [SerializeField] private SpriteRenderer _spriteRenderer;

    private CropData _currentCrop;
    private int _dayPlanted;
    private int _daysSinceLastWatered;

    public void Plant(CropData crop)
    {
        _currentCrop = crop;
        _dayPlanted = GameManager.instance.currentDay;
        _daysSinceLastWatered = 1;
        UpdateCropSprite();

        OnPlantCrop?.Invoke(crop);
    }

    public void NewDayCheck()
    {
        _daysSinceLastWatered++;

        if (_daysSinceLastWatered > 3 && !CanHarvest())
        {
            Destroy(gameObject);
        }

        UpdateCropSprite();
    }

    public void Water()
    {
        _daysSinceLastWatered = 0;
    }

    public void Harvest()
    {
        if (CanHarvest())
        {
            OnHarvestCrop?.Invoke(_currentCrop);
            Destroy(gameObject);
        }
    }

    public bool CanHarvest()
    {
        return CropProgress() >= _currentCrop.daysToGrow;
    }

    private void UpdateCropSprite()
    {
        int cropProgress = CropProgress();

        if(cropProgress < _currentCrop.daysToGrow)
        {
            _spriteRenderer.sprite = _currentCrop.growProgressSprites[cropProgress];
        }
        else
        {
            _spriteRenderer.sprite = _currentCrop.readyToHarvestSprite;
        }
    }

    private int CropProgress()
    {
        return GameManager.instance.currentDay - _dayPlanted;
    }
}