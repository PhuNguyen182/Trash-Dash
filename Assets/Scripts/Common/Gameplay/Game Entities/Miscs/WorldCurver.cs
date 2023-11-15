using UnityEngine;

namespace TrashDash.Scripts.Common.Gameplay.GameEntities.Miscs
{
	[ExecuteInEditMode]
	public class WorldCurver : MonoBehaviour
	{
		[Range(-0.1f, 0.1f)]
		public float curveStrength = 0.01f;
        [Range(-0.1f, 0.1f)]
        public float curveStrengthX = 0.01f;

        int m_CurveStrengthID, m_CurveXStrengthID;

		private void OnEnable()
		{
			m_CurveStrengthID = Shader.PropertyToID("_CurveStrength");
			m_CurveXStrengthID = Shader.PropertyToID("_CurveStrengthX");
        }

		private void Update()
		{
			Shader.SetGlobalFloat(m_CurveStrengthID, curveStrength);
            Shader.SetGlobalFloat(m_CurveXStrengthID, curveStrengthX);
        }
	}
}
