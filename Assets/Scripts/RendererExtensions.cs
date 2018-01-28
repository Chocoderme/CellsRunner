using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RendererExtensions {

	public static bool IsVisibleFromCamera(this Renderer renderer)
	{
		GameObject camera = GameObject.Find ("Main Camera");
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes (camera.GetComponent<Camera>());
		return GeometryUtility.TestPlanesAABB (planes, renderer.bounds);
	}

}
