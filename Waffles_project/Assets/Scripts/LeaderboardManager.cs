using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
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

    private void GenerateWithSampleData()
    {
        for (int i = 1; i <= 50; i++)
        {
            SpawnPlayerRankObj("Player " + i, Convert.ToInt32(498647 / (i*i)), i);
        }
    }

    private async void GenerateWithDatabaseRecords()
    {
        await FetchPlayersRanked();

        for (int i = 0; i < playersRanked.Count; i++)
        {
            SpawnPlayerRankObj(playersRanked[i].playerName, playersRanked[i].points, i + 1);
        }
    }

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

    private async Task FetchPlayersRanked()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://cz3003-waffles.firebaseio.com/");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        DataSnapshot leaderboardSnapshot = await FirebaseDatabase.DefaultInstance.GetReference("Leaderboard").GetValueAsync();

        List<PlayerInfo> players = new List<PlayerInfo>();

        foreach (var user in leaderboardSnapshot.Children)
        {
            String playerName = user.Child("Name").GetValue(true).ToString();
            int points = Convert.ToInt32(user.Child("Score").GetValue(true));
            players.Add(new PlayerInfo(playerName, points));
        }

        playersRanked = players.OrderByDescending(player => player.points).ToList();
    }
}
