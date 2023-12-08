using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class CoinCounter : MonoBehaviour
{
   public static CoinCounter instance;

   public TMP_Text coinText;
   public int currentCoins = 0;
   

   void Awake()
   {
        instance = this;
   }
    void Start()
    {
        coinText.text = "KEYS : " + currentCoins.ToString();
    }

    public void IncreaseKeys(int v)
    {
        currentCoins += v;
        coinText.text = "KEYS : " + currentCoins.ToString();
        
    }

    
    
}
