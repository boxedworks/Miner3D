using Miner.Engine.Camera;
using Miner.Engine.Input;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;

namespace Miner.Engine.Player
{
  public struct PlayerComponent : IComponentData
  {
  }

  [UpdateInGroup(typeof(PhysicsSystemGroup))]
  [UpdateBefore(typeof(PhysicsInitializeGroup))]
  [BurstCompile]
  public partial class PlayerSystem : SystemBase
  {
    protected override void OnCreate()
    {
      RequireForUpdate<InputState>();
      RequireForUpdate<PlayerComponent>();
      RequireForUpdate<FollowCameraComponent>();
    }

    [BurstCompile]
    protected override void OnUpdate()
    {
      var inputState = SystemAPI.GetSingleton<InputState>();
      var camera = SystemAPI.GetSingleton<FollowCameraComponent>();
      var forward = math.mul(camera.Rotation, new float3(0f, 0f, 1f));
      var right = math.mul(camera.Rotation, new float3(1f, 0f, 0f));
      forward.y = 0f;
      right.y = 0f;
      forward = math.normalizesafe(forward);
      right = math.normalizesafe(right);

      Dependency = new PlayerMovementJob
      {
        Movement = right * inputState.Horizontal + forward * inputState.Vertical,
        Speed = 5f
      }.ScheduleParallel(Dependency);
    }

    [BurstCompile]
    private partial struct PlayerMovementJob : IJobEntity
    {
      public float3 Movement;
      public float Speed;

      private void Execute(ref PhysicsVelocity velocity, ref PhysicsMass mass, in PlayerComponent player)
      {
        velocity.Linear.x = Movement.x * Speed;
        velocity.Linear.z = Movement.z * Speed;

        mass.InverseInertia = float3.zero;
      }
    }
  }
}
