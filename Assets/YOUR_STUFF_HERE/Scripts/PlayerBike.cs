using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBike : MonoBehaviour
{

    [SerializeField] PlayerBikeStatistic stats;
    [SerializeField] BikeGame bikeGame;

    float bikePower = 0;

    Vector2 startingPos;
    float restingPower = 0;

    void Start()
    {
        startingPos = transform.position;
    }
    void Update()
    {
        transform.position += Vector3.up * bikePower * Time.deltaTime;

        //if above middle, we will tend towards falling,otherwise not
        restingPower = (transform.position.y > 0) ? -1 : 0;
    }
    private void LateUpdate()
    {
        bikePower = Mathf.MoveTowards(bikePower, restingPower, stats.bikePowerDecayRate);
    }

    public void HandleButtonInput(int buttonID)
    {
        bikePower += stats.buttonPressPower * ((bikePower / stats.maxBikePower) + 1);
        bikePower = Mathf.Clamp(bikePower, 0, stats.maxBikePower);
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
}
