using UnityEngine;
using UnityEngine.InputSystem;


public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _gunPointerUI;
    [SerializeField] private GameObject _pauseMenuUI;

    private InputManager _inputManager;

    private void Start()
    {
        _inputManager = FindObjectOfType<InputManager>();
    }
    public void ActiveHUD()
    {
        _gunPointerUI.SetActive(true);
    }
    public void InactiveHUD()
    {
        _gunPointerUI.SetActive(false);
    }

    public void SetPauseMenu(InputAction.CallbackContext context)
    {
        bool value = !_pauseMenuUI.activeSelf;
        _pauseMenuUI.SetActive(value);

        if (value)
        {
            _inputManager.DisablePlayerInteraction();
            InactiveHUD();
        }
        else
        {
            ActiveHUD();
            _inputManager.EnablePlayerInteraction();
        }

        Time.timeScale = 1 - Time.timeScale;
    }


}
