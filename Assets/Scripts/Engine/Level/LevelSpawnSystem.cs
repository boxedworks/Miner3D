using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Miner.Engine.Level
{
  public partial class LevelSpawnSystem : SystemBase
  {
    protected override void OnCreate()
    {
      RequireForUpdate<LevelPrefabComponent>();
    }

    [BurstCompile]
    protected override void OnUpdate()
    {
      using var commands = new EntityCommandBuffer(Allocator.Temp);
      foreach (var prefabComponent in SystemAPI.Query<LevelPrefabComponent>())
      {
        commands.Instantiate(prefabComponent.Value);
      }

      commands.Playback(EntityManager);
      Enabled = false;
    }
  }
}
