using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Map map;
    GameEngine engine;
    float nextRoundTime = 0.0f;
    float period = 1.0f;
    // Start is called before the first frame update
    void Start()
    {

        engine = new GameEngine();//create GameEngine instance
        map = new Map(6, 3, 20, 20, 6);//create Map instance
        engine.MapHandle = map;
        GameEngine.Rounds = 0;//Reset rounds counter

        map.SpawnUnits();
        map.SpawnBuildings();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextRoundTime)
        {
            nextRoundTime += period;
            // execute block of code here
            engine.GameUpdater();
        }
    }
}
