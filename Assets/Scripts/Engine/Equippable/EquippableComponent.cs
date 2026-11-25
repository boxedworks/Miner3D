
using Unity.Entities;

namespace Miner.Engine.Equippable
{

  public struct EquippableComponent : IComponentData
  {
    public EquipSlot Slot;
  }

}