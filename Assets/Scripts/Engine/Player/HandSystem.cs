using Miner.Engine.Camera;
using Miner.Engine.Input;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Miner.Engine.Player
{
  public struct HandComponent : IComponentData
  {
    public float Side;
  }

  [BurstCompile]
  [UpdateAfter(typeof(CameraSystem))]
  public partial class HandSystem : SystemBase
  {
    protected override void OnCreate()
    {
      RequireForUpdate<FollowCameraComponent>();
      RequireForUpdate<InputState>();
      RequireForUpdate<HandComponent>();
    }

    [BurstCompile]
    protected override void OnUpdate()
    {
      var camera = SystemAPI.GetSingleton<FollowCameraComponent>();
      var input = SystemAPI.GetSingleton<InputState>();
      var movementAmount = math.saturate(math.length(new float2(input.Horizontal, input.Vertical)));

      Dependency = new HandMovementJob
      {
        CameraOffset = camera.Offset,
        CameraPitch = camera.Pitch,
        MovementAmount = movementAmount,
        DeltaTime = SystemAPI.Time.DeltaTime,
        PositionSmoothing = 12f,
        Time = (float)SystemAPI.Time.ElapsedTime,
        ForwardDistance = 0.8f,
        SideDistance = 0.4f,
        VerticalOffset = -0.2f,
        ForwardSwayAmount = 0.08f,
        VerticalSwayAmount = 0.04f,
        SwayFrequency = 8f
      }.ScheduleParallel(Dependency);
    }

    [BurstCompile]
    private partial struct HandMovementJob : IJobEntity
    {
      public float3 CameraOffset;
      public float CameraPitch;
      public float MovementAmount;
      public float DeltaTime;
      public float PositionSmoothing;
      public float Time;
      public float ForwardDistance;
      public float SideDistance;
      public float VerticalOffset;
      public float ForwardSwayAmount;
      public float VerticalSwayAmount;
      public float SwayFrequency;

      private void Execute(ref LocalTransform transform, in HandComponent hand)
      {
        var cameraRotation = quaternion.RotateX(CameraPitch);
        var forward = math.mul(cameraRotation, new float3(0f, 0f, 1f));
        var right = math.mul(cameraRotation, new float3(1f, 0f, 0f));
        var up = math.mul(cameraRotation, new float3(0f, 1f, 0f));
        var sway = math.sin(Time * SwayFrequency) * MovementAmount;
        var forwardSway = sway * ForwardSwayAmount * hand.Side;
        var verticalSway = math.cos(Time * SwayFrequency) *
          VerticalSwayAmount *
          MovementAmount *
          hand.Side;
        var localOffset = new float3(0f, 0f, -0.25f);
        var targetPosition = CameraOffset +
          forward * (ForwardDistance + forwardSway) +
          right * (hand.Side * SideDistance) +
          up * (VerticalOffset + verticalSway) +
          math.mul(cameraRotation, localOffset);
        var smoothing = 1f - math.exp(-PositionSmoothing * DeltaTime);

        transform.Position = math.lerp(transform.Position, targetPosition, smoothing);
        transform.Rotation = cameraRotation;
      }
    }
  }
}
