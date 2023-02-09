using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour 
{

    public static GameManager instance;

    public int totalItens = 5;
    public int qtdPumpkins;
    public int qtdBeets;
    public TextMeshProUGUI txtPumpkinsQtd;
    public TextMeshProUGUI txtBeetsQtd;

    public Transform positionItensTransform;

    public List<string> items;
    public List<GameObject> itemObjects;

    public List<GameObject> beetPrefabs;
    public List<GameObject> pumpkinPrefabs;

    public bool tutorialShow = true;

    void Awake() 
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    void Start()
    {
        
    }
}
