using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    Vector3[] path;
    int targetIndex;

   float speed = 2;

	private void Start ()
	{
		UnitManager.Instance.AddUnitToAllUnits (gameObject);
	}
	
	void Update ()
	{
	
	}
	
	
	public void Move (Vector3 point)
    {
        PathRequestManager.RequestPath(transform.position, point, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
        if (pathSuccessful)
        {
                path = newPath;
                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        while (true)
        {
                if (transform.position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        targetIndex = 0;
                        path = new Vector3[0];
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }

                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                yield return null;

        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
          {
                for (int i = targetIndex; i < path.Length; i++)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(path[i], Vector3.one);

                    if (i == targetIndex)
                    {
                        Gizmos.DrawLine(transform.position, path[i]);
                    }
                    else {
                        Gizmos.DrawLine(path[i - 1], path[i]);
                    }
                }
        }
    }
}
