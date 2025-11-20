using UnityEngine;

public class PerceptionBase : MonoBehaviour
{
    public BoxCollider boxCollider;
    public float visionDistance;
    public TankFire tankFire;
    public Health health;
    public float perceptionSizeFactor = 0.5f;

    protected Transform commandCenter;
    protected Transform life;
    protected int[] perceptionNeighborhood = new int[4];
    protected float[] perceptionNeighborhoodDistance = new float[4];
    protected Vector2 commandCenterPosition = Vector2.zero;
    protected Vector2 lifePosition = Vector2.zero;
    protected float time;
    protected MLGym.Parameters parameters;

    public MLGym.Parameters Parameters
    {
        get
        {
            return parameters;
        }
    }

    public enum INPUT_TYPE { NOTHING = 0, UNBREAKABLE = 1, BRICK = 2, COMMAND_CENTER = 3, PLAYER = 4, SHELL = 5, OTHER_AGENT = 6, LIFE = 7, SEMI_BREKABLE = 8, SEMI_UNBREKABLE = 9, EXIT = 10 }

    public enum ACTION_TYPE { NOTHING = 0, MOVE_UP = 1, MOVE_DOWN = 2, MOVE_RIGHT = 3, MOVE_LEFT = 4 }

    public Vector2 CommandCenterPosition
    {
        get { return commandCenterPosition; }
    }

    public Vector2 LifePosition
    {
        get { return lifePosition; }
    }

    public bool CanFire
    {
        get
        {
            return tankFire.CanFire;
        }
    }

    public int[] PerceptionNeighborhood
    {
        get { return perceptionNeighborhood; }
    }

    public float[] PerceptionNeighborhoodDistance
    {
        get { return perceptionNeighborhoodDistance; }
    }

    public Transform CommandCenter
    {
        get
        {
            if (commandCenter == null)
            {
                GameObject cc = GameObject.FindGameObjectWithTag(BCGlobals.CommandCenter);
                if (cc != null)
                    commandCenter = cc.transform;
            }
            return commandCenter;
        }
    }

    public Transform Life
    {
        get
        {
            if (life == null)
            {
                GameObject cc = GameLogic.Get().Life;
                if (cc != null)
                    life = cc.transform;
            }
            return life;
        }
    }

    protected void _Start()
    {
        time = 0f;
    }

