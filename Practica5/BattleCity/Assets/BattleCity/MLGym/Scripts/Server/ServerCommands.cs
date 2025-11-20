using Kodai100.Tcp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace MLGym
{
    public class ServerCommands : MonoBehaviour
    {
        public UnityTCP tcpServer;
        private TcpClient client;
        private Dictionary<string, TcpClient> clients;

        public UnityEvent<string,TcpClient> OnInit;
        public UnityEvent<string,string> AddAgent;
        public UnityEvent<string> RemoveAgent;


        public void OnMessage(string msg)
        {
            Debug.LogWarning(msg);

            if (msg.StartsWith("ok"))
            {
                return;
            }
            else if(msg.StartsWith("error"))
            {
                return;
            }

            Dictionary<string,string> attributes= SplitAttributes(msg);
            if(attributes != null)
            {
                
                string command = attributes["command"];
                switch (command)
                {
                    case "hello":
                        {
                            string id = attributes["id"];
                            clients.Add(id, client);
                            SendOk(client);
                        }
                        break;
                    case "init":
                        {
                            TcpClient c = GetClient(attributes);
                            string id = attributes["id"];
                            SendOk(c);
                            if(OnInit!=null)
                                OnInit.Invoke(id,c);
                        }
                        break;
                    case "addagent":
                        {
                            string agentID = attributes["agentid"];
                            string format = attributes["format"];
                            if (format == "custom")
                            {
                                string type = attributes["type"];
                                string agent = attributes["agent"];
                                if(AgentAllowed(agentID, agent))
                                {
                                    AddAgent?.Invoke(agentID, agent);
                                    SendOk(GetClient(attributes));
                                }
                                else
                                {
                                    SendAggentNotAllowed(GetClient(attributes), agentID);
                                }
                            }
                            else
                            {
                                Debug.Log("El formato del agente no es el esperado");
                            }
   
                        }
                        break;
                    case "reset":
                        {
                            SendOk(GetClient(attributes));
                            ResetLevel();
                        }
                        break;
                    case "actions":
                        {
                            Actions(GetClientID(attributes),attributes);
                            SendOk(GetClient(attributes));
                        }
                        break;
                    default:
                        {
                            SendUknownCommand(GetClient(attributes));
                            break;
                        }
                }
            }
            else
            {
                SendBadFormat(this.client);
            }
            
        }

        public virtual bool AgentAllowed(string id, string agentInfo)
        {
            return id != "";
        }

        public virtual void Actions(string id, Dictionary<string, string> attributes)
        {

        }

        public void SendPerception(TcpClient client, MLGym.Parameters parameters, bool gameOver, bool isDestroyed)
        {
            if (parameters == null)
                this.tcpServer.SendMessageToClient(client, "command=perception&gameover=" + gameOver + "&destroyed=" + isDestroyed);
            else
                this.tcpServer.SendMessageToClient(client, "command=perception&gameover=" + gameOver + "&destroyed=" + isDestroyed + "&parameters=" + parameters.Serialize());
        }

        public void SendPerceptionAndMap(TcpClient client, MLGym.Parameters parameters, IMap map, bool gameOver, bool isDestroyed)
        {
            if(parameters == null)
                this.tcpServer.SendMessageToClient(client, "command=perception_map&gameover=" + gameOver + "&destroyed=" + isDestroyed);
            else
                this.tcpServer.SendMessageToClient(client, "command=perception_map&gameover=" + gameOver + "&destroyed=" + isDestroyed  + "&parameters="+ parameters.Serialize() + "&map=" + map.Serialize());
        }

        public void SendMetrics(TcpClient client, MetricsData[] metrics)
        {
            this.tcpServer.SendMessageToClient(client, "command=metrics&ids="+ MetricsData.GenerateIDArray(metrics) + 
                "&time=" + MetricsData.GenerateTimeArray(metrics) + "&checkpoints=" + MetricsData.GenerateCheckpointsArray(metrics) + 
                "&collisions=" + MetricsData.GenerateCollisionsArray(metrics));
        }

        public void OnEstablished(TcpClient client)
        {
            Debug.LogWarning(client);
            this.client = client;
            this.tcpServer.SendMessageToClient(client, "ok");
        }

        public void OnDisconnected(EndPoint endPoint)
        {
            Debug.LogWarning(endPoint);
            string key = "";
            List<string> list = new List<string>();
            foreach (var pair in clients)
            {
                if(!pair.Value.Client.Connected)
                {
                    list.Add(pair.Key);
                }
            }

            for(int i = 0; i < list.Count; i++)
            {
                RemoveAgent?.Invoke(list[i]);
                clients.Remove(list[i]);
            }
                
        }
        // Start is called before the first frame update
        void Start()
        {
            clients = new Dictionary<string, TcpClient>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void SendOk(TcpClient cl)
        {
            tcpServer.SendMessageToClient(cl, "ok");
        }

        private void SendBadFormat(TcpClient client)
        {
            Log("error 01: bad format");
            tcpServer.SendMessageToClient(client, "error=01&name=bad format");
        }

        private void SendAggentNotAllowed(TcpClient client, string id)
        {
            Log("error 02: agent not allowed " + id);
            tcpServer.SendMessageToClient(client, "error=02&name=agent not allowed "+id);
        }

        private void SendUknownCommand(TcpClient client)
        {
            Log("error 02: unknown command");
            tcpServer.SendMessageToClient(client, "error=02&name=unknown command");
        }

        private TcpClient GetClient(Dictionary<string, string> attributes)
        {
            string id = attributes["id"];
            TcpClient client = clients[id];
            return client;
        }

        private string GetClientID(Dictionary<string, string> attributes)
        {
            string id = attributes["id"];
            return id;
        }


        /// <summary>
        /// Divide el mensaje en atributos.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Devuelve un diccionario con el nombre del atributo y el valor o null si el formato del mensaje no es correcto</returns>
        private Dictionary<string, string> SplitAttributes (string data)
        {
            Dictionary<string, string> msgAttributes = new Dictionary<string, string>();
            try
            {
                string[] attributes = data.Split("&");
                foreach (string attribute in attributes)
                {
                    string[] pairKeyValue = attribute.Split("=");
                    msgAttributes.Add(pairKeyValue[0].Trim(), pairKeyValue[1].Trim());
                }
            } catch(Exception e)
            {
                Debug.LogError("Bad format msg " + e);
                return null;
            }
            return msgAttributes;
        }

        protected virtual void Log(string msg)
        {
            Debug.Log(msg);
        }

        public void ResetLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //SceneManager.LoadScene(currentScene);
        }
    }
}
