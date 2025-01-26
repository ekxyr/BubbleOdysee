using System;
using Unity.VisualScripting;
using UnityEngine;

public class WorldScript : MonoBehaviour
{
   public static int collectedCoins = 0;

   public static void collectCoin()
   {
      collectedCoins += 1;
      print(collectedCoins + " Coins collected");
   }
}
