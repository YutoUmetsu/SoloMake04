using UnityEngine;
// ▼ 新しいInput Systemのネームスペースを追加
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class GravityPlayer : MonoBehaviour
{
    [Header("Gravity Settings")]
    [SerializeField] private float baseGravity = 12f;

    [Header("Ground / Ceiling Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 checkSize = new Vector2(0.6f, 0.1f);
    [SerializeField] private LayerMask groundLayer;

    [Header("Visual Juice")]
    [SerializeField] private Transform visualTransform;
    [SerializeField] private float scaleLerpSpeed = 20f;

    private Rigidbody2D rb;
    private bool isGravityDown = true;
    private float targetScaleY = 1f;
    private float originalScaleX;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = baseGravity;

        if (visualTransform == null) visualTransform = transform;
        originalScaleX = visualTransform.localScale.x;
        targetScaleY = visualTransform.localScale.y;
    }

    void Update()
    {
        // ▼ 新しいInput Systemの書き方に変更（スペースキー ＆ 左クリック対応）
        bool isPressed = (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame) ||
                         (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame);

        if (IsGrounded() && isPressed)
        {
            ToggleGravity();
        }

        Vector3 currentScale = visualTransform.localScale;
        currentScale.y = Mathf.Lerp(currentScale.y, targetScaleY, Time.deltaTime * scaleLerpSpeed);
        visualTransform.localScale = currentScale;
    }

    void ToggleGravity()
    {
        isGravityDown = !isGravityDown;
        rb.gravityScale = isGravityDown ? baseGravity : -baseGravity;
        targetScaleY = isGravityDown ? Mathf.Abs(originalScaleX) : -Mathf.Abs(originalScaleX);
    }

    bool IsGrounded()
    {
        if (groundCheck == null) return true;
        Collider2D hit = Physics2D.OverlapBox(groundCheck.position, checkSize, 0f, groundLayer);
        return hit != null;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundCheck.position, checkSize);
    }
}