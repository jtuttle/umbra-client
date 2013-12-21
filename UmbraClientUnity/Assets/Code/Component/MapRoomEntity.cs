using UnityEngine;
using System.Collections;

public class MapRoomEntity : MonoBehaviour {
    public XY Coord { get; private set; }
    public Rect RoomBounds { get; private set; }

    public void Initialize(XY coord, Rect roomBounds) {
        Coord = coord;
        RoomBounds = roomBounds;
    }
}
