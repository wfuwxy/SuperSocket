﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using AnyLog;
using SuperSocket.SocketBase.Metadata;

namespace SuperSocket.SocketEngine
{
    class RemoteBootstrapProxy : MarshalByRefObject, IBootstrap
    {
        class ServerProxy : MarshalByRefObject, IManagedApp
        {
            private IManagedApp m_Server;

            public ServerProxy(IManagedApp server)
            {
                m_Server = server;
            }

            public bool Setup(IBootstrap bootstrap, IServerConfig config)
            {
                throw new NotSupportedException();
            }

            public ServerState State
            {
                get { return m_Server.State; }
            }

            public string Name
            {
                get { return m_Server.Name; }
            }

            public void ReportPotentialConfigChange(IServerConfig config)
            {
                m_Server.ReportPotentialConfigChange(config);
            }

            public bool Start()
            {
                return m_Server.Start();
            }

            public void Stop()
            {
                m_Server.Stop();
            }

            public int SessionCount
            {
                get { throw new NotSupportedException(); }
            }

            public AppServerMetadata GetAppServerMetadata()
            {
                throw new NotSupportedException();
            }

            public StatusInfoCollection CollectServerStatus(StatusInfoCollection bootstrapStatus)
            {
                throw new NotSupportedException();
            }

            public void TransferSystemMessage(string messageType, object messageData)
            {
                throw new NotSupportedException();
            }
        }

        private IBootstrap m_Bootstrap;

        private List<IManagedApp> m_Servers = new List<IManagedApp>();

        public RemoteBootstrapProxy()
        {
            m_Bootstrap = (IBootstrap)AppDomain.CurrentDomain.GetData("Bootstrap");

            foreach (var s in m_Bootstrap.AppServers)
            {
                if (s is MarshalByRefObject)
                    m_Servers.Add(s);
                else
                    m_Servers.Add(new ServerProxy(s));
            }
        }

        public IEnumerable<IManagedApp> AppServers
        {
            get { return m_Servers; }
        }

        public IRootConfig Config
        {
            get { return m_Bootstrap.Config; }
        }

        public bool Initialize()
        {
            throw new NotSupportedException();
        }

        public bool Initialize(IDictionary<string, System.Net.IPEndPoint> listenEndPointReplacement)
        {
            throw new NotSupportedException();
        }

        public bool Initialize(Func<IServerConfig, IServerConfig> serverConfigResolver)
        {
            throw new NotSupportedException();
        }

        public bool Initialize(ILoggerFactory loggerFactory)
        {
            throw new NotSupportedException();
        }

        public bool Initialize(Func<IServerConfig, IServerConfig> serverConfigResolver, ILoggerFactory loggerFactory)
        {
            throw new NotSupportedException();
        }

        public StartResult Start()
        {
            throw new NotSupportedException();
        }

        public void Stop()
        {
            throw new NotSupportedException();
        }

        public string StartupConfigFile
        {
            get { return m_Bootstrap.StartupConfigFile; }
        }

        public string BaseDirectory
        {
            get { return m_Bootstrap.BaseDirectory; }
        }

        public override object InitializeLifetimeService()
        {
            //Never expire
            return null;
        }
    }
}
