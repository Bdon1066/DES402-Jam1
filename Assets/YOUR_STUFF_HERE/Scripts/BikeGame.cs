using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BikeGame : MinigameBase
{
    [Header("BikeGame Variables")]
    [SerializeField] private PlayerBike[] players;
    [SerializeField] EnviromentManager enviromentManager;
    [SerializeField] CountdownTimer countdownTimer;
    [SerializeField] PlayerPositionDisplay playerPositionDisplay;

    [Tooltip("How much score each player gets for 1st place, 2nd place and so on.")]
    [SerializeField] int[] playerRacePositionScore = new int[4];

    [SerializeField] AudioClip endGameAudio;
    

    //The playerID's sorted in order of race position
    int[] playerRacePositions = new int[4];

    bool allowInput = true;
    private void Awake()
    {
        MinigameStart.AddListener(StartGame);
    }

    /// <summary>
    /// This function is called at the end of the game so that it knows what to display on the score screen.
    /// You give it information about what each players score was, how much time they earned individually, and also how much time they've earned together
    /// </summary>
    /// <returns>A class that contains all the necessary information to display the score page</returns>
    public override GameScoreData GetScoreData()
    {
        //Here's an example of how you might generate scores
        int teamTime = 0;
        GameScoreData gsd = new GameScoreData();
        for (int i = 0; i < 4; i++)
        {
            if (PlayerUtilities.GetPlayerState(i) == Player.PlayerState.ACTIVE)
            {
                playerRacePositions = DeterminePlayerRacePositions();
                //each player is scored on how far they were from the finish
                gsd.PlayerScores[playerRacePositions[i]] = playerRacePositionScore[i];
                
                gsd.PlayerTimes[i] = gsd.PlayerScores[i] * 2;   //Each player gets two seconds per point scored
                teamTime += gsd.PlayerTimes[i];                 //Keep a running total of the total time scored by all players
            }
        }
        gsd.ScoreSuffix = " points";    //This lets you write something after the player's score.
        gsd.TeamTime = teamTime;
        return gsd;
    }

    public override void OnDirectionalInput(int playerIndex, Vector2 direction)
    {

    }

    public override void OnPrimaryFire(int playerIndex)
    {
        if (!allowInput) { return; }

        players[playerIndex].HandleButtonInput(0);
        enviromentManager.enviroments[playerIndex].HandleButtonInput(0);

        playerRacePositions = DeterminePlayerRacePositions();
        playerPositionDisplay.UpdateDisplays(playerRacePositions);
    }

    public override void OnSecondaryFire(int playerIndex)
    {
        if (!allowInput){return;}

        players[playerIndex].HandleButtonInput(1);
        enviromentManager.enviroments[playerIndex].HandleButtonInput(1);

        playerRacePositions = DeterminePlayerRacePositions();
        playerPositionDisplay.UpdateDisplays(playerRacePositions);
    }

    public override void TimeUp()
    {
        //Do you want to do something when the minigame timer runs out?
        //This is where you do that!
    }

    protected override void OnResetGame()
    {

    }

    public void OnFinishReached()
    {
        OnGameComplete(true);
        PlayerAudioManager.PlayGlobalOneShot(endGameAudio);
    }

    public void StartGame()
    {
        print("STAAART");
        for (int i = 0; i < 4; i++)
        {
            players[i].Reset();
            enviromentManager.enviroments[i].Reset();
        }
        countdownTimer.BeginCountdown();
        StartCoroutine(DisableInputForSeconds(4f));
    }
    private IEnumerator DisableInputForSeconds(float seconds)
    {
        allowInput = false;
        yield return new WaitForSeconds(seconds);
        allowInput = true;
    }

    int[] DeterminePlayerRacePositions()
    {
        Dictionary<int, float> playerDistances = new Dictionary<int, float>();
        int[] playerPositions = new int[4];

        //get each player's distance from the finish line
        for (int i = 0; i < 4; i++) {
            var distanceFromFinish = enviromentManager.enviroments[i].finishLine.position.y - players[i].transform.position.y;
            playerDistances.Add(i,(distanceFromFinish));
        }
        //create a list from the dictionary (TODO: could just make the dictionary a keyvalue list tbh)
        var playerIDsAndDistances = new List<KeyValuePair<int, float>>(playerDistances);

        //Sort the list based on distnace (shortest distance goes first, then second shortest etc.)
        playerIDsAndDistances.Sort((a, b) => a.Value.CompareTo(b.Value));
        
        //get each playerID from the sorted list
        for (int i = 0; i < 4; i++){
            playerPositions[i] = playerIDsAndDistances[i].Key;
            //print("Player " + (playerPositions[i] + 1) + " came in " + (i + 1) + "th place");
        }
        
        return playerPositions; 
        //in order of race position (1st place is element 0, 2nd is element 1 etc.)
    }

}
