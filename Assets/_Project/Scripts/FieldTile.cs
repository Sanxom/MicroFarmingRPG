using UnityEngine;

public class FieldTile : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite grassSprite;
    public Sprite tilledSprite;
    public Sprite wateredTilledSprite;

    [Header("References")]
    [SerializeField] private GameObject _cropPrefab;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Crop _currentCrop;
    private bool _isTilled;

    private void Start()
    {
        // Default sprite
        _spriteRenderer.sprite = grassSprite;
    }

    public void Interact()
    {
        if (!_isTilled)
            Till();
        else if (!HasCrop() && GameManager.instance.CanPlantCrop())
            PlantNewCrop(GameManager.instance.selectedCropToPlant);
        else if (HasCrop() && _currentCrop.CanHarvest())
            _currentCrop.Harvest();
        else
            Water();
    }

    private void PlantNewCrop(CropData crop)
    {
        if (!_isTilled)
            return;

        _currentCrop = Instantiate(_cropPrefab, transform).GetComponent<Crop>();
        _currentCrop.Plant(crop);

        GameManager.instance.OnNewDay += OnNewDay;
    }

    private void Till()
    {
        _isTilled = true;
        _spriteRenderer.sprite = tilledSprite;
    }

    private void Water()
    {
        _spriteRenderer.sprite = wateredTilledSprite;

        if(HasCrop())
            _currentCrop.Water();
    }

    private void OnNewDay()
    {
        if(_currentCrop == null)
        {
            _isTilled = false;
            _spriteRenderer.sprite = grassSprite;

            GameManager.instance.OnNewDay -= OnNewDay;
        }
        else if(_currentCrop != null)
        {
            _spriteRenderer.sprite = tilledSprite;
            _currentCrop.NewDayCheck();
        }
    }

    private bool HasCrop()
    {
        return _currentCrop != null;
    }
}