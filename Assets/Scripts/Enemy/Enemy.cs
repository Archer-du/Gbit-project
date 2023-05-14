using UnityEngine;

namespace EnemyController
{
    public class Enemy : MonoBehaviour
    {
        public EnemyState enemyState;
        public int enemyEnergy;
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
    public class EnemyState : Enemy
    {

        public enum State
        {
            Idle,
            Attack,
            Alert,
            Dead
        }

        private State currentState;
        public State CurrentState
        {
            get { return currentState; }
            set { currentState = value; }
        }

        public EnemyState()
        {
            currentState = State.Idle;
        }
    }
    public class BossState : Enemy
    {

        public enum State
        {
            Chase,
            BulletRain,
            Laser,
            Dash
        }

        private State currentState;

        public State CurrentState
        {
            get { return currentState; }
            set { currentState = value; }
        }

        public BossState()
        {
            currentState = State.Chase;
        }
    }
}