using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask _interactLayerMask;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _moveSpeed;

    private Vector2 _moveInput;
    private Vector2 _facingDirection;
    private bool _interactInput;

    private void Update()
    {
        // Set the facing direction
        if (_moveInput.magnitude != 0)
        {
            _facingDirection = _moveInput.normalized;
            _spriteRenderer.flipX = _moveInput.x > 0;
        }

        if (_interactInput)
        {
            TryInteractTile();
            _interactInput = false;
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = _moveInput.normalized * _moveSpeed;
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
            _interactInput = true;
    }

    private void TryInteractTile()
    {
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + _facingDirection, Vector3.up, 0.1f, _interactLayerMask);

        if(hit.collider != null)
        {
            FieldTile tile = hit.collider.GetComponent<FieldTile>();
            tile.Interact();
        }
    }
}