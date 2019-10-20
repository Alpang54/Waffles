using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;
using System;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private GameObject playerRankTemplate = null;

    // private List<Player> playersRanked;

    void Start()
    {
        GenerateWithSampleData();
	}

    private void GenerateWithSampleData()
    {
        for (int i = 1; i <= 50; i++)
        {
            SpawnPlayerRankObj("Player " + i, Convert.ToInt32(498647 / (i*i)), i);
        }
    }

    /* 
    private async void GenerateWithDatabaseRecords()
    {
        await FetchPlayersRanked();

        for (int i = 0; i < playersRanked.Count; i++)
        {
            SpawnPlayerRankObj(playersRanked[i].name, playersRanked[i].points, i + 1);
        }
    }
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

    /* 
    private async Task FetchPlayersRanked()
    {
        GameObject DBHandler = GameObject.Find("DBHandler");
        DBHandler DBHandlerScript = DBHandler.GetComponent<DBHandler>();

        await DBHandlerScript.ReadfromFirebase("User");
        DataSnapshot snapshotOfUser = DBHandlerScript.GetSnapshot();

        List<Player> players = new List<Player>();

        foreach (var user in snapshotOfUser.Children)
        {
            players.Add(user.GetValue(Player));
        }

        playersRanked = players.OrderByDescending(player => player.points).ToList();
    }
    */
}
