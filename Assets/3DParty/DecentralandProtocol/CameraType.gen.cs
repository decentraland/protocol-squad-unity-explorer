// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: decentraland/sdk/components/common/camera_type.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace DCL.ECSComponents {

  /// <summary>Holder for reflection information generated from decentraland/sdk/components/common/camera_type.proto</summary>
  public static partial class CameraTypeReflection {

    #region Descriptor
    /// <summary>File descriptor for decentraland/sdk/components/common/camera_type.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static CameraTypeReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CjRkZWNlbnRyYWxhbmQvc2RrL2NvbXBvbmVudHMvY29tbW9uL2NhbWVyYV90",
            "eXBlLnByb3RvEiJkZWNlbnRyYWxhbmQuc2RrLmNvbXBvbmVudHMuY29tbW9u",
            "KkgKCkNhbWVyYVR5cGUSEwoPQ1RfRklSU1RfUEVSU09OEAASEwoPQ1RfVEhJ",
            "UkRfUEVSU09OEAESEAoMQ1RfQ0lORU1BVElDEAJCFKoCEURDTC5FQ1NDb21w",
            "b25lbnRzYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::DCL.ECSComponents.CameraType), }, null, null));
    }
    #endregion

  }
  #region Enums
  public enum CameraType {
    [pbr::OriginalName("CT_FIRST_PERSON")] CtFirstPerson = 0,
    [pbr::OriginalName("CT_THIRD_PERSON")] CtThirdPerson = 1,
    /// <summary>
    /// controlled by the scene
    /// </summary>
    [pbr::OriginalName("CT_CINEMATIC")] CtCinematic = 2,
  }

  #endregion

}

#endregion Designer generated code
