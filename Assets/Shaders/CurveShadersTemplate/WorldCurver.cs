using UnityEngine;

[ExecuteInEditMode]
public class WorldCurver : MonoBehaviour
{
	[Range(-0.006f, 0.006f)]
	public float curveStrength = 0.06f;

	[Header("True/False Or 1/0")]
	[Range(0, 1)] public int isCurveX;
	[Range(0, 1)] public int isCurveY;
    int m_CurveStrengthID;
    int m_CurveXID;
    int m_CurveYID;

    [Header("Curve Stuff")] 
    [SerializeField] private float durationCurveChange;
    private float curveChangeSpeed = 10f;
    [Range(0, 1)] private int direct = 1;

    private void OnEnable()
    {
	    durationCurveChange = 10f;
        m_CurveStrengthID = Shader.PropertyToID("_CurveStrength");
        m_CurveXID = Shader.PropertyToID("_Curve_X");
        m_CurveYID = Shader.PropertyToID("_Curve_Y");
    }

	void Update()
	{
		// if (durationCurveChange > 0 && Mathf.Abs(curveStrength) >= 0.06f)
		// {
		// 	durationCurveChange -= Time.deltaTime;
		// }
		// else
		// {
		// 	if (direct == 1)
		// 	{
		// 		curveStrength += curveChangeSpeed * Time.deltaTime;
		// 	}
		// 	else
		// 	{
		// 		curveStrength -= curveChangeSpeed * Time.deltaTime;
		// 	}
		//
		// 	if (Mathf.Abs(curveStrength) >= 0.06f)
		// 	{
		// 		direct = direct == 1 ? 0 : 1;
		// 		durationCurveChange = 10f;
		// 	}
		// }
		
		Shader.SetGlobalFloat(m_CurveStrengthID, curveStrength);
		Shader.SetGlobalInt(m_CurveXID, isCurveX);
		Shader.SetGlobalInt(m_CurveYID, isCurveY);
	}
}
