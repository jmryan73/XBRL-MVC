using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace XBRLApp.Common
{
    public class XmlFunctionHelper
    {
        private string _fileXmlPath;

        public XmlFunctionHelper(string fileXmlPath)
        {
            this._fileXmlPath = fileXmlPath;
        }

        public XmlNode GetSingleNodeByTagName(string tagName)
        {
            XmlTextReader reader = new XmlTextReader(_fileXmlPath);
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);

            XmlNodeList nodeList = doc.GetElementsByTagName(tagName);
            XmlNode nodeGet = null;

            foreach (XmlNode node in nodeList)
            {
                if(node.Name.Trim() == tagName)
                {
                    nodeGet = node;
                    break;
                }
            }
            return nodeGet;
        }

        public XmlNodeList GetAllNodesByTagName(string tagName)
        {
            XmlTextReader reader = new XmlTextReader(_fileXmlPath);
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);

            XmlNodeList nodeList = doc.GetElementsByTagName(tagName);

            return nodeList;
        }


    }
}
