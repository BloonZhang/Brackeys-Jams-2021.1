using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    // Prefabs and GameObject
    public SnowflakeController snowflakeController;

    // abbreviation variables
    private Rigidbody2D playerRB2D;
    private ParticleSystemForceField snowflakeFF;

    // definition variables
    [SerializeField] private float windForce;

    // weather variables
    private bool isWindy = false;

    // helper variables
    [SerializeField] private ParticleSystem.MinMaxCurve calmXForce;
    [SerializeField] private ParticleSystem.MinMaxCurve windyXForce;

    // Start is called before the first frame update
    void Start()
    {
        // set up variables
        playerRB2D = PlayerController.Instance.gameObject.GetComponent<Rigidbody2D>();
        snowflakeFF = snowflakeController.gameObject.GetComponent<ParticleSystemForceField>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            ToggleWind();
        }
    }

    void FixedUpdate()
    {
        if (isWindy)
        {
            // TODO: should wind affect enemies as well?
            playerRB2D.AddForce(Vector2.left * windForce);
        }
    }

    // helper methods
    private void ToggleWind()
    {
        if (!isWindy)
        {
            TurnOnWind();
        }
        else
        {
            TurnOffWind();
        }
    }
    private void TurnOnWind()
    {
        isWindy = true;
        snowflakeFF.directionX = windyXForce;
    }
    private void TurnOffWind()
    {
        isWindy = false;
        snowflakeFF.directionX = calmXForce;
    }
}
