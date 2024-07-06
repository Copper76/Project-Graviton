using UnityEngine;
using UnityEngine.UIElements;
using UIButton = UnityEngine.UIElements.Button;

public class MenuUI : MonoBehaviour
{
    private static class UIClassNames
    {
        public const string MAIN_MENU = "main-menu";
        public const string MAIN_MENU_HIDDEN = MAIN_MENU + "--hidden";
        public const string MAIN_MENU_BUTTONS = "main-menu-buttons";
    }


    private static class UINames
    {
        public const string MAIN_MENU = "MainMenu";
        public const string START_BUTTON = "StartButton";
        public const string CREDIT_BUTTON = "CreditButton";
        public const string EXIT_BUTTON = "ExitButton";
        
    }

    [SerializeField] private UIDocument _uiDocument;
    [SerializeField] private UIDocument _creditsScreenUIDocument;


    //UI
    private VisualElement _root;
    private VisualElement _mainMenu;
    private VisualElement _mainMenuButtons;
    private VisualElement _creditsScreenRoot;

    private UIButton _startButton;
    private UIButton _creditButton;
    private UIButton _exitButton;
    private UIButton _creditsBackButton;
    
    private SceneController _sceneController;


    private void Awake()
    {
        if (_uiDocument == null || _creditsScreenUIDocument == null)
        {
            Debug.LogError("UIDocument references are not assigned.");
            return;
        }

        _root = _uiDocument.rootVisualElement;
        _mainMenu = _root.Q<VisualElement>(className: UIClassNames.MAIN_MENU);
        _startButton = _root.Q<UIButton>(UINames.START_BUTTON);
        _creditButton = _root.Q<UIButton>(UINames.CREDIT_BUTTON);
        _exitButton = _root.Q<UIButton>(UINames.EXIT_BUTTON);
        _mainMenuButtons = _root.Q<VisualElement>(className: UIClassNames.MAIN_MENU_BUTTONS);
        _creditsScreenRoot = _creditsScreenUIDocument.rootVisualElement;
        _creditsBackButton = _creditsScreenRoot.Q<UIButton>("CreditBackButton");
        _creditsScreenRoot.style.display = DisplayStyle.None;

        _sceneController = FindObjectOfType<SceneController>();
    }

    private void Start()
    {
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
    }

    private void OnEnable()
    {
        _startButton.clicked += OnStartButtonClicked;
        _creditButton.clicked += OnCreditButtonClicked;
        _exitButton.clicked += OnExitButtonClicked;
        _creditsBackButton.clicked += OnCreditsBackButtonClicked;
    }

    private void OnDestroy()
    {
        _startButton.clicked -= OnStartButtonClicked;
        _creditButton.clicked -= OnCreditButtonClicked;
        _exitButton.clicked -= OnExitButtonClicked;
        _creditsBackButton.clicked -= OnCreditsBackButtonClicked;
    }

    void OnStartButtonClicked()
    {
        Debug.Log("Start button clicked");
        _sceneController.LoadNextScene(0.1f);
    }

    void OnCreditButtonClicked()
    {
        Debug.Log("Credit button clicked");
        _creditsScreenRoot.style.display = DisplayStyle.Flex;
    }

    void OnExitButtonClicked()
    {
        Debug.Log("Exit button clicked");
        Application.Quit();
    }
    void OnCreditsBackButtonClicked()
    {
        Debug.Log("credit back button clicked");
        _creditsScreenRoot.style.display = DisplayStyle.None;
    }

    private void SetMainMenuVisibility(bool isVisible)
    {
        _mainMenu.EnableInClassList(UIClassNames.MAIN_MENU_HIDDEN, !isVisible);
        _mainMenuButtons.style.display = isVisible ? DisplayStyle.Flex : DisplayStyle.None;
    }


}
