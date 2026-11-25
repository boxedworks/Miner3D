using Unity.Entities;
using UnityEngine;

namespace Miner.Engine.Harvestable
{
  public struct HarvestablePrefabComponent : IComponentData
  {
    public Entity Value;
  }

  public class HarvestablePrefabBaker : Baker<HarvestablePrefabAuthoring>
  {
    public override void Bake(HarvestablePrefabAuthoring authoring)
    {
      var entity = GetEntity(TransformUsageFlags.None);
      AddComponent(entity, new HarvestablePrefabComponent
      {
        Value = GetEntity(authoring.HarvestablePrefab, TransformUsageFlags.Dynamic)
      });
    }
  }
}
