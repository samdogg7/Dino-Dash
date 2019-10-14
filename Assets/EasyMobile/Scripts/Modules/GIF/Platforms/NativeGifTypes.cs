using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

namespace EasyMobile.Internal.Gif
{
    internal delegate void DecodeCompleteCallback(int taskId, GifMetadata gifMetadata, GifFrameMetadata[] gifFrameMetadata, Color32[][] imageData);

    internal struct GifMetadata
    {
        public int width;
        public int height;
        public int frameCount;
    }

    internal struct GifFrameMetadata
    {
        public int delayTime;           /* pre-display delay in 0.01sec units */
        public int transparentColor;    /* Palette index for transparency, -1 if none */
    }

    internal class GifDecodeResources
    {
        // Buffers
        public GCHandle gifMetadataHandle;
        public GCHandle[] frameMetadataHandles;
        public GCHandle[] imageDataHandles;

        // Callbacks
        public DecodeCompleteCallback completeCallback;
    }
}