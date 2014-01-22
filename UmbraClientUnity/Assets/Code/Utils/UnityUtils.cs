using UnityEngine;
using System;

public class UnityUtils {
    public static T LoadResource<T>(string path, bool instantiate = false) where T : UnityEngine.Object {
        try {
            T resource = (T)Resources.Load(path, typeof(T));
            return (instantiate ? (T)GameObject.Instantiate(resource) : resource);
        } catch(Exception) {
            throw new ArgumentException("Unable to load resource: " + path);
        }
    }

    public static void SetTransparency(GameObject gameObject, float transparency) {
        Color current = gameObject.renderer.material.color;
        gameObject.renderer.material.color = new Color(current.r, current.g, current.b, transparency);
    }
}
