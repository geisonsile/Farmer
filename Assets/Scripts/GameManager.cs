using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public int coins;
    public int coinsPerFruit = 5;

    public int qtdPumpkins;
    public int qtdBeets;
    public TextMeshProUGUI txtPumpkinsQtd;
    public TextMeshProUGUI txtBeetsQtd;

    public Transform depositBoxTransform;

    public List<string> items;
    public List<GameObject> itemObjects;

    public List<GameObject> beetPrefabs;
    public List<GameObject> pumpkinPrefabs;

    public bool tutorialShow = true;

    void Awake() {
        Instance = this;
    }
    
    void Start()
    {
        
    }
}
