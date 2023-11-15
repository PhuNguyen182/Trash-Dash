using Cysharp.Threading.Tasks;

namespace TrashDash.Scripts.Common.Interfaces
{
    public interface IPanelUI
    {
        public UniTask OnAppear();
        public UniTask Close();
        public void OnCLose();
    }
}
