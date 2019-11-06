using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;

/**
 * Leaderboard Manager class to generate the leaderbaord based on scores currently stored in the database.
 * @author Lasnier Roman
 */
public class LeaderboardManager : MonoBehaviour
{
    /**
     * Player model class with all information necessary per user for the leaderboard.
     */
    private class PlayerInfo
    {
        [SerializeField] public String playerName;
        [SerializeField] public int points;

        public PlayerInfo(String playerName, int points)
        {
            this.playerName = playerName;
            this.points = points;
        }
    }

    [SerializeField] private GameObject playerRankTemplate = null;

    private List<PlayerInfo> playersRanked;

    void Start()
    {
        GenerateWithDatabaseRecords();
    }

    /**
     * Method to generate the leaderboard with a sample set of data automatically generated.
     * Intended to be used for testing purposes only.
     */
    private void GenerateWithSampleData()
    {
        for (int i = 1; i <= 50; i++)
        {
            SpawnPlayerRankObj("Player " + i, Convert.ToInt32(498647 / (i*i)), i);
        }
    }

    /**
     * Method to generate the leaderboard with user data fetched from Firebase.
     */
    private async void GenerateWithDatabaseRecords()
    {
        await FetchPlayersRanked();

        for (int i = 0; i < playersRanked.Count; i++)
        {
            SpawnPlayerRankObj(playersRanked[i].playerName, playersRanked[i].points, i + 1);
        }
    }

    /**
     * Method to spawn a PlayerRankInfo object in the Scroll View with the given player data.
     */
    private void SpawnPlayerRankObj(string playerName, int points, int rank)
    {
        GameObject playerRank = Instantiate(playerRankTemplate) as GameObject;

        PlayerRankInfo playerRankInfo = playerRank.GetComponent<PlayerRankInfo>();
        playerRankInfo.SetPlayerName(playerName);
        playerRankInfo.SetPoints(points);
        playerRankInfo.SetRank(rank);

        playerRank.transform.SetParent(playerRankTemplate.transform.parent, false);
        playerRank.SetActive(true);
    }

    /**
     * Method to fetch a list of all players ranked (ordered by score).
     * Stores the fetched data as a list of PlayerInfo instances in the playersRanked attribute.
     */
    private async Task FetchPlayersRanked()
    {
        // Instantiate the Firebase request for Leaderboard data
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://cz3003-waffles.firebaseio.com/");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        DataSnapshot leaderboardSnapshot = await FirebaseDatabase.DefaultInstance.GetReference("Leaderboard").GetValueAsync();

        // Build the player list based on all snapshot data
        List<PlayerInfo> players = new List<PlayerInfo>();
        foreach (var user in leaderboardSnapshot.Children)
        {
            String playerName = user.Child("Name").GetValue(true).ToString();
            int points = Convert.ToInt32(user.Child("Score").GetValue(true));
            players.Add(new PlayerInfo(playerName, points));
        }

        // Order and store the final list
        playersRanked = players.OrderByDescending(player => player.points).ToList();
    }
}
