using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject winScreen;
    public Stack[] stacks;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GameOver()
    {
        winScreen.SetActive(true);
    }
}
[System.Serializable]
public class Stack
{
    public string color;
    public GameObject obj;
}
