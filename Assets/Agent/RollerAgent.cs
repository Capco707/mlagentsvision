using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.XR;

public class RollerAgent : Agent
{
    public Transform Cube;
    public float move_speed = 10;

    private Rigidbody rBody;

    // Start is called before the first frame update
    private void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// agent reset
    /// </summary>
    public override void OnEpisodeBegin()
    {
        if (this.transform.position.y < 0)
        {
            rBody.velocity = Vector3.zero;
            rBody.angularVelocity = Vector3.zero;
            transform.position = new Vector3(0, 0.5f, 0);
        }

        Cube.position = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Cube.position);
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actions.ContinuousActions[0];
        controlSignal.z = actions.ContinuousActions[1];

        rBody.AddForce(controlSignal * move_speed);

        float distanceToTarget = Vector3.Distance(transform.position, Cube.position);

        if (distanceToTarget < 1.42f)
        {
            SetReward(1);
            EndEpisode();
        }

        if (transform.position.y < 0)
        {
            EndEpisode();
        }
    }

    // public override void Heuristic(in ActionBuffers actionsOut)
    // {
    //     var action = actionsOut.ContinuousActions;
    //     action[0] = Input.GetAxis(("Horizontal"));
    //     action[1] = Input.GetAxis(("Vertical"));
    // }
}
