
using Unity.Entities;
using UnityEngine;

namespace Miner.Engine.Player
{

  public struct PlayerPrefabComponent : IComponentData
  {
    public Entity PlayerEntityPrefab;
    public Entity HandPrefab;
  }

  public class PlayerPrefabBaker : Baker<PlayerPrefabAuthoring>
  {
    public override void Bake(PlayerPrefabAuthoring authoring)
    {
      var entity = GetEntity(TransformUsageFlags.None);
      var playerPrefab = GetEntity(authoring.PlayerPrefab, TransformUsageFlags.Dynamic);
      var handPrefab = GetEntity(authoring.HandPrefab, TransformUsageFlags.Dynamic);

      AddComponent(entity, new PlayerPrefabComponent
      {
        PlayerEntityPrefab = playerPrefab,
        HandPrefab = handPrefab
      });
    }
  }

}