using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class NewBehaviourScript : Agent
{
    [SerializeField] float speed = 10;
    Rigidbody rb;
    [SerializeField] GameObject target;

    Vector3 originPoint; 

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        originPoint = transform.localPosition;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(target.transform.localPosition);
        sensor.AddObservation(rb.velocity.x);
        sensor.AddObservation(rb.velocity.z);


    }
    public override void OnActionReceived(ActionBuffers actionsOut)
    {
        Vector3 force = Vector3.zero;
        var outC = actionsOut.ContinuousActions;
        force.x = outC[0];
        force.z = outC[1];
        rb.AddForce(force, ForceMode.Impulse);

        if(rb.velocity.y < 0)
        {
            SetReward(-1);
            EndEpisode();
        }
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continousActionsOut = actionsOut.ContinuousActions;
        continousActionsOut[0] = Input.GetAxis("Horizontal");
        continousActionsOut[1] = Input.GetAxis("Vertical");

    }
}
