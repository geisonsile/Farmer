using UnityEngine;

public class PlayerCheck : MonoBehaviour 
{
    public EnumSensor itemIndex = EnumSensor.EMPTY;
    public float itemOffset = 3;
    
    private GameObject holdingObject;

    private PlayerController playerController;
    public FinishGame finishGame;


    void Start() 
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update() 
    {
        if(holdingObject != null) 
        {
            var newPosition = transform.position + new Vector3(0, itemOffset, 0);
            holdingObject.transform.position = newPosition;
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        GameObject objectItem = other.gameObject;

        // Sensor
        if (objectItem.CompareTag("Sensor"))
        {
            if (itemIndex != EnumSensor.BEET_FRUIT && itemIndex != EnumSensor.PUMPKIN_FRUIT)
            {
                Sensor sensor = objectItem.GetComponent<Sensor>();
                EnumSensor index = sensor.itemIndex;

                UpdateIndex(index);
                
                if (itemIndex != EnumSensor.EMPTY)
                {
                    playerController.isCarry = true;
                    sensor.PlayPickUpSFX();
                }
                else
                {
                    playerController.isCarry = false;
                }
            }
        }

        // Soil
        if(objectItem.CompareTag("Soil")) 
        {
            Soil soil = objectItem.GetComponent<Soil>();

            switch (itemIndex)
            { 
                case EnumSensor.WATERING_CAN:
                    soil.Water();
                    break;
                case EnumSensor.SEED_BEET: case EnumSensor.SEED_PUMPKIN:
                    if (soil.IsEmpty()) soil.Seed(itemIndex);
                    break;
                case EnumSensor.EMPTY:
                    if (soil.IsFinished())
                    {
                        EnumSensor seedIndex = soil.seedIndex;
                        soil.RemoveCrop();
                        UpdateIndex(seedIndex + 3);
                    }
                    break;
            }
        }
    }

    void OnCollisionEnter(Collision other) 
    {
        var otherObject = other.gameObject;

        // Sell box
        if(otherObject.CompareTag("Sell")) 
        {
            if(itemIndex == EnumSensor.BEET_FRUIT || itemIndex == EnumSensor.PUMPKIN_FRUIT)
            {
                var gm = GameManager.instance;

                // Teleport fruit
                var fruitObject = holdingObject;
                holdingObject = null;
                
                var destination = gm.GetPositionItens();
                fruitObject.transform.position = destination;

                // Enable gravity
                var fruitRigidbody = fruitObject.GetComponent<Rigidbody>();
                if (fruitRigidbody != null)
                {
                    fruitRigidbody.useGravity = true;
                }

                gm.SetScore(itemIndex, 1);

                if(gm.CheckFinish())
                {
                    StartCoroutine(finishGame.ShowPanelEndGame(1f, playerController));
                }

                // Reset index
                UpdateIndex(0);
            }
        }
    }
    
    private void UpdateIndex(EnumSensor index) 
    {
        itemIndex = index;

        // Destroy previous object
        if(holdingObject != null) 
        {
            Destroy(holdingObject);
            holdingObject = null;
        }

        // Create new object
        GameObject newObjectPrefab = GameManager.instance.itemObjects[(int) index];
        if(newObjectPrefab != null) 
        {
            var position = transform.position;
            var rotation = newObjectPrefab.transform.rotation;
            holdingObject = Instantiate(newObjectPrefab, position, rotation);
        }
    }
}
