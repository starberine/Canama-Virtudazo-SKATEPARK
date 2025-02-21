using UnityEngine;

public abstract class SkateboardCharacter : MonoBehaviour
{
    public string characterName;
    public float speed;
    public float jumpHeight;
    public float gravity;

    protected CharacterController controller;
    protected Vector3 velocity;
    protected bool isGrounded;
    protected Animator animator;

    protected virtual void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        if (animator == null)
        {
            Debug.LogError($"❌ Animator component NOT FOUND on {characterName}!");
        }
    }


    public void SaveState()
    {
        PlayerPrefs.SetString("CharacterName", characterName);
        PlayerPrefs.SetFloat("Speed", speed);
        PlayerPrefs.SetFloat("JumpHeight", jumpHeight);
        PlayerPrefs.SetFloat("Gravity", gravity);
    }

    public void LoadState()
    {
        characterName = PlayerPrefs.GetString("CharacterName", "Default");
        speed = PlayerPrefs.GetFloat("Speed", 5.0f);
        jumpHeight = PlayerPrefs.GetFloat("JumpHeight", 1.5f);
        gravity = PlayerPrefs.GetFloat("Gravity", -9.81f);
    }
}
