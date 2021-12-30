using Godot;

public static class MathHelpers
{
	public static float ClampAngle(float angle, bool useDegrees = false)
	{
		var modulo = useDegrees ? 360 : Mathf.Tau;
		return angle % modulo;
	}

	public static float CenterAngle(float angle, bool useDegrees = false)
	{
		var offset = useDegrees ? 180 : Mathf.Pi;
		angle = ClampAngle(angle, useDegrees);
		if (angle > offset) angle = -offset + angle;
		else if (angle < -offset) angle = offset + (angle - offset);
		return angle;
	}
}