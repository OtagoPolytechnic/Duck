using System.Collections.Generic;
using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    public float UpdateInterval = 1f; // Time between enemy searches
    public Camera mainCamera; // Reference to the main camera

    private Transform target; // The current target
    private Transform closestEnemy; // The closest enemy found
    private bool isActive = false; // To track if the indicator should be active
    private float timeSinceLastUpdate = 0f; // Time since last enemy search

    // Distance at which the arrow is positioned relative to the player
    public float ArrowDistance = 5f;

    // Property to get or set the target Transform.
    public Transform Target
    {
        get => target;
        set => target = value;
    }

    // Updates the indicator each frame. Manages target visibility, updates position and rotation of the indicator arrow.
    void Update()
    {
        if (!isActive)
        {
            SetChildrenActive(false);
            return;
        }

        timeSinceLastUpdate += Time.deltaTime;
        if (timeSinceLastUpdate >= UpdateInterval)
        {
            FindClosestEnemy();
            timeSinceLastUpdate = 0f;
        }

        if (target != null)
        {
            // Convert the target position to viewport space
            Vector3 viewportPosition = mainCamera.WorldToViewportPoint(target.position);

            // Check if the target is within the camera view
            bool isInScreen = viewportPosition.z > 0 && viewportPosition.x >= 0 && viewportPosition.x <= 1 && viewportPosition.y >= 0 && viewportPosition.y <= 1;

            if (isInScreen)
            {
                SetChildrenActive(false);
            }
            else
            {
                SetChildrenActive(true);

                // Calculate the direction to the target and the angle
                Vector3 dir = target.position - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                // Update the arrow position to maintain the desired distance
                Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * ArrowDistance, Mathf.Sin(angle * Mathf.Deg2Rad) * ArrowDistance, 0);
                transform.localPosition = offset;

                // Set the rotation to face the target
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
        else
        {
            SetChildrenActive(false);
        }
    }

    // Finds the closest enemy in the scene and sets it as the target.
    void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
        {
            target = null;
            return;
        }

        float minDistance = float.MaxValue;
        closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        target = closestEnemy;
    }

    // Activates or deactivates all child GameObjects of the indicator.
    void SetChildrenActive(bool value)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(value);
        }
    }

    public void ActivateIndicator()
    {
        isActive = true;
    }

    public void DeactivateIndicator()
    {
        isActive = false;
        SetChildrenActive(false);
    }
}
