﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroid_Belt_Assault
{
    class AsteroidManager
    {
        private int screenWidth = 800;
        private int screenHeight = 600;
        private int screenPadding = 10;

        private Rectangle initialFrame;
        private int asteroidFrames;
        private Texture2D texture;

        public List<Sprite> Asteroids = new List<Sprite>();
        private int minSpeed = 60;
        private int maxSpeed = 120;

        private Random rand = new Random();

        public void AddAsteroid()
        {
            Sprite newAsteroid = new Sprite(new Vector2(-500, -500), texture, initialFrame, Vector2.Zero);
            for (int x=1;x<asteroidFrames;++x)
            {
                newAsteroid.AddFrame(new Rectangle(initialFrame.X + (initialFrame.Width * x), initialFrame.Y,
                    initialFrame.Width, initialFrame.Height));
            }
            newAsteroid.Rotation = MathHelper.ToRadians(rand.Next(360));
            newAsteroid.CollisionRadius = 15;
            Asteroids.Add(newAsteroid);
        }

        public void Clear()
        {
            Asteroids.Clear();
        }

        public AsteroidManager(int asteroidCount,Texture2D texture,Rectangle initialFrame, int asteroidFrames,int screenWidth, int screenHeight)
        {
            this.texture = texture;
            this.initialFrame = initialFrame;
            this.asteroidFrames = asteroidFrames;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            for (int i = 0; i < asteroidCount; ++i)
                AddAsteroid();
        }

        private Vector2 RandomLocation()
        {
            Vector2 location = Vector2.Zero;
            bool locationOK = true;
            int tryCount = 0;

            do
            {
                locationOK = true;
                switch (rand.Next(3))
                {
                    case 0:
                        location.X = -initialFrame.Width;
                        location.Y = rand.Next(screenHeight);
                        break;
                    case 1:
                        location.X = screenWidth;
                        location.Y = rand.Next(screenHeight);
                        break;
                    case 2:
                        location.X = rand.Next(screenWidth);
                        location.Y = -initialFrame.Height;
                        break;
                }
                foreach (Sprite asteroid in Asteroids)
                {
                    if (asteroid.IsBoxColliding(new Rectangle((int)location.X, (int)location.Y, initialFrame.Width, initialFrame.Height)))
                        locationOK = false;
                }
                ++tryCount;
                if ((tryCount > 5) && !locationOK)
                {
                    location = new Vector2(-500, -500);
                    locationOK = true;
                }
            } while (!locationOK);

            return location;
        }

        private Vector2 RandomVelocity()
        {
            Vector2 velocity=new Vector2(rand.Next(-50,51),rand.Next(-50,51));
            velocity.Normalize();
            velocity *= rand.Next(minSpeed, maxSpeed);
            return velocity;
        }

        private bool IsOnScreen(Sprite asteroid)
        {
            return asteroid.Destination.Intersects(
                new Rectangle(-screenPadding,-screenPadding,screenPadding+screenWidth,screenPadding+screenHeight));
        }

        public void Update(GameTime gameTime)
        {
            foreach(Sprite asteroid in Asteroids)
            {
                asteroid.Update(gameTime);
                if (!IsOnScreen(asteroid))
                {
                    asteroid.Location = RandomLocation();
                    asteroid.Velocity = RandomVelocity();
                }
            }

            for (int x = 0; x < Asteroids.Count; ++x)
                for (int y = x + 1; y < Asteroids.Count; ++y)
                    if (Asteroids[x].IsCircleColliding(Asteroids[y].Center, Asteroids[y].CollisionRadius))
                        BounceAsteroids(Asteroids[x], Asteroids[y]);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(Sprite asteroid in Asteroids)
            {
                asteroid.Draw(spriteBatch);
            }
        }

        private void BounceAsteroids(Sprite asteroid1, Sprite asteroid2)
        {
            Vector2 cOfMass = (asteroid1.Velocity + asteroid2.Velocity) / 2;

            Vector2 normal1 = asteroid2.Center - asteroid1.Center;
            normal1.Normalize();
            Vector2 normal2 = asteroid1.Center - asteroid2.Center;
            normal2.Normalize();

            asteroid1.Velocity -= cOfMass;
            asteroid1.Velocity = Vector2.Reflect(asteroid1.Velocity, normal1);
            asteroid1.Velocity += cOfMass;

            asteroid2.Velocity -= cOfMass;
            asteroid2.Velocity = Vector2.Reflect(asteroid2.Velocity, normal2);
            asteroid2.Velocity += cOfMass;
        }
    }
}
