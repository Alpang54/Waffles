using UnityEngine;
using UnityEngine.UI;

public class PlayerRankInfo : MonoBehaviour
{
    [SerializeField] private Text playerName;
    [SerializeField] private Text points;
    [SerializeField] private Text rank;

    public void SetPlayerName(string playerName)
    {
        this.playerName.text = playerName;
    }

    public void SetPoints(int points)
    {
        this.points.text = points.ToString();
    }

    public void SetRank(int rank)
    {
        this.rank.text = rank.ToString();
    }
}
