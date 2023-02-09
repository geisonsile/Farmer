using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soil : MonoBehaviour
{
    public bool isWet;
    public float timeToDry = 120;
    public Material materialDry;
    public Material materialWet;

    public EnumSensor seedIndex;
    private int oldCropStage;
    public int cropStage;
    public GameObject cropObject;

    private MeshRenderer thisMeshRenderer;
    private float dryCooldown = 0;

    private float growInterval = 1;
    private float growCooldown = 0;
    private float growChance = 0.025f;

    [SerializeField] AudioClip plantingSeedsSFX, wateringPlantsSFX, croopSFX;

    void Awake()
    {
        thisMeshRenderer = GetComponent<MeshRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        dryCooldown = timeToDry;
    }

    // Update is called once per frame
    void Update() 
    {
        // Update material
        thisMeshRenderer.material = isWet ? materialWet : materialDry;

        // Dry soil
        if(isWet) 
        {
            dryCooldown -= Time.deltaTime;
            if(dryCooldown <= 0) 
            {
                isWet = false;
            }
        }

        // Update crops
        if(oldCropStage != cropStage) 
        {
            // Remove crops
            if(cropObject != null) 
            {
                Destroy(cropObject);
            }

            // Plant crops
            if(cropStage > 0) 
            {
                var gm = GameManager.instance;
                var prefabs = seedIndex == EnumSensor.SEED_BEET ? gm.beetPrefabs : gm.pumpkinPrefabs;
                var cropPrefab = prefabs[cropStage - 1];

                var position = transform.position;
                var rotation = cropPrefab.transform.rotation
                 * Quaternion.Euler(Vector3.up * Random.Range(0, 360));
                cropObject = Instantiate(cropPrefab, position, rotation);
            }
        }
        oldCropStage = cropStage;

        // Grow crops
        if(!IsEmpty() && !IsFinished()) 
        {
            if((growCooldown -= Time.deltaTime) <= 0) 
            {
                growCooldown = growInterval;
                var realChance = growChance;
                if(isWet) 
                {
                    realChance *= 2f;
                }
                if(Random.Range(0f, 1f) < growChance){
                    cropStage++;   
                }
            }
        }
    }

    public void Water()
    {
        isWet = true;
        dryCooldown = timeToDry;
        AudioSource.PlayClipAtPoint(wateringPlantsSFX, Camera.main.transform.position);
    }

    public bool IsEmpty() 
    {
        return cropStage == 0;
    }

    public bool IsFinished() {
        return cropStage == 5;
    }

    public void Seed(EnumSensor index) 
    {
        if(!IsEmpty()) return;
        
        seedIndex = index;
        cropStage = 1;
        AudioSource.PlayClipAtPoint(plantingSeedsSFX, Camera.main.transform.position);
    }

    public void RemoveCrop() 
    {
        seedIndex = EnumSensor.EMPTY;
        cropStage = 0;
        
        AudioSource.PlayClipAtPoint(croopSFX, Camera.main.transform.position);
    }
}
