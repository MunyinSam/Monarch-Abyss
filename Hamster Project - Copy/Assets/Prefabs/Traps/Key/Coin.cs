using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value;
    public GameObject Door;
    static int currentKey = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            CoinCounter.instance.IncreaseKeys(value);
            currentKey = currentKey + value;
            print(currentKey);

            // Check the condition after updating currentKey
            if (currentKey >= 3)
            {
                Door.GetComponent<BoxCollider2D>().enabled = false;
                print("Door Opened");
            }
        }
    }
}