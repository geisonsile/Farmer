using UnityEngine;

public class PlayerCheck : MonoBehaviour 
{
    public EnumSensor itemIndex = EnumSensor.EMPTY;
    public float itemOffset = 3;
    
    private GameObject holdingObject;

    
    void Start() 
    {
        
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
                    sensor.PlayPickUpSFX();
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
        if(otherObject.CompareTag("SellBox")) 
        {
            if(itemIndex == EnumSensor.BEET_FRUIT || itemIndex == EnumSensor.PUMPKIN_FRUIT) 
            {

                var gm = GameManager.Instance;

                // Teleport fruit
                var fruitObject = holdingObject;
                holdingObject = null;
                var offset = new Vector3(
                    Random.Range(-1f,1f),
                    Random.Range(-1f,1f),
                    Random.Range(-1f,1f)
                );
                var destination = gm.depositBoxTransform.position + offset;
                fruitObject.transform.position = destination;

                // Enable gravity
                var fruitRigidbody = fruitObject.GetComponent<Rigidbody>();
                if(fruitRigidbody != null) {
                    fruitRigidbody.useGravity = true;
                }

                // Reset index
                UpdateIndex(0);

                // Give money
                gm.coins += gm.coinsPerFruit;
                Debug.Log("Player coins: " + gm.coins);
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
        GameObject newObjectPrefab = GameManager.Instance.itemObjects[(int) index];
        if(newObjectPrefab != null) 
        {
            var position = transform.position;
            var rotation = newObjectPrefab.transform.rotation;
            holdingObject = Instantiate(newObjectPrefab, position, rotation);
        }
    }

}
