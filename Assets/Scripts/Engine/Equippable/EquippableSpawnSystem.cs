using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Miner.Engine.Equippable
{

  [BurstCompile]
  public partial class EquippableSpawnSystem : SystemBase
  {
    protected override void OnCreate()
    {
      RequireForUpdate<EquippablePrefabComponent>();
      RequireForUpdate<Player.HandComponent>();
    }

    [BurstCompile]
    protected override void OnUpdate()
    {
      var rightHand = Entity.Null;
      var firstHand = Entity.Null;
      foreach (var (hand, entity) in SystemAPI
        .Query<RefRO<Player.HandComponent>>()
        .WithEntityAccess())
      {
        firstHand = firstHand == Entity.Null ? entity : firstHand;
        if (hand.ValueRO.Side > 0f)
        {
          rightHand = entity;
        }
      }

      var handEntity = rightHand != Entity.Null ? rightHand : firstHand;
      using var commands = new EntityCommandBuffer(Allocator.Temp);
      foreach (var prefabComponent in SystemAPI.Query<EquippablePrefabComponent>())
      {
        var entity = commands.Instantiate(prefabComponent.Value);
        commands.AddComponent(entity, new EquippableComponent
        {
          Slot = prefabComponent.Slot
        });

        if (prefabComponent.Slot == EquipSlot.HAND)
        {
          commands.AddComponent(entity, new Parent { Value = handEntity });
        }
      }

      commands.Playback(EntityManager);
      Enabled = false;
    }
  }
}
