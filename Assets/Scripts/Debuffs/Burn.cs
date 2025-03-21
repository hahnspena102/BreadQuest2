using UnityEngine;

public class Burns : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
         
            SirGluten sirGluten = other.gameObject.GetComponent<SirGluten>();
            if (sirGluten == null) return;
            
            sirGluten.BurnTime = 2f;
        }
    }


}
