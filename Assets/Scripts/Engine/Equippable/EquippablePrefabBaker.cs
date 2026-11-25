using Unity.Entities;
using UnityEngine;

namespace Miner.Engine.Equippable
{
  public struct EquippablePrefabComponent : IComponentData
  {
    public Entity Value;
    public EquipSlot Slot;
  }

  public class EquippablePrefabBaker : Baker<EquippablePrefabAuthoring>
  {
    public override void Bake(EquippablePrefabAuthoring authoring)
    {
      var entity = GetEntity(TransformUsageFlags.None);
      AddComponent(entity, new EquippablePrefabComponent
      {
        Value = GetEntity(authoring.PickaxePrefab, TransformUsageFlags.Dynamic),
        Slot = authoring.PixkaxeSlot
      });
    }
  }
}
