using Miner.Engine.Input;
using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;

namespace Miner.Engine.Camera
{

  public struct FollowCameraComponent : IComponentData
  {
    public float3 Position;

    public float3 Offset;

    public quaternion Rotation;

    public float Pitch;
  }

  [BurstCompile]
  [UpdateInGroup(typeof(SimulationSystemGroup))]
  [UpdateBefore(typeof(TransformSystemGroup))]
  [UpdateAfter(typeof(InputSystem))]
  public partial class CameraSystem : SystemBase
  {

    UnityEngine.Camera camera;

    protected override void OnCreate()
    {
      RequireForUpdate<FollowCameraComponent>();
      RequireForUpdate<InputState>();
    }

    [BurstCompile]
    protected override void OnUpdate()
    {
      var inputState = SystemAPI.GetSingleton<InputState>();
      var job = new CameraMovementJob
      {
        MouseDelta = inputState.MouseDelta,
        Sensitivity = 0.0025f,
        MinPitch = math.radians(-89f),
        MaxPitch = math.radians(89f)
      };
      Dependency = job.ScheduleParallel(Dependency);
      Dependency.Complete();

      var cameraComponent = SystemAPI.GetSingleton<FollowCameraComponent>();
      if (camera == null)
      {
        camera = UnityEngine.Camera.main;
      }
      camera.transform.SetPositionAndRotation(
        cameraComponent.Position,
        new UnityEngine.Quaternion(
          cameraComponent.Rotation.value.x,
          cameraComponent.Rotation.value.y,
          cameraComponent.Rotation.value.z,
          cameraComponent.Rotation.value.w));
    }

    [BurstCompile]
    private partial struct CameraMovementJob : IJobEntity
    {
      public float2 MouseDelta;
      public float Sensitivity;
      public float MinPitch;
      public float MaxPitch;

      private void Execute(ref LocalTransform localTransform, ref FollowCameraComponent camera)
      {
        camera.Position = localTransform.Position + camera.Offset;
        localTransform.Rotation = math.mul(
          quaternion.RotateY(MouseDelta.x * Sensitivity),
          localTransform.Rotation);
        camera.Pitch = math.clamp(
          camera.Pitch - MouseDelta.y * Sensitivity,
          MinPitch,
          MaxPitch);
        camera.Rotation = math.mul(
          localTransform.Rotation,
          quaternion.RotateX(camera.Pitch));
      }
    }
  }

}