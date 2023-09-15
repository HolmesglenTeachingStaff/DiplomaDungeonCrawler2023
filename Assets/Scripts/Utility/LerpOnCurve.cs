using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpOnCurve : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform targetTransform; // The target position to lerp to
    public AnimationCurve movementCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f); // Customize the curve in the Inspector
    public float speed = 2.0f; // Adjust the speed in the Inspector

    private float journeyLength;
    private float startTime;

    private void Start()
    {
        if (targetTransform == null)
        {
            Debug.LogError("Target transform is not assigned!");
            enabled = false; // Disable the script if there's no target
            return;
        }

        // Calculate the length of the journey based on the positions
        journeyLength = Vector3.Distance(transform.position, targetTransform.position);
        startTime = Time.time; // Store the start time
    }

    private void Update()
    {
        float distanceCovered = (Time.time - startTime) * speed; // Calculate how far we've gone
        float fractionOfJourney = distanceCovered / journeyLength; // Calculate the journey progress

        // Ensure we stay within the curve range [0, 1]
        fractionOfJourney = Mathf.Clamp01(fractionOfJourney);

        // Use the curve to determine the eased progress
        float easedProgress = movementCurve.Evaluate(fractionOfJourney);

        // Lerp the object's position
        transform.position = Vector3.Lerp(transform.position, targetTransform.position, easedProgress);

        // If we've reached the end of the journey, stop the script
        if (fractionOfJourney >= 1.0f)
        {
            enabled = false;
        }
    }
}
