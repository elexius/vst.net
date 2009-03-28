﻿using System;
using System.Diagnostics;

using Jacobi.Vst.Core;

namespace Jacobi.Vst.Framework.Plugin
{
    /// <summary>
    /// The VstPluginAudioProcessorBase implements the <see cref="IVstPluginAudioProcessor"/> 
    /// interface and provides a basis for the Plugin implementation.
    /// </summary>
    public abstract class VstPluginAudioProcessorBase : IVstPluginAudioProcessor
    {
        /// <summary>
        /// A default ctor for derived classes.
        /// </summary>
        /// <remarks>When using this constructor you have to set the 
        /// <see cref="InputCount"/>, <see cref="OutputCount"/> and <see cref="TailSize"/>
        /// properties or they will be zero.</remarks>
        protected VstPluginAudioProcessorBase() { }

        /// <summary>
        /// Initialization ctor for derived classes.
        /// </summary>
        /// <param name="inputCount">The number of audio input channels.</param>
        /// <param name="outputCount">The number of audio output channels.</param>
        /// <param name="tailSize">The number of samples the Audio Processor will produce
        /// after input has stopped. Typically used in reverbs, echos and delays.</param>
        protected VstPluginAudioProcessorBase(int inputCount, int outputCount, int tailSize)
        {
            InputCount = inputCount;
            OutputCount = outputCount;
            TailSize = tailSize;
        }
        
        #region IVstPluginAudioProcessor Members
        
        /// <inheritdoc />
        public int InputCount { get; protected set; }
        
        /// <inheritdoc />
        public int OutputCount { get; protected set; }
        
        /// <inheritdoc />
        public int TailSize { get; protected set; }
        
        /// <inheritdoc />
        public virtual double SampleRate { get; set; }
        
        /// <inheritdoc />
        public virtual int BlockSize { get; set; }
        
        /// <inheritdoc />
        public virtual void Process(VstAudioBuffer[] inChannels, VstAudioBuffer[] outChannels)
        {
            int outCount = outChannels.Length;

            for (int n = 0; n < outCount; n++)
            {
                for (int i = 0; i < inChannels.Length && n < outCount; i++, n++)
                {
                    Copy(inChannels[i], outChannels[n]);
                }
            }
        }

        /// <inheritdoc />
        public virtual bool SetPanLaw(VstPanLaw type, float gain)
        {
            return false;
        }

        #endregion

        /// <summary>
        /// Copies the samples from the <paramref name="source"/> to the <paramref name="dest"/>ination.
        /// </summary>
        /// <param name="source">The source audio buffer. Must not be null.</param>
        /// <param name="dest">The destination audio buffer. Must be writable. Must not be null.</param>
        protected void Copy(VstAudioBuffer source, VstAudioBuffer dest)
        {
            Debug.Assert(source.SampleCount == dest.SampleCount);
            Debug.Assert(dest.CanWrite);

            unsafe
            {
                float* inputBuffer = ((IDirectBufferAccess32)source).Buffer;
                float* outputBuffer = ((IDirectBufferAccess32)dest).Buffer;

                for (int i = 0; i < source.SampleCount; i++)
                {
                    outputBuffer[i] = inputBuffer[i];
                }
            }
        }
    }
}
