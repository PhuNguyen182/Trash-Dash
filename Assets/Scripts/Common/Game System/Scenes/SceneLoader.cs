using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace TrashDash.Scripts.Common.GameSystem.Scenes
{
    public static class SceneLoader
    {
        public const string LOADING = "Loading";
        public const string MAINHOME = "Mainhome";
        public const string GAMEPLAY = "Gameplay";

        public static async UniTask LoadScene(string sceneName, IProgress<float> progress, LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            AsyncOperation loadOperator = SceneManager.LoadSceneAsync(sceneName, loadMode);
            await loadOperator.ToUniTask(progress);
        }
    }
}
