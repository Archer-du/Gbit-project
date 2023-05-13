using GbitProjectControl;
using GbitProjectState;
using UnityEngine;

using EnemyController;

public class StaticBoomCircle : StaticBoom
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        GetComponent<StaticBoom>().OnTrigger(controller);
    }
}
