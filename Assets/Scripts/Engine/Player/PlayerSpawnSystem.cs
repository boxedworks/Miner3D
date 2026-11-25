
using Unity.Entities;
using Unity.Burst;
using Unity.Collections;
using Miner.Engine.Camera;
using Unity.Mathematics;
using Unity.Transforms;

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
        var entity = commands.Instantiate(prefabComponent.PlayerEntityPrefab);

        commands.AddComponent<PlayerComponent>(entity);
        commands.AddComponent(entity, new FollowCameraComponent
        {
          Offset = new float3(0, 1, 0),
          Rotation = quaternion.identity
        });

        var leftHand = commands.Instantiate(prefabComponent.HandPrefab);
        commands.AddComponent(leftHand, new HandComponent { Side = -1f });
        commands.AddComponent(leftHand, new Parent { Value = entity });

        var rightHand = commands.Instantiate(prefabComponent.HandPrefab);
        commands.AddComponent(rightHand, new HandComponent { Side = 1f });
        commands.AddComponent(rightHand, new Parent { Value = entity });
      }

      commands.Playback(EntityManager);
      Enabled = false;
    }
  }
}