using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis.Attributes;
using Artemis.Manager;
using Artemis.System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using CrawLib.Artemis.Components;
using Artemis;
using UmbraClient.Spatials;
using Microsoft.Xna.Framework;
using CrawLib;

namespace UmbraClient.Systems {
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = 1)]
    public class RenderSystem : EntityComponentProcessingSystem<SpatialFormComponent, TransformComponent> {
        private ContentManager _content;
        private SpriteBatch _spriteBatch;
        private GraphicsDevice _graphicsDevice;

        private BasicEffect _effect;

        private Texture2D _texture;
        private VertexDeclaration _vertexDeclaration;

        private string _spatialName;

        public override void LoadContent() {
            _content = BlackBoard.GetEntry<ContentManager>("ContentManager");
            _spriteBatch = BlackBoard.GetEntry<SpriteBatch>("SpriteBatch");
            _graphicsDevice = BlackBoard.GetEntry<GraphicsDevice>("GraphicsDevice");
            ContentManager content = BlackBoard.GetEntry<ContentManager>("ContentManager");

            _texture = content.Load<Texture2D>("Images/OryxEnv");

            _effect = new BasicEffect(_graphicsDevice); //content.Load<Effect>("Effects/DiffuseColorEffect");
            _effect.TextureEnabled = true;
            _effect.Texture = _texture;

            //CreateCubeVertexBuffer();
            //CreateCubeIndexBuffer();

            _vertexDeclaration = new VertexDeclaration(new VertexElement[]
    {
        new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
        new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
        new VertexElement(24, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
    }
);
        }

        public override void Process(Entity entity, SpatialFormComponent spatialFormComponent, TransformComponent transformComponent) {

            Matrix World = Matrix.Identity;
            //Matrix View = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
            //Matrix Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, _graphicsDevice.Viewport.AspectRatio, 1, 10);

            Matrix View = Matrix.CreateLookAt(new Vector3(0, 0, 2), Vector3.Zero, Vector3.Up);
            Matrix Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 4.0f / 3.0f, 1, 500);

            _graphicsDevice.SetVertexBuffer(vertices);
            _graphicsDevice.Indices = indices;

            _effect.World = World;
            _effect.View = View;
            _effect.Projection = Projection;

            //_effect.Parameters["World"].SetValue(World);
            //_effect.Parameters["View"].SetValue(View);
            //_effect.Parameters["Projection"].SetValue(Projection);

            _effect.CurrentTechnique.Passes[0].Apply();
            //_graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, number_of_vertices, 0, number_of_indices / 3);
            //_graphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, _quad.Vertices, 0, 4, _quad.Indexes, 0, 2);

            //if(spatialFormComponent != null) {
            //    _spatialName = spatialFormComponent.SpatialFormFile;

                // previously commented out
                //if(transformComponent.X >= 0 && transformComponent.Y >= 0 &&
                //    transformComponent.X < _spriteBatch.GraphicsDevice.Viewport.Width &&
                //    transformComponent.Y < _spriteBatch.GraphicsDevice.Viewport.Height)
                //{



                    //if(string.Compare("Hero", _spatialName, StringComparison.InvariantCultureIgnoreCase) == 0) {
                    //    Hero.Render(_spriteBatch, _content, transformComponent);
                    //}

                    //if(string.Compare("NPC", _spatialName, StringComparison.InvariantCultureIgnoreCase) == 0) {
                    //    NPC.Render(_spriteBatch, _content, transformComponent);
                    //}



                //}
            //}
        }



        const int number_of_vertices = 8;
        const int number_of_indices = 36;

        VertexBuffer vertices;

        void CreateCubeVertexBuffer()
        {
            VertexPositionColor[] cubeVertices = new VertexPositionColor[number_of_vertices];

            cubeVertices[0].Position = new Vector3(-1, -1, -1);
            cubeVertices[1].Position = new Vector3(-1, -1, 1);
            cubeVertices[2].Position = new Vector3(1, -1, 1);
            cubeVertices[3].Position = new Vector3(1, -1, -1);
            cubeVertices[4].Position = new Vector3(-1, 1, -1);
            cubeVertices[5].Position = new Vector3(-1, 1, 1);
            cubeVertices[6].Position = new Vector3(1, 1, 1);
            cubeVertices[7].Position = new Vector3(1, 1, -1);

            vertices = new VertexBuffer(_graphicsDevice, VertexPositionColor.VertexDeclaration, number_of_vertices, BufferUsage.WriteOnly);
            vertices.SetData<VertexPositionColor>(cubeVertices);
        }
        
        IndexBuffer indices;

        void CreateCubeIndexBuffer()
        {
            UInt16[] cubeIndices = new UInt16[number_of_indices];

            //bottom face
            cubeIndices[0] = 0;
            cubeIndices[1] = 2;
            cubeIndices[2] = 3;
            cubeIndices[3] = 0;
            cubeIndices[4] = 1;
            cubeIndices[5] = 2;

            //top face
            cubeIndices[6] = 4;
            cubeIndices[7] = 6;
            cubeIndices[8] = 5;
            cubeIndices[9] = 4;
            cubeIndices[10] = 7;
            cubeIndices[11] = 6;

            //front face
            cubeIndices[12] = 5;
            cubeIndices[13] = 2;
            cubeIndices[14] = 1;
            cubeIndices[15] = 5;
            cubeIndices[16] = 6;
            cubeIndices[17] = 2;

            //back face
            cubeIndices[18] = 0;
            cubeIndices[19] = 7;
            cubeIndices[20] = 4;
            cubeIndices[21] = 0;
            cubeIndices[22] = 3;
            cubeIndices[23] = 7;

            //left face
            cubeIndices[24] = 0;
            cubeIndices[25] = 4;
            cubeIndices[26] = 1;
            cubeIndices[27] = 1;
            cubeIndices[28] = 4;
            cubeIndices[29] = 5;

            //right face
            cubeIndices[30] = 2;
            cubeIndices[31] = 6;
            cubeIndices[32] = 3;
            cubeIndices[33] = 3;
            cubeIndices[34] = 6;
            cubeIndices[35] = 7;

            indices = new IndexBuffer(_graphicsDevice, IndexElementSize.SixteenBits, number_of_indices, BufferUsage.WriteOnly);
            indices.SetData<UInt16>(cubeIndices);
        }
    }
}
