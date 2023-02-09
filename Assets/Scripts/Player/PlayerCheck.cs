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
        if(otherObject.CompareTag("SellBox")) 
        {
            if(itemIndex == EnumSensor.BEET_FRUIT || itemIndex == EnumSensor.PUMPKIN_FRUIT)
            {

                var gm = GameManager.instance;

                // Teleport fruit
                var fruitObject = holdingObject;
                holdingObject = null;
                var offset = new Vector3(
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, 1f)
                );
                var destination = gm.positionItensTransform.position + offset;
                fruitObject.transform.position = destination;

                // Enable gravity
                var fruitRigidbody = fruitObject.GetComponent<Rigidbody>();
                if (fruitRigidbody != null)
                {
                    fruitRigidbody.useGravity = true;
                }

                SetPoints(gm);

                finishGame.CheckFinish(playerController);

                // Reset index
                UpdateIndex(0);
            }
        }
    }

    private void SetPoints(GameManager gm)
    {
        if (itemIndex == EnumSensor.BEET_FRUIT)
        {
            gm.qtdBeets++;
            gm.txtBeetsQtd.text = string.Format("{0}/{1}", gm.qtdBeets.ToString(), gm.totalItens);
        }
        else if (itemIndex == EnumSensor.PUMPKIN_FRUIT)
        {
            gm.qtdPumpkins++;
            gm.txtPumpkinsQtd.text = string.Format("{0}/{1}", gm.qtdPumpkins.ToString(), gm.totalItens);
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
