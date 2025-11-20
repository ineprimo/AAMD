using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;


class XMLUtils
{
    public static XmlNode GetFirstNode(XmlNode node, string s)
    {
        XmlNode result = null;
        XmlNodeList list = node.ChildNodes;
        for (int i = 0; i < (list.Count) && (result == null); ++i)
        {
            XmlNode child = list[i];
            if(child.Name == s)
            {
                result = child;
            }
        }
        return result;
    }

    public static List<XmlNode> GetNodes(XmlNode node, string s)
    {
        List<XmlNode> result = null;
        XmlNodeList list = node.ChildNodes;
        for (int i = 0; i < list.Count; ++i)
        {
            XmlNode child = list[i];
            if (child.Name == s)
            {
                if (result == null)
                    result = new List<XmlNode>();
                result.Add(child);
            }
        }
        return result;
    }
}

