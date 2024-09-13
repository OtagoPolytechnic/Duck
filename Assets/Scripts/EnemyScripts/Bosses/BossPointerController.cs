using UnityEngine;

public class BossPointerController : MonoBehaviour
{
    public Transform target;
    public float hideDistance;

    private void Update()
    {
        var dir = target.position - transform.position;

        if (dir.magnitude < hideDistance)
        {
           SetChildrenActive(false); 
        }
        else 
        {
          SetChildrenActive(true);

        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation =  Quaternion.AngleAxis(angle, Vector3.forward);
        
        }

      
        }
        void SetChildrenActive(bool value)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
