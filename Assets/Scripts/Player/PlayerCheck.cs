using UnityEngine;

public class PlayerCheck : MonoBehaviour {
    public int itemIndex = 0;
    public float itemOffset = 2;
    
    private GameObject holdingObject;
        
    
    void Start() {
        
    }

    void Update() {
        if(holdingObject != null) {
            var newPosition = transform.position + new Vector3(0, itemOffset, 0);
            holdingObject.transform.position = newPosition;
        }
    }

    void OnTriggerEnter(Collider other) {
        GameObject otherObject = other.gameObject;

        // Sensor
        if(otherObject.CompareTag("Sensor")) {
            var sensorScript = otherObject.GetComponent<SensorScript>();
            var index = sensorScript.itemIndex;
            UpdateIndex(index);
        }

        // Soil
        if(otherObject.CompareTag("Soil")) {
            var soilScript = otherObject.GetComponent<SoilScript>();

            // With watering can
            if(itemIndex == 3) {
                soilScript.Water();
            }

            // With seeds
            if(itemIndex == 1 || itemIndex == 2) {
                if(soilScript.IsEmpty()) {
                    soilScript.Seed(itemIndex);
                }
            }

            // With air
            if(itemIndex == 0) {
                if(soilScript.IsFinished()) {
                    var seedIndex = soilScript.seedIndex;
                    soilScript.RemoveCrop();
                    UpdateIndex(seedIndex + 3);
                }
            }
        }
    }

    void OnCollisionEnter(Collision other) {
        var otherObject = other.gameObject;

        // Sell box
        if(otherObject.CompareTag("SellBox")) {
            if(itemIndex == 4 || itemIndex == 5) {

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

    private void UpdateIndex(int index) {
        itemIndex = index;

        // Destroy previous object
        if(holdingObject != null) {
            Destroy(holdingObject);
            holdingObject = null;
        }

        // Create new object
        GameObject newObjectPrefab = GameManager.Instance.itemObjects[index];
        if(newObjectPrefab != null) {
            var position = transform.position;
            var rotation = newObjectPrefab.transform.rotation;
            holdingObject = Instantiate(newObjectPrefab, position, rotation);
        }
    }

}
