using System.Collections.Generic;
using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    public float HideDistance = 10f; // Distance at which the indicator hides
    public float UpdateInterval = 1f; // Time between enemy searches

    private Transform target; // The current target
    private Transform closestEnemy; // The closest enemy found
    private bool isActive = false; // To track if the indicator should be active
    private float timeSinceLastUpdate = 0f; // Time since last enemy search

    // Distance at which the arrow is positioned relative to the player
    public float ArrowDistance = 5f;

    public Transform Target
    {
        get => target;
        set => target = value;
    }

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
            var dir = target.position - transform.position;
            if (dir.magnitude < HideDistance)
            {
                SetChildrenActive(false);
            }
            else
            {
                SetChildrenActive(true);

                // Calculate the angle to face the target
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);

                // Update the arrow position to maintain the desired distance
                Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * ArrowDistance, Mathf.Sin(angle * Mathf.Deg2Rad) * ArrowDistance, 0);
                transform.localPosition = offset;
            }
        }
        else
        {
            SetChildrenActive(false);
        }
    }

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
