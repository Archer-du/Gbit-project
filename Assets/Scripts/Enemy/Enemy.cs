using GbitProjectControl;
using GbitProjectState;
using UnityEngine;

namespace EnemyController
{
    public class Enemy : MonoBehaviour
    {
        public EnemyState enemyState;
        [SerializeField] public int enemyEnergy;
        public float attackScale;
        public float currentInterval;
        public float maxInterval;
        public GameObject player;
        public Animator animator;

        public void Damage(int damage)
        {
            enemyEnergy -= damage;
            Debug.Log(enemyEnergy);
        }
    }
}