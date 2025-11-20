using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Trajectories. Algunos algoritmos para trajectorias. Bezier, velocidad uniformemente acelerada, tiro parabolico, etc.
/// </summary>
public class Trajectories 
{
   public struct ParabolicBezierControlPoints
    {
        public Vector3 controlPointA;
        public Vector3 controlAToA;
        public Vector3 controlAToApex;
        public Vector3 controlPointB;
        public Vector3 controlBToApex;
        public Vector3 controlBToB;
    }

    public static bool IsOpositeDirection(Vector3 v1, Vector3 v2)
    {
        float v = Vector3.Dot(v1, v2);
        return v < 0;
    }
    public static Vector2 Bezier(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float time, float timeMax)
	{
		
		//float step = time / timeMax;
		float t = time / timeMax;

		float _1_t = 1 - t;
		float _1_t_exp2 = _1_t*_1_t;
		float _1_t_exp3 = _1_t_exp2 * _1_t;
		float t_exp2 = t * t;
		float t_exp3 = t_exp2 * t;
		Vector2 point = _1_t_exp3 * p0 + 3* _1_t_exp2 * t * p1 + 3 * _1_t * t_exp2 * p2 + t_exp3 * p3;
		return point;
	}

	public static Vector2 Bezier(Vector2 p0, Vector2 p1, Vector2 p2, float time, float TimeMax)
	{
		float t = time / TimeMax;

		float _1_t = 1 - t;
		float _1_t_exp2 = _1_t*_1_t;
		float t_exp2 = t * t;
		Vector2 point = _1_t_exp2 * p0 + 2 * _1_t * t * p1 +  t_exp2 * p2 ;
		return point;
	}

