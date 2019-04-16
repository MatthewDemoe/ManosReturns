using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLockOn : MonoBehaviour
{
    [SerializeField]
    List<Transform> enemies;

    CameraManager cam;

    Vector2 _CAMERA_VIEW_CENTER = new Vector2(0.5f, 0.5f);

    public void SetCameraManager(CameraManager c)
    {
        cam = c;
    }

    public Transform GetInitialTarget()
    {
        // find target closest to center of view
        Camera c = cam.GetCamera();
        int closest;
        int n = enemies.Count;
        closest = 0;

        float closestDist = Vector2.Distance(_CAMERA_VIEW_CENTER, c.WorldToViewportPoint(enemies[closest].position));
        
        for (int i = 1; i < n - 1; i++)
        {
            float compareDist = Vector2.Distance(_CAMERA_VIEW_CENTER, c.WorldToViewportPoint(enemies[i].position));
            if (compareDist < closestDist)
                {
                    closest = i;
                    closestDist = compareDist;
                }
        }

        return enemies[closest];
    }

    public Transform GetClosestTarget()
    {
        int closest = 0;
        if (enemies.Count > 1)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (Vector3.Distance(enemies[i].transform.position, transform.position) <
                    Vector3.Distance(enemies[closest].transform.position, transform.position))
                {
                    closest = i;
                }
            }
        }
        else
            return enemies[0];

        return enemies[closest];
    }

    public void GetVisible()
    {
        
    }

    public void SortEnemies()
    {
        Camera c = cam.GetCamera();
        Transform temp;
        int smallest;
        int n = enemies.Count;
        for (int i = 0; i < n - 1; i++)
        {
            smallest = i;
            for (int j = i + 1; j < n; j++)
            {
                if (c.WorldToViewportPoint(enemies[j].position).x < c.WorldToViewportPoint(enemies[smallest].position).x)
                {
                    smallest = j;
                }
            }
            temp = enemies[smallest];
            enemies[smallest] = enemies[i];
            enemies[i] = temp;
        }
    }

    public Transform GetEnemyToLeft(Transform enemy)
    {
        SortEnemies();
        int index = enemies.IndexOf(enemy);
        if (index != 0)
        {
            return enemies[index - 1];
        }
        return enemies[index];
    }

    public Transform GetEnemyToRight(Transform enemy)
    {
        SortEnemies();
        int index = enemies.IndexOf(enemy);
        if (index != enemies.Count - 1)
        {
            return enemies[index + 1];
        }
        return enemies[index];
    }

    public int GetEnemyCount()
    {
        return enemies.Count;
    }

    public void RemoveEnemy(Transform s)
    {
        enemies.Remove(s);
        if (cam.IsLockedOn())
        {
            if (cam.GetLockedTarget() == s)
            {
                cam.SetLockedOn(false);
                cam.SetLockedTarget(null);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform != transform.parent)
        {
            enemies.Add(other.transform);
            other.GetComponent<TargettableBehaviour>().DesignateTargetter(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform != transform.parent)
        {
            RemoveEnemy(other.transform);
        }
    }
}