    protected void _Update(int pnInit)
    {
        time += Time.deltaTime;
        int layer = 1 << Globals.GROUND_LAYER | 1 << Globals.ENEMY_LAYER | 1 << BCGlobals.COMMAND_CENTER | 1 << BCGlobals.SHELL | 1 << Globals.PLAYER_LAYER;
        bool isMultyAgent = GameLogic.Get().IsMultyAgent;
        for (int i = 0; i < perceptionNeighborhood.Length; i++)
        {
            perceptionNeighborhood[i] = pnInit;
        }

        for (int i = 0; i < perceptionNeighborhoodDistance.Length; i++)
        {
            perceptionNeighborhoodDistance[i] = 0;
        }

        lifePosition = Life != null ? Life.position : Vector2.zero;
        commandCenterPosition = CommandCenter != null ? CommandCenter.position : Vector3.zero;

        float radious = 0.25f;
        RaycastHit upHit;
        if (Physics.BoxCast(this.transform.position + boxCollider.center, boxCollider.size * perceptionSizeFactor, Vector3.up, out upHit, Quaternion.identity, visionDistance, layer))
        {
            if (IsDetectMyShell(upHit))
                Debug.DrawRay(this.transform.position + Vector3.back * (2 * radious), Vector3.up * visionDistance, Color.white);
            else
            {
                perceptionNeighborhood[0] = GetID(isMultyAgent, upHit);
                perceptionNeighborhoodDistance[0] = upHit.distance;
                Debug.DrawRay(this.transform.position + Vector3.back * (2 * radious), Vector3.up * upHit.distance, Color.red);
            }
        }
        else
            Debug.DrawRay(this.transform.position + Vector3.back * (2 * radious), Vector3.up * visionDistance, Color.white);


        RaycastHit downHit;
        if (Physics.BoxCast(this.transform.position + boxCollider.center, boxCollider.size * perceptionSizeFactor, Vector3.down, out downHit, Quaternion.identity, visionDistance, layer))
        //if (Physics.SphereCast(new Ray(this.transform.position + Vector3.back * (2 * radious), Vector3.down), radious, out downHit, visionDistance, layer))
        {
            if (IsDetectMyShell(downHit))
                Debug.DrawRay(this.transform.position + Vector3.back * (2 * radious), Vector3.down * visionDistance, Color.white);
            else
            {
                perceptionNeighborhood[1] = GetID(isMultyAgent, downHit);
                perceptionNeighborhoodDistance[1] = downHit.distance;
                Debug.DrawRay(this.transform.position + Vector3.back * (2 * radious), Vector3.down * downHit.distance, Color.red);
            }
        }
        else
            Debug.DrawRay(this.transform.position + Vector3.back * (2 * radious), Vector3.down * visionDistance, Color.white);

        RaycastHit rightHit;
        if (Physics.BoxCast(this.transform.position + boxCollider.center, boxCollider.size * perceptionSizeFactor, Vector3.right, out rightHit, Quaternion.identity, visionDistance, layer))
        //if (Physics.SphereCast(new Ray(this.transform.position + Vector3.back * (2 * radious), Vector3.right), radious, out rightHit, visionDistance, layer))
        {
            if (IsDetectMyShell(rightHit))
                Debug.DrawRay(this.transform.position + Vector3.back * (2 * radious), Vector3.right * visionDistance, Color.white);
            else
            {
                perceptionNeighborhood[2] = GetID(isMultyAgent, rightHit);
                perceptionNeighborhoodDistance[2] = rightHit.distance;
                Debug.DrawRay(this.transform.position + Vector3.back * (2 * radious), Vector3.right * rightHit.distance, Color.red);
            }
        }
        else
            Debug.DrawRay(this.transform.position + Vector3.back * (2 * radious), Vector3.right * visionDistance, Color.white);

        RaycastHit leftHit;
        if (Physics.BoxCast(this.transform.position + boxCollider.center, boxCollider.size * perceptionSizeFactor, Vector3.left, out leftHit, Quaternion.identity, visionDistance, layer))
        //if (Physics.SphereCast(new Ray(this.transform.position + Vector3.back * (2 * radious), Vector3.left), radious, out leftHit, visionDistance, layer))
        {
            if (IsDetectMyShell(leftHit))
                Debug.DrawRay(this.transform.position + Vector3.back * (2 * radious), Vector3.left * visionDistance, Color.white);
            else
            {
                perceptionNeighborhood[3] = GetID(isMultyAgent, leftHit);
                perceptionNeighborhoodDistance[3] = leftHit.distance;
                Debug.DrawRay(this.transform.position + Vector3.back * (2 * radious), Vector3.left * leftHit.distance, Color.red);
            }
        }
        else
            Debug.DrawRay(this.transform.position + Vector3.back * (2 * radious), Vector3.left * visionDistance, Color.white);
    }

    private bool IsDetectMyShell(RaycastHit upHit)
    {
        if (upHit.collider.gameObject.layer == BCGlobals.SHELL)
        {
            ShellImpact shellInpact = upHit.collider.GetComponent<ShellImpact>();
            if (shellInpact != null)
            {
                TankFire tankFire = shellInpact.GetTankFire;
                if (tankFire != null)
                    return tankFire.gameObject == this.gameObject;
            }
        }
        return false;
    }

    public static int GetID(bool isMultyAgent, RaycastHit hit)
    {

        if (hit.collider.gameObject.layer == Globals.GROUND_LAYER) // ha chocado con el escenario.
        {
            Breakable breakable = hit.collider.gameObject.GetComponent<Breakable>();
            if (breakable != null)
                return (int)INPUT_TYPE.BRICK;
            else
                return (int)INPUT_TYPE.UNBREAKABLE;
        }
        else if (hit.collider.gameObject.layer == BCGlobals.COMMAND_CENTER) // Ha chocado con la commandCenter
        {
            return (int)INPUT_TYPE.COMMAND_CENTER;
        }
        else if (hit.collider.gameObject.layer == Globals.PLAYER_LAYER && !isMultyAgent) // Ha chocado con el player
        {
            return (int)INPUT_TYPE.PLAYER;
        }
        else if (hit.collider.gameObject.layer == Globals.ENEMY_LAYER && isMultyAgent) // Ha chocado con otro agente, lo consideramos player
        {
            //en el modo isMultyAgent no hay jugadores humanos.
            TankAgent tankAgent = hit.collider.gameObject.GetComponent<TankAgent>();
            if (tankAgent != null)
            {
                return (int)INPUT_TYPE.PLAYER;
            }
            else
            {
                return (int)INPUT_TYPE.OTHER_AGENT;
            }

        }
        else if (hit.collider.gameObject.layer == BCGlobals.SHELL) // Ha chocado una bala
        {
            return (int)INPUT_TYPE.SHELL;
        }
        else  // Ha chocado con algo desconocido
        {
            return (int)INPUT_TYPE.OTHER_AGENT;
        }
    }
}
