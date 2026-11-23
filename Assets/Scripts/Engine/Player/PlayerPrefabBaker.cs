
using Unity.Entities;
using UnityEngine;

namespace Miner.Engine.Player
{

  public struct PlayerPrefabComponent : IComponentData
  {
    public Entity Value;
  }

  public class PlayerPrefabBaker : Baker<PlayerPrefabAuthoring>
  {
    public override void Bake(PlayerPrefabAuthoring authoring)
    {
      var entity = GetEntity(TransformUsageFlags.None);
      AddComponent(entity, new PlayerPrefabComponent { Value = GetEntity(authoring.PlayerPrefab, TransformUsageFlags.Dynamic) });
    }
  }

}