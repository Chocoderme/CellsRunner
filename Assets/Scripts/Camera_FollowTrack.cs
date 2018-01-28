using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_FollowTrack : MonoBehaviour {

    [Header("Track")]
    public List<BezierPoint> Points = new List<BezierPoint>();

    [Header("Camera Settings")]
    public Vector3 Offset;
    public float Speed = 1f;
    public float Delay = 3f;

    public void Start()
    {
        StartCoroutine(CameraMovement());
    }

    private IEnumerator CameraMovement()
    {
        var index = 1;

        if (Points.Count > 0)
            transform.position = new Vector3(Points[0].B.x, Points[0].B.y, 0) + Offset;
        yield return new WaitForSeconds(Delay);
        while (index < Points.Count)
        {
            var currentTime = 0f;
			float dist = Vector2.Distance (Points [index - 1].B, Points [index].B);
			var maxTime = (10f * dist) / Speed;
            while (currentTime < maxTime)
            {
                currentTime += Time.deltaTime;
                if (currentTime >= maxTime)
                    currentTime = maxTime;

                var newPos = Bezier(Points[index - 1], Points[index], currentTime / maxTime);

                transform.position = new Vector3(newPos.x, newPos.y, 0) + Offset;
                yield return null;
            }
            index++;
        }

        yield return null;
    }

    private Vector2 Interpolate(Vector2 a, Vector2 b, Vector2 c, float time)
    {
        Vector2 res1 = Vector2.Lerp(a, b, time);
        Vector2 res2 = Vector2.Lerp(b, c, time);
		return Vector2.Lerp(res1, res2, time);
    }

    public Vector2 Bezier(BezierPoint A, BezierPoint B, float time)
    {
        Vector2 res1 = Interpolate(A.B, A.C, B.A, time);
        Vector2 res2 = Interpolate(A.C, B.A, B.B, time);

		return Vector2.Lerp(res1, res2, time);
    }

    public void AddPoint()
    {
        Points.Add(new BezierPoint());
    }

    public void AddPoint(Vector2 pos)
    {
        var a = new Vector2(pos.x - .5f, pos.y - .5f);
        var c = new Vector2(pos.x + .5f, pos.y + .5f);
        Points.Add(new BezierPoint(a, pos, c));
    }

    [System.Serializable]
    public class BezierPoint
    {
        public Vector2 A = Vector2.down;
        public Vector2 B = Vector2.zero;
        public Vector2 C = Vector2.up;

        public BezierPoint() { }
        public BezierPoint(Vector2 a, Vector2 b, Vector2 c)
        {
            A = a;
            B = b;
            C = c;
        }
    }
}
