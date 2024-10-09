using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles the per-screen scrolling of the enviroment.
/// </summary>
public class EnviromentScroller : MonoBehaviour
{
    public Transform finishLine;
    [SerializeField] PlayerBikeStatistic scrollingStats;

    float scrollPower = 0;

    Vector2 startingPos;

    private void Start()
    {
        startingPos = transform.position;
    }
    void Update()
    {
        transform.position -= Vector3.up * scrollPower * Time.deltaTime;
    }
    private void LateUpdate()
    {
        scrollPower = Mathf.MoveTowards(scrollPower, 0, scrollingStats.bikePowerDecayRate);
    }

    public void HandleButtonInput(int buttonID)
    {
        scrollPower += scrollingStats.buttonPressPower * ((scrollPower / scrollingStats.maxBikePower) + 1);
        scrollPower = Mathf.Clamp(scrollPower, 0, scrollingStats.maxBikePower);
    }
    public void Reset()
    {
        transform.position = startingPos;
        scrollPower = 0;
    }
}
