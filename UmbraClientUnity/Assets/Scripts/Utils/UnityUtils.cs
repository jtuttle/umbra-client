using UnityEngine;

public class UnityUtils {
    public static T LoadResource<T>(string path, bool instantiate = false) where T : Object {
        T resource = (T)Resources.Load(path, typeof(T));
        return (instantiate ? (T)GameObject.Instantiate(resource) : resource);
    }
}
