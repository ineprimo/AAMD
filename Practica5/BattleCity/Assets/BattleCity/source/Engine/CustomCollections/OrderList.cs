using System;
using System.Collections.Generic;

public class ListItem<T> where T : IComparable<T>
{
    private T _content;
    private ListItem<T> _next;

    public ListItem(T content)
    {
        _content = content;
        _next = null;
    }

    public T Item
    {
        get { return _content; }
    }

    public ListItem<T> Next
    {
        get { return _next; }
        set { _next = value; }
    }
}

public class OrderListEmpyExeption : GameException
{
    public OrderListEmpyExeption(string msg) : base(msg) { }
}

public class OrderList<T> where T : IComparable<T>
{
    private ListItem<T> _first;
    private int _count;

    public OrderList()
    {
        _first = null;
        _count = 0;
    }

    public void Clear()
    {
        _first = null;
        _count = 0;
    }

    public ListItem<T> GetIterator()
    {
        return _first;
    }

    public IEnumerable<T> GetElements()
    {
        if (_first == null)
            throw new OrderListEmpyExeption("Error try to get the first element. The OrderList is empty");
        else
        {
            ListItem<T> current = _first;
            while (current != null)
            {
                T item = current.Item;
                current = current.Next;
                yield return item;
            }
        }
    }

    public T First
    {
        get
        {
            if (_first == null)
                throw new OrderListEmpyExeption("Error try to get the first element. The OrderList is empty");
            return _first.Item;
        }
    }



    public void RemoveFirst()
    {
        if (_first != null)
        {
            _first = _first.Next;
            _count--;
        }
    }

    public bool Remove(T element)
    {
        bool ret = false;
        if(_first != null)
        {
            if(_first.Item.Equals(element))
            {
                _first = _first.Next;
                ret = true;
                _count--;
            }
            else
            {
                ListItem<T> aux = _first.Next;
                ListItem<T> last = _first;
                while (aux != null && !ret)
                {
                    if (aux.Item.Equals(element))
                    {
                        aux = aux.Next;
                        ret = true;
                        _count--;
                        last.Next = aux;
                    }
                    else
                    {
                        last = aux;
                        aux = aux.Next;
                    }
                }
            }
        }
        return ret;
    }

    public T ExtractFirst()
    {
        T t = First;
        RemoveFirst();
        return t;
    }

    public int Count
    {
        get { return _count; }
    }

    public void InsertInOrder(T element)
    {
        ListItem<T> newItem = new ListItem<T>(element);
        if (_first == null) // la lista esta vacía
            _first = newItem;
        else // si no está vacía
        {
            if (element.CompareTo(_first.Item) < 0) //si es menor que le primero, va antes que el primero
            {
                ListItem<T> n = _first;
                _first = newItem; //ahora le primero es el nuevo
                _first.Next = n; //el que era el primero pasa a ser segundo.
            }
            else // si va despues del primero
            {
                ListItem<T> previous = _first; //me apunto primero
                ListItem<T> current = _first.Next; // me apunto el siguiente al primero y es el current
                bool inserted = false;
                while (!inserted && current != null) //mientras no inserte o haya comprobado le orden de todos...
                {
                    if (element.CompareTo(current.Item) < 0) //va antes que el current
                    {
                        previous.Next = newItem; //el siguiente del previo ahora es el nuevo
                        newItem.Next = current; // el siguiente del nuevo ahora es el current
                        inserted = true;
                    }
                    else
                    {
                        previous = current;
                        current = current.Next;
                    }
                }

                if(!inserted)
                {
                    previous.Next = newItem;
                }  
            }
        }
        _count++;
    }
}
