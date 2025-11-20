using System;

public class AINode<T> : IComparable<T> where T : AINode<T>
{
    protected enum NodeStatus { UNDISCOVER, OPEN, CLOSE}
    private float _g;
    private float _h;
    private AINode<T> _parent;
    private NodeStatus _status;

    public AINode()
    {
        Reset();
    }

    public void Reset()
    {
        _g = float.MaxValue;
        _h = -1f;
        _parent = null;
        _status = NodeStatus.UNDISCOVER;
    }

    public int CompareTo(T other)
    {
        // If other is not a valid object reference, this instance is greater.
        if (other == null) return 1;

        return FCost.CompareTo(other.FCost);
    }

    public float GCost
    {
        get { return _g; }
        set { _g = value; }
    }

    public float HCost
    {
        get { return _h; }
        set { _h = value; }
    }

    public float FCost
    {
        get { return GCost + HCost; }
    }

    public AINode<T> Parent
    {
        get { return _parent; }
        set { _parent = value; }
    }

    public bool IsClose
    {
        get { return _status == NodeStatus.CLOSE; }
    }

    public bool IsOpen
    {
        get { return _status == NodeStatus.OPEN; }
    }

    public bool IsUndiscovered
    {
        get { return _status == NodeStatus.UNDISCOVER; }
    }

    public void SetClose()
    {
        _status = NodeStatus.CLOSE;
    }

    public void SetOpen()
    {
        _status = NodeStatus.OPEN;
    }
}
