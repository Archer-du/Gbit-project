using UnityEngine;

public class StaticBoomCircle : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            GetComponentInParent<StaticBoom>().OnTrigger();
        }
        //TODO:boss
    }
}
