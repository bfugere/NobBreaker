using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    // Configuration Parameters
    [SerializeField] GameObject blockSparklesVFX;
    [SerializeField] AudioClip breakSound;
    [SerializeField] float audioClipVolume = 0.05f;
    [SerializeField] Sprite[] hitSprites;
    public Transform extraLifePowerUp;
    public Transform megaPaddlePowerUp;
    public Transform extraBallPowerUp;

    // Cached Reference
    Level level;

    // State Variables
    [SerializeField] int timesHit; // Serialized for debugging purposes

    // Start Method
    private void Start()
    {
        CountBreakableBlocks();
    }

    // Methods
    private void CountBreakableBlocks()
    {
        level = FindObjectOfType<Level>();
        if (tag == "Breakable")
        {
            level.CountBlocks();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (tag == "Breakable")
        {
            // Random PowerUp Chance
            int randomPowerUpChance = Random.Range(1, 101);
            
            if (randomPowerUpChance < 3)
                Instantiate(extraLifePowerUp, collision.transform.position, collision.transform.rotation);
            else if (randomPowerUpChance < 5)
                Instantiate(megaPaddlePowerUp, collision.transform.position, collision.transform.rotation);
            else if (randomPowerUpChance < 7)
                Instantiate(extraBallPowerUp, collision.transform.position, collision.transform.rotation);

            // Then, Handle Hit
            HandleHit();

            // Debug to Check Number
            // Debug.Log("Random Number: " + randomPowerUpChance);
        }
    }

    private void HandleHit()
    {
        timesHit++;
        int MaxHits = hitSprites.Length + 1;
        if (timesHit >= MaxHits)
            DestroyBlock();
        else
            ShowNextHitSprite();
    }

    private void ShowNextHitSprite()
    {
        int spriteIndex = timesHit - 1;
        if (hitSprites[spriteIndex] != null)
            GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
        else
            Debug.LogError("Block sprite is missing from the hitSprite array: " + gameObject.name);
    }

    private void DestroyBlock()
    {
        PlayBlockDestroySFX();
        Destroy(gameObject);
        level.BlockDestroyed();
        TriggerSparklesVFX();
    }

    private void PlayBlockDestroySFX()
    {
        FindObjectOfType<GameSession>().AddToScore();
        AudioSource.PlayClipAtPoint(breakSound, Camera.main.transform.position, audioClipVolume);
    }

    private void TriggerSparklesVFX()
    {
        GameObject sparkles = Instantiate(blockSparklesVFX, transform.position, transform.rotation);
        Destroy(sparkles, 1f);
    }
}
