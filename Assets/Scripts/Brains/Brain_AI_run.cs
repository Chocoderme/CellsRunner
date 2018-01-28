using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain_AI_run : Brain {

    [SerializeField]
    private string playerTag = "Player";

    [SerializeField]
    private float ViewDistance = 4f;

    [SerializeField]
    private int Precision = 8;
    [SerializeField]
    private int Iteration = 3;

    private GameObject currentTarget = null;

    private Vector2 currentDir = Vector2.zero;

    public override void Think()
    {
        var targets = GameObject.FindGameObjectsWithTag(playerTag);
        GameObject target = null;
        foreach (var obj in targets)
        {
            if (obj.GetComponent<LifePlayer>() != null && obj != this) //&& obj.GetComponent<LifePlayer>().getContaminated())
            {
                target = obj;
                break;
            }
        }

        currentTarget = target;

        CalculateDir();
    }

    private struct RayDir
    {
        public Vector2 dir;
        public float hitDist;

        public RayDir(Vector2 d, float f) { dir = d; hitDist = f; }
    }

    private void CalculateDir()
    {
        if (currentTarget != null)
        {
            var dir = currentTarget.transform.position - transform.position;
            var dirTarget = currentTarget.transform.position - transform.position;
            dir = dir.normalized;
            dir = new Vector2(-dir.x, -dir.y);

            Debug.DrawRay(transform.position, dir, Color.yellow, Time.deltaTime);

            for (int i = 0; i < Iteration; i++)
            {
                List<RayDir> dirs = RayDirs(Precision, dir);

                var middle = dirs.Count / 2;
                // Front is clear
                if (dirs[middle].hitDist == Mathf.Infinity)
                {
                    var left = middle;
                    var right = middle;
                    while (left > 0 && dirs[left].hitDist == Mathf.Infinity)
                        left--;
                    while (right < dirs.Count - 2 && dirs[right].hitDist == Mathf.Infinity)
                        right++;

                    var realDir = dirs[left].dir + dirs[right].dir;

                    dir = realDir.normalized;
                    if (i == Iteration - 1)
                        Debug.DrawRay(transform.position, currentDir * ViewDistance * 2, Color.cyan, Time.deltaTime);
                }
                // Front is not clear
                else
                {
                    var left = 0;
                    var right = dirs.Count - 1;

                    while (left < dirs.Count - 2 && dirs[left].hitDist != Mathf.Infinity)
                        left++;
                    while (left < dirs.Count - 2 && dirs[left].hitDist == Mathf.Infinity)
                        left++;
                    while (right > 0 && dirs[right].hitDist != Mathf.Infinity)
                        right--;
                    while (right > 0 && dirs[right].hitDist == Mathf.Infinity)
                        right--;

                    var leftPow = left;
                    var rightPow = (dirs.Count - 1) - right;

                    var realDirLeft = dirs[left].dir + dirs[0].dir;
                    var realDirRight = dirs[right].dir + dirs[dirs.Count - 1].dir;

                    var leftPowAngle = Vector2.Angle(dirTarget, realDirLeft);
                    var RightPowAngle = Vector2.Angle(dirTarget, realDirRight);

                    var realDir = realDirLeft;
                    if (RightPowAngle > leftPowAngle)
                        realDir = realDirRight;

                    dir = realDir.normalized;
                    if (i == Iteration - 1)
                        Debug.DrawRay(transform.position, currentDir * ViewDistance * 2, Color.cyan, Time.deltaTime);
                }
            }
            currentDir = dir.normalized;

            var providedDest = transform.position + new Vector3(currentDir.x, currentDir.y, 0);
            var camPoint = Camera.main.WorldToViewportPoint(providedDest);
            if (camPoint.x < 0f || 1f < camPoint.x || camPoint.y < 0f || 1f < camPoint.y)
                currentDir = Vector2.zero;
        }
        else
        {
            currentDir = Vector2.zero;
        }
    }

    private List<RayDir> RayDirs(int precision, Vector2 dir)
    {
        List<RayDir> res = new List<RayDir>();
        float incAngle = 90f / precision;

        for (int i = precision; i >= 0; i--)
        {
            var actualDir = Vector2Extensions.Rotate(dir, incAngle * i);
            float dist = Mathf.Infinity;
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, actualDir, ViewDistance);
            foreach (var hit in hits)
            {
                var actualDistView = actualDir.normalized * ViewDistance;
                var camPoint = Camera.main.WorldToViewportPoint(transform.position + new Vector3(actualDistView.x, actualDistView.y, 0));
                if (hit.transform.tag == "World" && hit.distance < dist)
                    dist = hit.distance;
                else if (camPoint.x < 0f || 1f < camPoint.x || camPoint.y < 0f || 1f < camPoint.y)
                    dist = ViewDistance * 2;
            }
            res.Add(new RayDir(actualDir, dist));

            if (dist != Mathf.Infinity)
            {
                Debug.DrawRay(transform.position, actualDir * ViewDistance, Color.red, Time.deltaTime);
            }
            else
            {
                Debug.DrawRay(transform.position, actualDir * ViewDistance, Color.green, Time.deltaTime);
            }
        }

        for (int i = 1; i <= precision; i++)
        {
            var actualDir = Vector2Extensions.Rotate(dir, -incAngle * i);
            float dist = Mathf.Infinity;
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, actualDir, ViewDistance);
            foreach (var hit in hits)
            {
                var actualDistView = actualDir.normalized * ViewDistance;
                var camPoint = Camera.main.WorldToViewportPoint(transform.position + new Vector3(actualDistView.x, actualDistView.y, 0));
                if (hit.transform.tag == "World" && hit.distance < dist)
                    dist = hit.distance;
                else if (camPoint.x < 0f || 1f < camPoint.x || camPoint.y < 0f || 1f < camPoint.y)
                    dist = ViewDistance * 2;
            }
            res.Add(new RayDir(actualDir, dist));

            if (dist != Mathf.Infinity)
            {
                Debug.DrawRay(transform.position, actualDir * ViewDistance, Color.red, Time.deltaTime);
            }
            else
            {
                Debug.DrawRay(transform.position, actualDir * ViewDistance, Color.green, Time.deltaTime);
            }
        }

        return res;
    }

    public override void Act(Movement mv)
    {
        if (currentTarget != null)
        {

            mv.setDirection(currentDir);
            mv.Move();
        }
    }
}
