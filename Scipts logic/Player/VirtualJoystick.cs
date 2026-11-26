using UnityEngine;
using UnityEngine.InputSystem;

// Joystick virtuel flottant
public class VirtualJoystick : MonoBehaviour
{
    [Header("Réglages")]
    [SerializeField] private float radiusInInches = 0.5f;            // course du stick, en pouces réels
    [SerializeField] [Range(0f, 0.9f)] private float deadZone = 0.15f;
    [SerializeField] [Range(0.1f, 1f)] private float activeAreaHeight = 0.5f;   // zone tactile (part de l'écran depuis le bas)

    [Header("Visuels (optionnels)")]
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform handle;

    private bool isActive;
    private Vector2 origin;
    private Vector2 direction;

    // Direction entre (-1, -1) et (1, 1), longueur entre 0 et 1.
    public Vector2 Direction
    {
        get { return direction; }
    }

    public bool IsActive
    {
        get { return isActive; }
    }

    private void Start()
    {
        ShowVisuals(false);
    }

    private void Update()
    {
        bool isPressed = false;
        bool justPressed = false;
        Vector2 pointerPosition = Vector2.zero;

        // Sur téléphone 
        Touchscreen touchscreen = Touchscreen.current;
        bool touchUsed = false;

        if (touchscreen != null)
        {
            if (touchscreen.primaryTouch.press.isPressed || touchscreen.primaryTouch.press.wasReleasedThisFrame)
            {
                isPressed = touchscreen.primaryTouch.press.isPressed;
                justPressed = touchscreen.primaryTouch.press.wasPressedThisFrame;
                pointerPosition = touchscreen.primaryTouch.position.ReadValue();
                touchUsed = true;
            }
        }

        // Dans l'éditeur
        Mouse mouse = Mouse.current;

        if (!touchUsed && mouse != null)
        {
            isPressed = mouse.leftButton.isPressed;
            justPressed = mouse.leftButton.wasPressedThisFrame;
            pointerPosition = mouse.position.ReadValue();
        }

        if (!isActive)
        {
            bool inActiveArea = pointerPosition.y <= Screen.height * activeAreaHeight;

            if (justPressed && inActiveArea)
            {
                isActive = true;
                origin = pointerPosition;
                ShowVisuals(true);
            }
        }

        if (isActive)
        {
            if (!isPressed)
            {
                Stop();
                return;
            }

            UpdateDirection(pointerPosition);
        }
    }

    private void UpdateDirection(Vector2 pointerPosition)
    {
        float dpi = Screen.dpi;

        if (dpi <= 0f)
        {
            dpi = 160f;   // certains appareils ne donnent pas leur dpi
        }

        float radiusInPixels = radiusInInches * dpi;
        Vector2 offset = Vector2.ClampMagnitude(pointerPosition - origin, radiusInPixels);
        Vector2 input = offset / radiusInPixels;
        float strength = input.magnitude;

        if (strength < deadZone)
        {
            direction = Vector2.zero;
        }
        else
        {
            // On remet la force entre 0 et 1
            float scaledStrength = (strength - deadZone) / (1f - deadZone);
            direction = input.normalized * scaledStrength;
        }

        if (background != null)
        {
            background.position = origin;
        }

        if (handle != null)
        {
            handle.position = origin + offset;
        }
    }

    private void Stop()
    {
        isActive = false;
        direction = Vector2.zero;
        ShowVisuals(false);
    }

    private void ShowVisuals(bool visible)
    {
        if (background != null)
        {
            background.gameObject.SetActive(visible);
        }

        if (handle != null)
        {
            handle.gameObject.SetActive(visible);
        }
    }
}