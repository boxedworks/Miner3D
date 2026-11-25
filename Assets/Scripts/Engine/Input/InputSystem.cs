using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.InputSystem;

namespace Miner.Engine.Input
{
  public struct InputState : IComponentData
  {
    public float Horizontal;
    public float Vertical;
    public float2 MouseDelta;
  }

  public partial class InputSystem : SystemBase
  {
    protected override void OnCreate()
    {
      EntityManager.CreateEntity(typeof(InputState));
      RequireForUpdate<InputState>();
    }

    protected override void OnUpdate()
    {
      var keyboard = Keyboard.current;
      var mouse = Mouse.current;
      var inputState = SystemAPI.GetSingletonRW<InputState>();

      inputState.ValueRW.Horizontal = keyboard == null
        ? 0f
        : (keyboard.dKey.isPressed ? 1f : 0f) -
          (keyboard.aKey.isPressed ? 1f : 0f);
      inputState.ValueRW.Vertical = keyboard == null
        ? 0f
        : (keyboard.wKey.isPressed ? 1f : 0f) -
          (keyboard.sKey.isPressed ? 1f : 0f);
      inputState.ValueRW.MouseDelta = mouse == null
        ? float2.zero
        : mouse.delta.ReadValue();
    }
  }
}