using UnityEngine;
using System.Collections;

public class MapOverlay : MonoBehaviour {
    private Dungeon _map;

    private GameObject _backdrop;

    public virtual void Awake() {
        Vector3 camPos = GameManager.Instance.GameCamera.transform.position;
        transform.position = new Vector3(camPos.x, camPos.y, transform.position.z);
    }

    public void Dispose() {
        _map = null;
    }

    public void ShowMap(Dungeon map) {
        _map = map;
    }
}
