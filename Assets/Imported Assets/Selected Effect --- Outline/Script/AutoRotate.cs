using UnityEngine;

public class AutoRotate : MonoBehaviour
{
	public float m_Speed = 30f;

	void Update ()
	{
		float angle = Time.deltaTime * m_Speed;
		transform.Rotate (0.0f, angle, 0f);
	}
}
