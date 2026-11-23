using UnityEngine;

using Miner.Engine;

namespace Miner.Game
{

  public class GameController : MonoBehaviour
  {

    EngineController engineController;

    void Start()
    {
      engineController = new();
    }

  }

}