﻿using System;
using Jacobi.Vst3.Interop;

namespace Jacobi.Vst3.Plugin
{
    public abstract class ComponentBase : IPluginBase, IConnectionPoint, IServiceContainerSite
    {
        private IConnectionPoint _peer;

        protected ComponentBase()
        {
            ServiceContainer = new ServiceContainer();
        }

        #region IServiceContainerSite Members

        public ServiceContainer ServiceContainer { get; protected set; }

        #endregion

        #region IPluginBase Members

        public virtual int Initialize(object context)
        {
            System.Diagnostics.Trace.WriteLine("IPluginBase.Initialize");

            ServiceContainer.Unknown = context;

            return TResult.S_OK;
        }

        public virtual int Terminate()
        {
            System.Diagnostics.Trace.WriteLine("IPluginBase.Terminate");

            _peer = null;

            ServiceContainer.Dispose();

            return TResult.S_OK;
        }

        #endregion

        #region IConnectionPoint Members

        public virtual int Connect(IConnectionPoint other)
        {
            System.Diagnostics.Trace.WriteLine("IConnectionPoint.Connect");

            if (other == null)
            {
                return TResult.E_InvalidArg;
            }
            if (_peer != null)
            {
                return TResult.S_False;
            }

            _peer = other;

            return TResult.S_OK;
        }

        public virtual int Disconnect(IConnectionPoint other)
        {
            System.Diagnostics.Trace.WriteLine("IConnectionPoint.Disconnect");

            if (_peer != null && _peer == other)
            {
                _peer = null;

                return TResult.S_OK;
            }

            return TResult.S_False;
        }

        public virtual int Notify(IMessage message)
        {
            System.Diagnostics.Trace.WriteLine("IConnectionPoint.Notify");

            if (message == null)
            {
                return TResult.E_InvalidArg;
            }

            return OnMessageReceived(new MessageEventArgs(message)) ? TResult.S_OK : TResult.S_False;
        }

        #endregion

        protected virtual bool OnMessageReceived(MessageEventArgs messageEventArgs)
        {
            var handler = MessageReceived;

            if (handler != null)
            {
                handler(this, messageEventArgs);

                return true;
            }

            return false;
        }

        public event EventHandler<MessageEventArgs> MessageReceived;
    }
}
