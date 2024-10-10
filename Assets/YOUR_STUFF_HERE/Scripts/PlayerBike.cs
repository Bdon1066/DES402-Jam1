using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerBike : MonoBehaviour
{

    [SerializeField] PlayerBikeStatistic stats;
    [SerializeField] BikeGame bikeGame;

    float bikePower = 0;

    Vector2 startingPos;
    float restingPower = 0;
    SpriteRenderer spriteRenderer;
    int currentSpriteIndex;
    float yVelocity;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        startingPos = transform.position;
    }
    void Update()
    {
        //transform.position += Vector3.up * bikePower * Time.deltaTime;
        
        //if above middle, we will tend towards falling,otherwise not
        //restingPower = (transform.position.y > 0) ? -1 : 0;

        var step = transform.position.y + bikePower * Time.deltaTime;
        float newYPosition = Mathf.SmoothDamp(transform.position.y, step, ref yVelocity, 0.3f);
        transform.position = new Vector3(transform.position.x, newYPosition, transform.position.z);
    }
    private void LateUpdate()
    {
        bikePower = Mathf.MoveTowards(bikePower, restingPower, stats.bikePowerDecayRate);
    }

    public void HandleButtonInput(int buttonID)
    {
        bikePower += stats.buttonPressPower * ((bikePower / stats.maxBikePower) + 1);
        bikePower = Mathf.Clamp(bikePower, 0, stats.maxBikePower);

        CycleAnimationSprites();
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FinishLine"))
        {
            bikeGame.OnFinishReached();
        }
    }
    public void Reset()
    {
        transform.position = startingPos;
        bikePower = 0;
    }
    void CycleAnimationSprites()
    {
        if (currentSpriteIndex > stats.playerAnimationSprites.Length - 1)
        {
            currentSpriteIndex = 0;
        }
        spriteRenderer.sprite = stats.playerAnimationSprites[currentSpriteIndex];
        currentSpriteIndex++;
    }
}
