using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Miner.Engine.Harvestable
{
  public struct HarvestableComponent : IComponentData
  {
  }

  public partial class HarvestableSpawnSystem : SystemBase
  {
    protected override void OnCreate()
    {
      RequireForUpdate<HarvestablePrefabComponent>();
    }

    [BurstCompile]
    protected override void OnUpdate()
    {
      using var commands = new EntityCommandBuffer(Allocator.Temp);
      foreach (var prefabComponent in SystemAPI.Query<HarvestablePrefabComponent>())
      {
        var entity = commands.Instantiate(prefabComponent.Value);
        commands.AddComponent<HarvestableComponent>(entity);
      }

      commands.Playback(EntityManager);
      Enabled = false;
    }
  }
}
