using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace TrashDash.Scripts.Common.GameSystem.Scenes
{
    public class LoadingScene : MonoBehaviour, IProgress<float>
    {
        private CancellationToken _cancellationToken;

        public void Report(float value)
        {
            
        }

        private async UniTask Start()
        {
            _cancellationToken = this.GetCancellationTokenOnDestroy();
            await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: _cancellationToken);
            await SceneLoader.LoadScene(SceneLoader.MAINHOME, this);
        }
    }
}
