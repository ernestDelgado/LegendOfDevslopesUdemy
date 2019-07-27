using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int startingHealth = 100;
    [SerializeField] float timeSinceLastHit = 2f;
    [SerializeField] Slider healthSlider;

    private float timer = 0f;
    private CharacterController characterController;
    private Animator anim;
    private int currentHealth;
    private AudioSource audio;
    private ParticleSystem blood;

    public int CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            if (value < 0) currentHealth = 0;
            else currentHealth = value;
        }
    }

    private void Awake()
    {
        Assert.IsNotNull(healthSlider);
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        CurrentHealth = startingHealth;
        audio = GetComponent<AudioSource>();
        blood = GetComponentInChildren<ParticleSystem>();
        healthSlider.value = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (timer >= timeSinceLastHit && !GameManager.instance.GameOver)
        {
            if (other.tag == "Weapon")
            {
                takeHit();
                timer = 0;
            }
        }
    }

    void takeHit()
    {
        if (CurrentHealth > 0)
        {
            GameManager.instance.PlayerHit(CurrentHealth);
            anim.Play("Hurt");
            CurrentHealth -= 10;
            healthSlider.value = CurrentHealth;
            audio.PlayOneShot(audio.clip);
            blood.Play();
        }
        if (CurrentHealth <= 0)
        {
            killPlayer();
        }
    }

    void killPlayer()
    {
        GameManager.instance.PlayerHit(CurrentHealth);
        anim.SetTrigger("HeroDie");
        characterController.enabled = false;
        audio.PlayOneShot(audio.clip);
        blood.Play();
    }

    public void PowerUpHealth()
    {
        if (currentHealth <= 70)
        {
            CurrentHealth += 30;
        } else if (currentHealth < startingHealth)
        {
            CurrentHealth = startingHealth;
        }
        healthSlider.value = currentHealth;
    }
}
