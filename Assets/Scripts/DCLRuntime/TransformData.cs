using System.Runtime.InteropServices;
using UnityEngine;

namespace DCLRuntime
{
    [StructLayout(LayoutKind.Explicit)]
    public struct TransformData
    {
        [FieldOffset(0)] public Vector3 position;

        [FieldOffset(12)] // 3*4
        public Quaternion rotation;

        [FieldOffset(28)] // 12 + 4*4
        public Vector3 scale;

        [FieldOffset(40)] // 28 + 3*4
        public int parentId;

        public static TransformData FromData(byte[] bytes)
        {
            return Marshal.PtrToStructure<TransformData>(Marshal.UnsafeAddrOfPinnedArrayElement(bytes, 0));
        }


        public void ApplyWithParentOn(Transform transform, GameObject parent)
        {
            transform.SetParent(parent.transform);
            ApplyOn(transform);
        }

        public void ApplyOn(Transform transform)
        {
            transform.localPosition = position;
            transform.localRotation = rotation;
            transform.localScale = scale;
        }
    }
}