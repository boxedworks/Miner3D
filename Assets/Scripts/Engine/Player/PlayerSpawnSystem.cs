
using Unity.Entities;
using Unity.Burst;
using Unity.Collections;

namespace Miner.Engine.Player
{
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
        commands.Instantiate(prefabComponent.Value);
      }

      commands.Playback(EntityManager);
      Enabled = false;
    }
  }
}