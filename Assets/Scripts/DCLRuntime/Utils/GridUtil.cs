using UnityEngine;

namespace DCLRuntime.Utils
{
    public static class GridUtil
    {
        private const float PARCEL_SIZE_METERS = 16f;
        
        public static Vector3 ToWorldPosition(this Vector2Int pos)
        {
            return new Vector3(pos.x * PARCEL_SIZE_METERS, 0, pos.y * PARCEL_SIZE_METERS);
        }
    }
}