    /// <summary>
    /// Metodo para calcular una bezier dado punto inicial, punto de control y punto final
    /// </summary>
    /// <param name="p0"></param>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="time"> </param>
    /// <param name="timeMax"></param>
    /// <returns></returns>
    public static Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, float time, float timeMax)
    {
        float t = time / timeMax;
        float _1_t = 1 - t;
        float _1_t_exp2 = _1_t * _1_t;
        float _1_t_exp3 = _1_t_exp2 * _1_t;
        float t_exp2 = t * t;
        float t_exp3 = t_exp2 * t;

        float x = _1_t_exp3 * p0.x + 3 * _1_t_exp2 * t * p1.x + 3 * _1_t * t_exp2 * p2.x + t_exp3 * p2.x;
        float y = _1_t_exp3 * p0.y + 3 * _1_t_exp2 * t * p1.y + 3 * _1_t * t_exp2 * p2.y + t_exp3 * p2.y;
        float z = _1_t_exp3 * p0.z + 3 * _1_t_exp2 * t * p1.z + 3 * _1_t * t_exp2 * p2.z + t_exp3 * p2.z;

        return new Vector3(x, y, z);
    }

    /// <summary>
    /// Calcula los puntos de control de curva de Bezier parabólica iniPos y endPos, usando una altura máxima y 
    /// un factor de distancia para la ubicación del vértice (apexDistanceFactor).
    /// </summary>
    /// <param name="iniPos"></param>
    /// <param name="endPos"></param>
    /// <param name="upDirection"></param>
    /// <param name="apexHeight"></param>
    /// <param name="apexDistanceFactor"></param>
    /// <returns></returns>
    public static ParabolicBezierControlPoints GetParabolicBezierControlpoints(Vector3 iniPos, Vector3 endPos, Vector3 upDirection, float apexHeight, float apexDistanceFactor)
    {
        // Factor para determinar la distancia de los puntos de control respecto al vértice más alto (apex).
        float controlPointDistanceFactor = 0.5f;

        // Vector que representa la dirección entre los puntos iniPos y endPos.
        Vector3 aToB = endPos - iniPos;

        // Proyecta el vector aToB en el plano definido por la dirección vertical (upDirection).
        Vector3 flatAToB = Vector3.ProjectOnPlane(aToB, upDirection);

        // Calcula la posición del vértice más alto (apex) en función de apexHeight y apexDistanceFactor.
        Vector3 posApex = iniPos
                + flatAToB * apexDistanceFactor
                + apexHeight * upDirection;

        // Vectores desde el vértice más alto (apex) hasta los puntos iniPos y endPos.
        Vector3 apexToA = iniPos - posApex;
        Vector3 apexToB = endPos - posApex;

        ParabolicBezierControlPoints controlPoints;

        // Calcula el primer punto de control para la curva de Bezier parabólica.
        controlPoints.controlPointA = posApex
                + controlPointDistanceFactor
                * Vector3.ProjectOnPlane(apexToA, upDirection);

        // Calcula el segundo punto de control para la curva de Bezier parabólica.
        controlPoints.controlPointB = posApex
                + controlPointDistanceFactor
                * Vector3.ProjectOnPlane(apexToB, upDirection);

        // Calcula los vectores entre los puntos iniPos y endPos hacia sus respectivos puntos de control.
        controlPoints.controlAToA = iniPos - controlPoints.controlPointA;
        controlPoints.controlBToB = endPos - controlPoints.controlPointB;

        // Calcula los vectores desde el vértice más alto (apex) hacia los puntos de control.
        controlPoints.controlAToApex = posApex - controlPoints.controlPointA;
        controlPoints.controlBToApex = posApex - controlPoints.controlPointB;

        // Devuelve los puntos de control para la curva de Bezier parabólica.
        return controlPoints;
    }

    /// <summary>
    /// Calcula la posición actual en una curva parabólica en función del tiempo transcurrido y puntos de control dados.
    /// </summary>
    /// <param name="deltaTime"></param>
    /// <param name="maxTimeToApex"></param>
    /// <param name="maxTimeToB"></param>
    /// <param name="apexTravelFactor"></param>
    /// <param name="controlPoints"></param>
    /// <returns></returns>
    public static Vector3 StepParabolicBezier(float deltaTime, float maxTimeToApex, float maxTimeToB, float apexTravelFactor, ParabolicBezierControlPoints controlPoints)
    {
        // Calcula el tiempo total de recorrido hasta el vértice más alto (apex) de la curva.
        float totalTime = ((maxTimeToApex) / apexTravelFactor);

        // Calcula el valor del parámetro "t" que indica el progreso en el tiempo entre 0 y 1.
        float overallT = (float)deltaTime / (totalTime);

        Vector3 control, controlToOrigin, controlToDest;
        float t;

        // Verifica si el tiempo total transcurrido está dentro del tramo de A al vértice más alto (apex).
        if (overallT < apexTravelFactor)
        {
            // Calcula el valor de "t" para el tramo de A al vértice más alto (apex).
            float overallTH = overallT;

            // Asigna los puntos de control correspondientes a este tramo.
            control = controlPoints.controlPointA;
            controlToOrigin = controlPoints.controlAToA;
            controlToDest = controlPoints.controlAToApex;

            t = overallTH / apexTravelFactor; // Normaliza "t" para el tramo actual.
        }
        else
        {
            // Calcula el tiempo total transcurrido desde el vértice más alto (apex) hasta B.
            float overallTH = (float)(deltaTime - (maxTimeToApex)) / (maxTimeToB);

            // Asigna los puntos de control correspondientes a este tramo.
            control = controlPoints.controlPointB;
            controlToOrigin = controlPoints.controlBToApex;
            controlToDest = controlPoints.controlBToB;

            t = (overallTH) / (1f - apexTravelFactor); // Normaliza "t" para el tramo actual.
        }

        // Calcula la posición actual en la curva utilizando la fórmula de Bezier parabólica.
        Vector3 currentPos = control
                + Mathf.Pow(1f - t, 2f) * controlToOrigin
                + Mathf.Pow(t, 2f) * controlToDest;

        return currentPos; 
    }

    public static void GetParabolicByThreePoints(Vector2 p1, Vector2 p2, Vector2 p3, out float A, out float B, out float C)
    {
        float denom = (p1.x - p2.x) * (p1.x - p3.x) * (p2.x - p3.x);
        A = (p3.x * (p2.y - p1.y) + p2.x * (p1.y - p3.y) + p1.x * (p3.y - p2.y)) / denom;
        B = (p3.x * p3.x * (p1.y - p2.y) + p2.x * p2.x * (p3.y - p1.y) + p1.x * p1.x * (p2.y - p3.y)) / denom;
        C = (p2.x * p3.x * (p2.x - p3.x) * p1.y + p3.x * p1.x * (p3.x - p1.x) * p2.y + p1.x * p2.x * (p1.x - p2.x) * p3.y) / denom;
    }

    public static Vector2 GetParabolicMovement(Vector2 p1, float A, float B, float C, float incX)
    {
        float x = p1.x+incX;
        float y = A * (x * x) + B*(x) + C;
        return new Vector2(x, y);
    }


    public static Vector2 GetParabolicInitialVelocity(Vector2 initialPoint, Vector2 target, float angle, float g, float verticalRange = 1f)
    {
        // Y = V0yt - 0.5gt^2
        // V0x = |V0| * cos(a)
        // V0y = |V0| * sin(a)
        // shotDistance (R) = V0x * T.
        // H = when V0y == 0 => t/2 h = 0.5g*t^2
        // T = 2V0y/G Time
        // R = V0x * 2V0y/g Using trigonometric ratios
        // V0 = sqrt(RG/Sin(2a)) where R = distance, a = angle and G gravity.
        if ((target.y - initialPoint.y) > verticalRange)
            return Upward(initialPoint, target, angle, g);
        else if ((target.y - initialPoint.y) < -verticalRange)
                return Downward(initialPoint, target, angle, g);
        else
            return FullParabolic(initialPoint,target,angle,g);
        
    }

    public static Vector3 ApplyVelocityInXZPlane(Vector3 direction, Vector2 v)
    {
        Vector2 d = new Vector2(direction.x, direction.z);
        d.Normalize();
        return  new Vector3(d.x * v.x, v.y, d.y * v.x);
    }

    public static Vector3 GetParabolicInitialVelocityOverXZPlane(Vector3 initialPoint, Vector3 target, float angle, float g, out Vector3 direction)
    {
        direction = target - initialPoint;
        Vector2 dir2D = new Vector2(target.x, target.z) - new Vector2(initialPoint.x, initialPoint.z);
        float yDistance = target.y - initialPoint.y;
        return Trajectories.GetParabolicInitialVelocity(Vector2.zero, new Vector2(dir2D.magnitude, 0f), 60f, -40f);

    }


    private static Vector2 Downward(Vector2 initialPoint, Vector2 target, float angle, float g)
    {
        //V0x = d / t where t = sqrt(2*h/g)
        //V0y = 0
        Vector2 targetVector = target - initialPoint;
        float d = Mathf.Abs(target.x - initialPoint.x);
        float h = Mathf.Abs(target.y - initialPoint.y);
        float t = Mathf.Sqrt(2 * h / Mathf.Abs(g));
        float V0x = d / t * Mathf.Sign(targetVector.x);
        float V0y = 0f;
        Vector2 V0 = new Vector2(V0x, V0y);
        return V0;
    }

    private static Vector2 Upward(Vector2 initialPoint, Vector2 target, float angle, float g)
    {
        //V0x = d/t where t = V0y/g
        //V0y = sqrt(2*g*h)
        Vector2 targetVector = target - initialPoint;
        float d = Mathf.Abs(target.x - initialPoint.x);
        float h = Mathf.Abs(target.y - initialPoint.y);
        float V0y = Mathf.Sqrt(2 * Mathf.Abs(g) * h);
        float t = V0y / Mathf.Abs(g);
        float V0x = d / t * Mathf.Sign(targetVector.x);
        Vector2 V0 = new Vector2(V0x, V0y);
        return V0;

    }
    private static Vector2 FullParabolic(Vector2 initialPoint, Vector2 target, float angle, float g)
    {
        // V0x = |V0| * cos(a)
        // V0y = |V0| * sin(a)
        Vector2 targetVector = target - initialPoint;
        float X = Mathf.Abs(target.x - initialPoint.x);
        float num = X * Mathf.Abs(g);
        float den = Mathf.Sin(Mathf.Deg2Rad * angle * 2);
        float V0_module = Mathf.Sqrt(num / den);

        float V0x = V0_module * Mathf.Cos(Mathf.Deg2Rad * angle) * Mathf.Sign(targetVector.x);
        float V0y = V0_module * Mathf.Sin(Mathf.Deg2Rad * angle);
        Vector2 V0 = new Vector2(V0x, V0y);
        //Debug.Log("Max "+GetMaxHeight(angle, V0_module, Mathf.Abs(g)));
        return V0;
    }

    private static Vector3 FullParabolic3D(Vector3 initialPoint, Vector3 target, float angle, float g)
    {
        Vector3 targetVector = target - initialPoint;
        float X = Mathf.Abs(targetVector.x);
        //float Y = targetVector.y;
        //float Z = Mathf.Abs(targetVector.z);

        // Solving for V0x, V0y, and V0z:
        // V0x = |V0| * cos(angle) * sign(targetVector.x)
        // V0y = |V0| * sin(angle)
        // V0z = |V0| * cos(angle) * sign(targetVector.z)
        float num = X * Mathf.Abs(g);
        float den = Mathf.Sin(Mathf.Deg2Rad * angle * 2);
        float V0_module = Mathf.Sqrt(num / den);

        float V0x = V0_module * Mathf.Cos(Mathf.Deg2Rad * angle) * Mathf.Sign(targetVector.x);
        float V0y = V0_module * Mathf.Sin(Mathf.Deg2Rad * angle);
        float V0z = V0_module * Mathf.Cos(Mathf.Deg2Rad * angle) * Mathf.Sign(targetVector.z);

        Vector3 V0 = new Vector3(V0x, V0y, V0z);
        return V0;
    }

    public static float GetMaxHeight(float angle, float v0, float g)
    {
        float sin = Mathf.Sin(Mathf.Deg2Rad * angle);
        return (v0 * v0 * sin * sin) / (2f * Mathf.Abs(g));
    }


    public static Vector2 SimulateParabolicStep(Vector2 V0, float g, float elapseTime, float deltaTime)
    {
        Vector2 v = Vector2.zero;
        v.x = V0.x * deltaTime;
        v.y = (V0.y - (Mathf.Abs(g) * elapseTime)) * deltaTime;
        return v;
    }

    public static Vector3 SimulateParabolicStep3D(Vector3 V0, float g, float elapseTime, float deltaTime)
    {
        Vector3 v = Vector3.zero;
        v.x = V0.x * deltaTime;
        v.y = (V0.y - Mathf.Abs(g) * elapseTime) * deltaTime;
        v.z = V0.z * deltaTime;
        return v;
    }


    public static float UniformAccelerateVelocity(float v0, float a, float deltaT)
	{
		return v0 + a*deltaT;
	}
	
	public static float UniformAccelerateMovement(float v0, float a, float deltaT)
	{
		return v0*deltaT + 0.5f*a*deltaT*deltaT;
	}


	
	/*public static Vector2 ParabolicShoot(float v0x, float v0y, float angle, float g, float deltaT)
	{
		Vector2 increment = new Vector2();
		
		increment.x = v0x*Mathf.Cos(Mathf.Deg2Rad*angle)*deltaT;
		increment.y = v0y*Mathf.Sin(Mathf.Deg2Rad*angle)*deltaT - 0.5f*g*deltaT*deltaT;
		return increment;
	}
	
	public static float ParabolicShootMaxDistance(float v0, float angle, float g)
	{
		return (v0*v0 * 2f*Mathf.Sin(Mathf.Deg2Rad*angle)*Mathf.Cos(Mathf.Deg2Rad*angle)) / g;
	}
	
	public static float ParabolicShootTimeInMovement(float v0, float angle, float g)
	{
		return ( 2f*v0 * Mathf.Sin(Mathf.Deg2Rad*angle) ) / g;
	}
	
	public static float ParabolicShootMaxHeight(float v0, float angle, float g)
	{
		float sin = Mathf.Sin(Mathf.Deg2Rad*angle);
		return (v0*v0 *sin*sin) / (2f*g);
	}*/
	
}



