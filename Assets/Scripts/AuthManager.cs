using UnityEngine;
using StrapiForUnity;

public class AuthManager : MonoBehaviour
{
    public static AuthManager Instance { get; private set; }
    public AuthResponse CurrentUser { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // persists across scenes
    }

    public void SetUser(AuthResponse user)
    {
        CurrentUser = user;
    }
}