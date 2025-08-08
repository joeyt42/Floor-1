using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PointManager : MonoBehaviour
{

    public int points = 0;
    public TextMeshProUGUI pointsPrompt;
    public int addAmount;

    void Start()
    {
        addAmount = 0;
    }

    void Update()
    {
        if (pointsPrompt != null)
        {
            pointsPrompt.text = points.ToString();
        }
    }

    public bool SpendPoints(int amount)
    {
        if (points >= amount)
        {
            points -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public void AddPoints(int addAmount)
    {
        points += addAmount;
    }
}
