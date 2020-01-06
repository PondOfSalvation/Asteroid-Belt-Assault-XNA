using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroid_Belt_Assault
{
    class Sprite
    {
        public Texture2D Texture;
        protected List<Rectangle> frames = new List<Rectangle>();
        private int frameWidth = 0;
        private int frameHeight = 0;
        private int currentFrame;
        private float frameTime = 0.1f;
        private float timeForCurrentFrame = 0.0f;

        public Color TintColor { get; set; } = Color.White;
        private float rotation = 0.0f;
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value % MathHelper.TwoPi; }
        }

        public int CollisionRadius = 0;
        public int BoundingXPadding = 0;
        public int BoundingYPadding = 0;

        public Vector2 Location { get; set; } = Vector2.Zero;
        public Vector2 Velocity { get; set; } = Vector2.Zero;

        public int Frame
        {
            get => currentFrame;
            set => currentFrame = (int)MathHelper.Clamp(value, 0, frames.Count - 1);
        }
        public float FrameTime
        {
            get => frameTime;
            set => frameTime = MathHelper.Max(0, value);
        }
        public Rectangle Source
        {
            get => frames[currentFrame];
        }
        public Rectangle Destination
        {
            get => new Rectangle((int)Location.X, (int)Location.Y, frameWidth, frameHeight);
        }
        public Vector2 Center
        {
            get => Location + new Vector2((float)frameWidth / 2, (float)frameHeight / 2);
        }

        public Rectangle BoundingBoxRect
        {
            get => new Rectangle
                ( (int)Location.X + BoundingXPadding, (int)Location.Y + BoundingYPadding,
                frameWidth - 2 * BoundingXPadding, frameHeight - 2 * BoundingYPadding );
        }
        public bool IsBoxColliding(Rectangle otherBox)
        {
            return BoundingBoxRect.Intersects(otherBox);
        }
        public bool IsCircleColliding(Vector2 otherCenter, float otherRadius)
        {
            return (Vector2.Distance(Center, otherCenter) < (CollisionRadius + otherRadius));
        }

        public Sprite(Vector2 location, Texture2D texture, Rectangle initialFrame, Vector2 velocity)
        {
            Location = location;
            Texture = texture;
            Velocity = velocity;

            frames.Add(initialFrame);
            frameWidth = initialFrame.Width;
            frameHeight = initialFrame.Height;
        }
    }
}
