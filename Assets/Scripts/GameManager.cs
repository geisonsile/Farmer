using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
    public static GameManager instance;

    [SerializeField] int totalItens = 5;
    [SerializeField] int qtdPumpkins;
    [SerializeField] int qtdBeets;
    
    [SerializeField] TextMeshProUGUI txtPumpkinsQtd;
    [SerializeField] TextMeshProUGUI txtBeetsQtd;
    [SerializeField] Transform positionItens;

    [SerializeField] AudioClip sellItemSFX;

    //public List<string> items;
    public List<GameObject> itemObjects;

    public List<GameObject> beetPrefabs;
    public List<GameObject> pumpkinPrefabs;


    void Awake() 
    {
        instance = this;
    }
    
    void Start()
    {
        SetScore(EnumSensor.BEET_FRUIT, 0);
        SetScore(EnumSensor.PUMPKIN_FRUIT, 0);
    }

    public void SetScore(EnumSensor itemIndex, int increment)
    {
        if (itemIndex == EnumSensor.PUMPKIN_FRUIT)
        {
            qtdPumpkins += increment;
            txtPumpkinsQtd.text = string.Format("{0}/{1}", qtdPumpkins.ToString(), totalItens);
        }
        else if (itemIndex == EnumSensor.BEET_FRUIT)
        {
            qtdBeets+= increment;
            txtBeetsQtd.text = string.Format("{0}/{1}", qtdBeets.ToString(), totalItens);
        }

        if(increment > 0)
            AudioSource.PlayClipAtPoint(sellItemSFX, Camera.main.transform.position);
    }

    public Vector3 GetPositionItens()
    {
        var offset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        return positionItens.position + offset;
    }

    public bool CheckFinish()
    {
        return qtdBeets >= totalItens && qtdPumpkins >= totalItens;
    }
}
