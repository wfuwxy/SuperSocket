﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyLog;

namespace SuperSocket.SocketBase.CompositeTargets
{
    class LoggerFactoryCompositeTarget : SingleResultCompositeTargetCore<ILoggerFactory, ILoggerFactoryMetadata>
    {
        public LoggerFactoryCompositeTarget(Action<ILoggerFactory> callback)
            : base((config) => config.LoggerFactory, callback, true)
        {

        }

        protected override bool MetadataNameEqual(ILoggerFactoryMetadata metadata, string name)
        {
            return metadata.Name.Equals(name, StringComparison.OrdinalIgnoreCase);
        }

        protected override IEnumerable<Lazy<ILoggerFactory, ILoggerFactoryMetadata>> Sort(IEnumerable<Lazy<ILoggerFactory, ILoggerFactoryMetadata>> factories)
        {
            return factories.OrderBy(f => f.Metadata.Priority);
        }

        protected override bool PrepareResult(ILoggerFactory result, IAppServer appServer, ILoggerFactoryMetadata metadata)
        {
            if(string.IsNullOrEmpty(metadata.ConfigFileName))
            {
                return result.Initialize(new string[0]);
            }

            var currentAppDomain = AppDomain.CurrentDomain;
            var isolation = IsolationMode.None;

            var isolationValue = currentAppDomain.GetData(typeof(IsolationMode).Name);

            if (isolationValue != null)
                isolation = (IsolationMode)isolationValue;

            var configFileName = metadata.ConfigFileName;

            if (Path.DirectorySeparatorChar != '\\')
            {
                configFileName = Path.GetFileNameWithoutExtension(configFileName) + ".unix" + Path.GetExtension(configFileName);
            }

            var configFiles = new List<string>();

            if (isolation == IsolationMode.None)
            {
                configFiles.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFileName));
                configFiles.Add(Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config"), configFileName));
            }
            else //The running AppServer is in isolated appdomain
            {
                configFiles.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFileName));
                configFiles.Add(Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config"), configFileName));

                //go to the application's root
                //the appdomain's root is /WorkingDir/DomainName, so get parent path twice to reach the application root
                var rootDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName;

                configFiles.Add(Path.Combine(rootDir, AppDomain.CurrentDomain.FriendlyName + "." + configFileName));
                configFiles.Add(Path.Combine(Path.Combine(rootDir, "Config"), AppDomain.CurrentDomain.FriendlyName + "." + configFileName));
                configFiles.Add(Path.Combine(rootDir, configFileName));
                configFiles.Add(Path.Combine(Path.Combine(rootDir, "Config"), configFileName));
            }

            if (!result.Initialize(configFiles.ToArray()))
            {
                throw new Exception("Failed to initialize the logfactory:" + metadata.Name);
            }

            return true;
        }
    }
}
