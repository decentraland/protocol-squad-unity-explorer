using System.Runtime.InteropServices;
using UnityEngine;

namespace DCLRuntime
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TransformData
    {
        public Vector3 position;
        public Vector3 scale;
        public Quaternion rotation;
        public long parentId;

        public static TransformData FromData(byte[] bytes)
        {
            return Marshal.PtrToStructure<TransformData>(Marshal.UnsafeAddrOfPinnedArrayElement(bytes, 0));
        }

        public void ApplyOn(Transform transform)
        {
            transform.position = position;
            transform.rotation = rotation;
            transform.localScale = scale;
        }
    }
}