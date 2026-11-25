
using Unity.Entities;
using Unity.Burst;
using Unity.Collections;
using Unity.Physics;
using Miner.Engine.Camera;
using Unity.Mathematics;

namespace Miner.Engine.Player
{
  [BurstCompile]
  public partial class PlayerSpawnSystem : SystemBase
  {
    protected override void OnCreate()
    {
      RequireForUpdate<PlayerPrefabComponent>();
    }

    [BurstCompile]
    protected override void OnUpdate()
    {
      using var commands = new EntityCommandBuffer(Allocator.Temp);
      foreach (var prefabComponent in SystemAPI.Query<PlayerPrefabComponent>())
      {
        var entity = commands.Instantiate(prefabComponent.Value);

        commands.AddComponent<PlayerComponent>(entity);
        commands.AddComponent(entity, new FollowCameraComponent
        {
          Offset = new float3(0, 1, 0),
          Rotation = quaternion.identity
        });
      }

      commands.Playback(EntityManager);
      Enabled = false;
    }
  }
}