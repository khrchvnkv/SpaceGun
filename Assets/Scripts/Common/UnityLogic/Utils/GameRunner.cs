using Common.Infrastructure;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common.UnityLogic.Utils
{
    public sealed class GameRunner : MonoBehaviour
    {
        #if UNITY_EDITOR
        // Run from another scene
        private void Awake()
        {
            var gameBootstrapper = FindObjectOfType<GameBootstrapper>();
            if (gameBootstrapper is null)
            {
                SceneManager.LoadScene(Constants.Scenes.BootstrapScene);
            }
        }
        #endif
    }
}