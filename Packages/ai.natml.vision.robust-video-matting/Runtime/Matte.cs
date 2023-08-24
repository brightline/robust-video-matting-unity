/* 
*   Robust Video Matting
*   Copyright © 2023 NatML Inc. All Rights Reserved.
*/

namespace NatML.Vision {

    using System;
    using UnityEngine;

    public sealed partial class RobustVideoMattingPredictor {

        /// <summary>
        /// Alpha matte.
        /// Each pixel in the map returns a probability of that pixel location being a person (~1.0) or background (~0.0).
        /// </summary>
        public sealed class Matte {
            public Texture2D tempTexture;
            #region --Client API--
            /// <summary>
            /// Map width.
            /// </summary>
            public readonly int width;

            /// <summary>
            /// Map height.
            /// </summary>
            public readonly int height;

            /// <summary>
            /// Render the probability map to a texture.
            /// Each pixel will have value `(p, p, p, 1.0)` where `p` is the foreground probability for that pixel.
            /// </summary>
            /// <param name="destination">Destination texture.</param>
            public void Render (RenderTexture destination) {
                // Check texture
                if (!destination)
                    throw new ArgumentNullException(nameof(destination));
                // Upload texture data
                
                tempTexture.GetRawTextureData<float>().CopyFrom(data);
                tempTexture.Apply(false);
                // Blit
                renderer = renderer ? renderer : new Material(Shader.Find(@"Hidden/RobustVideoMatting/Blit"));
                Graphics.Blit(tempTexture, destination, renderer);
            }
            #endregion

            #region --Operations--
            public float[] data;
            private static Material renderer;

            internal Matte (int width, int height, float[] data) {
                this.width = width;
                this.height = height;
                this.data = data;
                tempTexture = new Texture2D(width, height, TextureFormat.RFloat, false);
            }
            #endregion
        }
    }
}