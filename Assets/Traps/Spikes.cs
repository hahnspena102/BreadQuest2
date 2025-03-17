using UnityEngine;


public class Spikes : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            SirGluten sirGluten = other.gameObject.GetComponent<SirGluten>();
            sirGluten.SpikeInterval = 0.1f;
        }
    }

}



