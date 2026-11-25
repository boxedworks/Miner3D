using Unity.Entities;
using UnityEngine;

namespace Miner.Engine.Level
{
  public struct LevelPrefabComponent : IComponentData
  {
    public Entity Value;
  }

  public class LevelPrefabBaker : Baker<LevelPrefabAuthoring>
  {
    public override void Bake(LevelPrefabAuthoring authoring)
    {
      var entity = GetEntity(TransformUsageFlags.None);
      AddComponent(entity, new LevelPrefabComponent
      {
        Value = GetEntity(authoring.LevelPrefab, TransformUsageFlags.Dynamic)
      });
    }
  }
}
