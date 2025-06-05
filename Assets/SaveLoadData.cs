using UnityEngine;

public class SaveLoadData : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


public class Save{
    public float score;
    public float coins;

    Save(float score_arg, float coin_args) {
        score = score_arg;
        coins = coin_args;
    }
}