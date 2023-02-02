using UnityEngine;

[CreateAssetMenu(fileName = "Crop Data", menuName = "New Crop Data")]
public class CropData : ScriptableObject
{
    public Sprite[] growProgressSprites;
    public Sprite readyToHarvestSprite;
    public int daysToGrow;
    public int purchasePrice;
    public int sellPrice;
}