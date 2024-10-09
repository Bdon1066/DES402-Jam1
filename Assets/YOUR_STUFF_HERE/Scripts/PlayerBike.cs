using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBike : MonoBehaviour
{

    [SerializeField] PlayerBikeStatistic stats;
    [SerializeField] BikeGame bikeGame;

    float bikePower = 0;
    int m_ScreenID = -1;


    void Update()
    {
        transform.position += Vector3.up * bikePower * Time.deltaTime;
    }
    private void LateUpdate()
    {
        bikePower = Mathf.MoveTowards(bikePower, 0, stats.bikePowerDecayRate);
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
}
