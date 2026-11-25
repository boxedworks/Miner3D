
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
      var playerPrefab = GetEntity(authoring.PlayerPrefab, TransformUsageFlags.Dynamic);

      AddComponent(entity, new PlayerPrefabComponent { Value = playerPrefab });
    }
  }

}