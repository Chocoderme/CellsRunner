using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain_AI_chase : Brain {
    
    [SerializeField]
    private string chaseTag = "Player";

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
        var targets = GameObject.FindGameObjectsWithTag(chaseTag);
        GameObject target = null;
        var dist = Mathf.Infinity;
        foreach (var obj in targets)
        {
            var d = Vector2.Distance(obj.transform.position, transform.position);
            if (d < dist && d > 0 && obj != this)
            {
                target = obj;
                dist = d;
            }
        }

        currentTarget = target;

        CalculateDir();
    }

    private struct RayDir
    {
        public Vector2 dir;
        public float hitDist;
        public bool priority;

        public RayDir(Vector2 d, float f, bool p = false) { dir = d; hitDist = f; priority = p; }
    }

    private void CalculateDir()
    {
        if (currentTarget != null)
        {
            var dir = currentTarget.transform.position - transform.position;
            dir = dir.normalized;

            for (int i = 0; i < Iteration; i++)
            {
                List<RayDir> dirs = RayDirs(Precision, dir);

                // FindIfPlayerIsInRay
                bool hasFoundPlayer = false;
                foreach (var ray in dirs)
                {
                    if (ray.priority)
                    {
                        dir = ray.dir.normalized;
                        hasFoundPlayer = true;
                        break;
                    }
                }
                if (hasFoundPlayer)
                    break;

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

                    var realDir = dirs[left].dir + dirs[0].dir;
                    if (rightPow > leftPow)
                        realDir = dirs[right].dir + dirs[dirs.Count - 1].dir;

                    dir = realDir.normalized;
                    if (i == Iteration - 1)
                        Debug.DrawRay(transform.position, currentDir * ViewDistance * 2, Color.cyan, Time.deltaTime);
                }
            }
            currentDir = dir.normalized;
        }
        else
            currentDir = Vector2.zero;
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
            bool hasPlayerInRay = false;
            foreach (var hit in hits)
            {
                if (hit.transform.tag == "Player" && hit.distance < ViewDistance && hit.transform.gameObject != gameObject)
                    hasPlayerInRay = true;
                if (hit.transform.tag == "World" && hit.distance < dist)
                    dist = hit.distance;
            }
            res.Add(new RayDir(actualDir, dist, hasPlayerInRay));

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
            bool hasPlayerInRay = false;
            foreach (var hit in hits)
            {
                if (hit.transform.tag == "Player" && hit.distance < ViewDistance && hit.transform.gameObject != gameObject)
                    hasPlayerInRay = true;
                if (hit.transform.tag == "World" && hit.distance < dist)
                    dist = hit.distance;
            }
            res.Add(new RayDir(actualDir, dist, hasPlayerInRay));

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
