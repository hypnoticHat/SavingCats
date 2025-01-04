using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SimpleJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public static SimpleJoystick Instance { get; private set; }

    [Header("Joystick Components")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image handleImage;

    [Header("Settings")]
    [SerializeField] private float handleRange = 1f;
    [SerializeField] private float deadZone = 0.1f;

    private RectTransform backgroundRectTransform;
    private RectTransform handleRectTransform;

    private Canvas canvas;
    private Camera canvasCamera;

    private Vector2 input = Vector2.zero;
    public Vector3 FormatInput => new Vector3(input.x, 0, input.y);
    public bool IsActive { get; private set; } 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        backgroundRectTransform = backgroundImage.rectTransform;
        handleRectTransform = handleImage.rectTransform;

        canvas = GetComponentInParent<Canvas>();
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            canvasCamera = canvas.worldCamera;
        }

        HideJoystick();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        IsActive = true;
        backgroundRectTransform.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        ShowJoystick();
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!IsActive) return;

        Vector2 position = RectTransformUtility.WorldToScreenPoint(canvasCamera, backgroundRectTransform.position);
        Vector2 radius = backgroundRectTransform.sizeDelta / 2;
        input = (eventData.position - position) / (radius * canvas.scaleFactor);

        HandleInput(input.magnitude, input.normalized);
        handleRectTransform.anchoredPosition = input * radius * handleRange;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsActive = false;
        ResetJoystick();
        HideJoystick();
    }

    private void HandleInput(float magnitude, Vector2 normalized)
    {
        if (magnitude > deadZone)
        {
            input = normalized; 
        }
        else
        {
            input = Vector2.zero;
        }
    }

    private void ResetJoystick()
    {
        input = Vector2.zero;
        handleRectTransform.anchoredPosition = Vector2.zero;
    }

    private Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            backgroundRectTransform.parent.GetComponent<RectTransform>(),
            screenPosition,
            canvasCamera,
            out Vector2 localPoint
        );
        return localPoint;
    }

    private void ShowJoystick()
    {
        backgroundImage.enabled = true;
        handleImage.enabled = true;
    }

    private void HideJoystick()
    {
        backgroundImage.enabled = false;
        handleImage.enabled = false;
    }
}
