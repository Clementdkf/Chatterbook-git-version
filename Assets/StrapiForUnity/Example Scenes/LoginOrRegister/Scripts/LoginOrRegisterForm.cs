using System;
using System.Collections;
using System.Collections.Generic;
using StrapiForUnity;
using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
//using UnityEditor.Localization.Plugins.XLIFF.V12;

public class LoginOrRegisterForm : MonoBehaviour
{
    public Button LoginToggleButton;
    public Button RegisterToggleButton;
    public Button LoginSubmitButton;
    public Button RegisterSubmitButton;
    public InputField UsernameInput;
    public InputField EmailInput;
    public InputField PasswordInput;
    public TextMeshProUGUI HeaderText;
    public TextMeshProUGUI WrongText; 
    public VerticalLayoutGroup ContainerLayout;
    public GameObject LoadingObject;
    public Toggle RememberMeToggle;
    
    
    public StrapiComponent Strapi;
    public TMP_FontAsset chineseFont;
    public TMP_FontAsset englishFont;
    
    private LocalizedString loginHeader = new LocalizedString("風text", "login_header");
    private LocalizedString registerHeader = new LocalizedString("風text", "register_header");

    private bool isLoading = false;

    // Start is called before the first frame update
    void Start()
    {
        if (Strapi == null || HeaderText == null)
        {
            Debug.LogError("No Strapi component found. Please make sure you've got an active Strapi component assigned to the LoginOrRegisterForm");
            return;
        }

        LoginToggleButton.onClick.AddListener(OnLoginToggle);
        RegisterToggleButton.onClick.AddListener(OnRegisterToggle);
        
        LoginToggleButton.onClick.Invoke();
        LoadingObject.SetActive(false);
        WrongText.gameObject.SetActive(false);

        registerEventHandlers();
        Debug.Log(HeaderText);
    }

    private void registerEventHandlers()
    {
        Strapi.OnAuthSuccess += handleSuccessfulAuthentication;
        Strapi.OnAuthFail += handleUnsuccessfulAuthentication;
        Strapi.OnAuthStarted += toggleLoading;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (LoginSubmitButton.IsActive())
            {
                LoginSubmitButton.onClick.Invoke();
            }
            else
            {
                RegisterSubmitButton.onClick.Invoke();
            }
        }
    }

    void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    private void OnLocaleChanged(UnityEngine.Localization.Locale locale)
    {
        if (locale.Identifier.Code.StartsWith("en")) // English locale
        {
            HeaderText.font = englishFont;
        }
        else if (locale.Identifier.Code.StartsWith("zh")) // Chinese locale
        {
            HeaderText.font = chineseFont;
        }
    }

    public void OnLoginToggle()
    {
        RegisterSubmitButton.gameObject.SetActive(false);
        LoginSubmitButton.gameObject.SetActive(true);
        EmailInput.transform.parent.gameObject.SetActive(false);
        RegisterToggleButton.gameObject.SetActive(true);
        LoginToggleButton.gameObject.SetActive(false);
        loginHeader.StringChanged += (value) => HeaderText.text = value;
        loginHeader.RefreshString();
        
        forceLayoutUpdate();
    }

    public void OnRegisterToggle()
    {
        RegisterSubmitButton.gameObject.SetActive(true);
        LoginSubmitButton.gameObject.SetActive(false);
        EmailInput.transform.parent.gameObject.SetActive(true);
        RegisterToggleButton.gameObject.SetActive(false);
        LoginToggleButton.gameObject.SetActive(true);
        registerHeader.StringChanged += (value) => HeaderText.text = value;
        registerHeader.RefreshString();
        Debug.Log(HeaderText.text);
        
        forceLayoutUpdate();
    }
    
    private void forceLayoutUpdate()
    {
        Canvas.ForceUpdateCanvases();
        ContainerLayout.enabled = false;
        ContainerLayout.enabled = true;
    }

    public void OnLoginSubmit()
    {
        Strapi.Login(UsernameInput.text, PasswordInput.text, RememberMeToggle.isOn);
    }

    public void OnRegisterSubmit()
    {
        Strapi.Register(UsernameInput.text, EmailInput.text, PasswordInput.text, RememberMeToggle.isOn);
    }

    private void toggleLoading()
    {
        isLoading = !isLoading;
        LoadingObject.SetActive(isLoading);
    }

    private void handleSuccessfulAuthentication(AuthResponse authUser)
    {
        toggleLoading();
        HeaderText.text = $"Success. Welcome {authUser.user.username}";
        RegisterSubmitButton.gameObject.SetActive(false);
        LoginSubmitButton.gameObject.SetActive(false);
        UsernameInput.transform.parent.gameObject.SetActive(false);
        EmailInput.transform.parent.gameObject.SetActive(false);
        PasswordInput.transform.parent.gameObject.SetActive(false);
        RegisterToggleButton.gameObject.SetActive(false);
        LoginToggleButton.gameObject.SetActive(false);
        RememberMeToggle.gameObject.SetActive(false);
        //AuthManager.Instance.SetUser(authUser);
        SceneManager.LoadScene("SampleScene");
    }

    private void handleUnsuccessfulAuthentication(Exception error)
    {
        toggleLoading();
        Debug.Log($"Authentication Error: {error.Message}");
        //TestingText.text = strapiComponent.AuthenticatedUser.username;
        if (UsernameInput.text != Strapi.AuthenticatedUser.username)
        {
            WrongText.gameObject.SetActive(true);
        }
    }
}
