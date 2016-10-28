using UnityEngine;
using System.Collections;
using Teario.Util;

namespace Teario.Halloween
{
	public class Enemy : PoolableObject
	{
		[SerializeField]
		private float m_WalkSpeed;
		[SerializeField]
		private int m_MaxHealth;
        [SerializeField]
        private ParticleSystem m_SpawnParticleSystem;
        [SerializeField]
        private ParticleSystem m_DespawnParticleSystem;
		
		private int m_Health;
		private EventRouter m_EventRouter;
        private EnemyStateController m_StateController;
        private bool m_IsInitialised = false;
        private AudioSource m_AudioSource;
	
		void Start()
		{
            Init();
		}

        private void Init()
        {
            if( !m_IsInitialised )
            {
                ObjectLocator lLocator = FindObjectOfType<ObjectLocator>();
                Debug.Assert( lLocator != null );
                if( lLocator != null )
                {
                    m_EventRouter = ObjectLocator.FindObjectOfType<EventRouter>();
                    m_EventRouter.RegisterListener( "game_reset", OnGameReset );
                    Debug.Assert( m_EventRouter != null );
                }
    
                m_StateController = GetComponentInChildren<EnemyStateController>();
                Debug.Assert( m_StateController != null, "Failed to find enemy state controller" );

                AudioSource lAudioSource = GetComponent<AudioSource>();
                Debug.Assert( lAudioSource != null );

                NavMeshAgent lNavAgent = GetComponent<NavMeshAgent>();
                Debug.Assert( lNavAgent != null, "Failed to find Navigation Agent" );
                if( lNavAgent != null )
                {
                    lNavAgent.speed = m_WalkSpeed;

                    EnemyStateSeekPlayer lSeekBehaviour = (EnemyStateSeekPlayer)m_StateController.FetchState( typeof(EnemyStateSeekPlayer) );
                    Debug.Assert( lSeekBehaviour != null );
                    lSeekBehaviour.SetNavigationAgent( lNavAgent );
                    lSeekBehaviour.SetMoveSpeed( m_WalkSpeed );
                    lSeekBehaviour.SetAudioSource( lAudioSource );
                }

                Renderer lRenderer = GetComponentInChildren<Renderer>();
                Debug.Assert( lRenderer != null && lRenderer.material != null );

                EnemyStateIdle lIdleBehaviour = (EnemyStateIdle)m_StateController.FetchState( typeof(EnemyStateIdle) );
                Debug.Assert( lIdleBehaviour != null );
                lIdleBehaviour.SetAudioSource( lAudioSource );

                EnemyStateDie lDeathBehaviour = (EnemyStateDie)m_StateController.FetchState( typeof(EnemyStateDie) );
                Debug.Assert( lDeathBehaviour != null );
                lDeathBehaviour.RegisterCompletionListener( Despawn );
                lDeathBehaviour.SetDespawnParticleSystem( m_DespawnParticleSystem );
                lDeathBehaviour.SetMaterial( lRenderer.material );
                lDeathBehaviour.SetAudioSource( lAudioSource );

                EnemyStateHurt lHurtState = (EnemyStateHurt)m_StateController.FetchState( typeof(EnemyStateHurt) );
                Debug.Assert( lHurtState != null );
                lHurtState.SetAudioSource( lAudioSource );

                EnemyStateAttackPlayer lAttackState = (EnemyStateAttackPlayer)m_StateController.FetchState( typeof(EnemyStateAttackPlayer) );
                Debug.Assert( lAttackState != null );
                lAttackState.SetAudioSource( lAudioSource );

                EnemyStateSpawn lSpawnBehaviour = (EnemyStateSpawn)m_StateController.FetchState( typeof(EnemyStateSpawn) );
                Debug.Assert( lSpawnBehaviour != null );
                lSpawnBehaviour.SetSpawnParticleSystem( m_SpawnParticleSystem );

                NavMeshObstacle lNavObstacle = GetComponent<NavMeshObstacle>();
                Debug.Assert( lNavObstacle != null, "Failed to find Navigation Obstacle" );
                if( lNavObstacle != null )
                {
                    EnemyStateAttackPlayer lAttackBehaviour = (EnemyStateAttackPlayer)m_StateController.FetchState( typeof(EnemyStateAttackPlayer) );
                    Debug.Assert( lAttackBehaviour != null );

                    lAttackBehaviour.SetNavigationObstacle( lNavObstacle );

                    lDeathBehaviour.SetNavigationObstacle( lNavObstacle );
                }

                m_AudioSource = GetComponent<AudioSource>();
                Debug.Assert( m_AudioSource != null );

                m_IsInitialised = true;
            }
        }
		
        public void Spawn( Vector3 lPosition )
		{
            Spawn( lPosition, typeof( EnemyStateSpawn ) );
		}

        public void Spawn( Vector3 lPosition, System.Type lDefaultState )
        {
            Init();

            transform.position = lPosition;
            m_Health = m_MaxHealth;
            m_StateController.SetState( lDefaultState );
        }

        private void OnStateFinished( EnemyBaseState lState )
        {
        }
	
		public void TakeHit()
		{
			if( m_Health > 0 )
            {
                if( (--m_Health) <= 0 )
    			{
                    m_EventRouter.TriggerEvent( "enemy_destroyed" );
                    m_StateController.SetState( typeof(EnemyStateDie) );
    			}
                else
                {
                    m_StateController.SetState( typeof(EnemyStateHurt) );
                }
            }
		}

        private void Despawn( System.Type lNextState )
        {
            m_EventRouter.TriggerEvent( "enemy_despawn" );
            ReturnToPool();
        }

        private void OnGameReset()
        {
            ReturnToPool();
        }
	}
}