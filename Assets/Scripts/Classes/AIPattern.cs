using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AI Pattern", menuName = "ScriptableObjects/New AI Pattern")]
public class AIPattern : ScriptableObject
{
    // enum
    public enum AIAction
    {
        Walk,
        WalkTowardsPlayer,
        Run,
        Stand,
        ShootAndStand,
        TurnAndStand,
        StandTowardsPlayer
    }

    // public variables
    //public string patternName;
    public List<AIAction> listOfActions;
    public List<float> delayAfterActions;

    // abbreviation variables
    private Rigidbody2D m_RB2D;
    private EnemyController m_EnemyController;

    // definition variables
    private float walkSpeed;
    private float runSpeed;

    // settings variables
    private float forceMultiplier = 75f;

    // helper variables
    private AIAction currentAction;
    private float timer = 0f;
    private int currentIndex = 0;

    // public methods
    public void Initialize(GameObject obj)
    {
        // Reset helper variables
        ResetAIPattern();

        // set up variables
        m_RB2D = obj.GetComponent<Rigidbody2D>();
        m_EnemyController = obj.GetComponent<EnemyController>();
        walkSpeed = m_EnemyController.m_walkSpeed;
        runSpeed = m_EnemyController.m_runSpeed;

        // Check if AIPattern is valid
        if (listOfActions.Count != delayAfterActions.Count)
        {
            Debug.Log("Error in counts for AIPattern for enemy: " + m_EnemyController.name);
        }
    }
    
    public void UpdateAIPattern(float deltaTime)
    {
        // Update timer
        timer += deltaTime;
        
        // Check if it's time to move onto the next action
        if (timer > delayAfterActions[currentIndex])
        {
            // Move onto next action
            ++currentIndex; currentIndex = currentIndex % listOfActions.Count;
            timer = 0;
            // Check if the actions has a prefire behavior
            switch (listOfActions[currentIndex])
            {
                case AIAction.TurnAndStand:
                    m_EnemyController.TurnAround();
                    break;
                case AIAction.ShootAndStand:
                    m_EnemyController.Fire();
                    break;
                default:
                    break;
            }
        }

        // See what we need to be currently doing
        switch (listOfActions[currentIndex])
        {
            case AIAction.Walk:
                m_RB2D.AddForce(Vector2.right * walkSpeed * (m_EnemyController.facingRight ? 1 : -1) * forceMultiplier);
                break;
            case AIAction.WalkTowardsPlayer:
                // check if we need to flip
                if (m_EnemyController.facingRight && m_EnemyController.transform.position.x > PlayerController.Instance.transform.position.x)
                { m_EnemyController.TurnAround(); }
                if (!m_EnemyController.facingRight && m_EnemyController.transform.position.x < PlayerController.Instance.transform.position.x)
                { m_EnemyController.TurnAround(); }
                m_RB2D.AddForce(Vector2.right * walkSpeed * (m_EnemyController.facingRight ? 1 : -1) * forceMultiplier);
                break;
            case AIAction.Run:
                m_RB2D.AddForce(Vector2.right * runSpeed * (m_EnemyController.facingRight ? 1 : -1) * forceMultiplier);
                break;
            case AIAction.StandTowardsPlayer:
                // check if we need to flip
                if (m_EnemyController.facingRight && m_EnemyController.transform.position.x > PlayerController.Instance.transform.position.x)
                { m_EnemyController.TurnAround(); }
                if (!m_EnemyController.facingRight && m_EnemyController.transform.position.x < PlayerController.Instance.transform.position.x)
                { m_EnemyController.TurnAround(); }
                break;
            default:
                break;
        }
    }

    public void ResetAIPattern()
    {
        timer = 0f;
        currentIndex = 0;
    }
    public void StartAIPattern()
    {
        // Nothing here. Maybe I'll need to add something later?
    }
    public void StopAIPattern()
    {
        ResetAIPattern();
    }

}
