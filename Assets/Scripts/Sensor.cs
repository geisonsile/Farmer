using UnityEngine;

public class Sensor : MonoBehaviour
{
    public EnumSensor itemIndex;
    [SerializeField] AudioClip pickUpSFX;

    public void PlayPickUpSFX()
    {
        AudioSource.PlayClipAtPoint(pickUpSFX, Camera.main.transform.position);
    }
}
