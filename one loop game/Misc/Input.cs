﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace one_loop_game
{
    class Input
    {
        public static KeyboardState k;
        public static MouseState m;
        public static KeyboardState kO;
        public static MouseState mO;

        public static Vector2 mPos;

        public static void Update(GameTime gameTime)
        {
            kO = k;
            k = Keyboard.GetState();

            mO = m;
            m = Mouse.GetState();

            //Input.mPos = Vector2.Transform(new Vector2(Input.m.X, Input.m.Y), Matrix.Invert(cam.get_transformation(gd)));
        }

        public static bool KeyClick(Keys key)
        {
            return k.IsKeyDown(key) && kO.IsKeyUp(key);
        }
        public static bool KeyHold(Keys key)
        {
            return k.IsKeyDown(key);
        }

        public static bool LeftClickInside(Rectangle rec)
        {
            return m.LeftButton == ButtonState.Pressed && mO.LeftButton == ButtonState.Released && rec.Contains(mPos);
        }
        public static bool LeftClick()
        {
            return m.LeftButton == ButtonState.Pressed && mO.LeftButton == ButtonState.Released;
        }
        public static bool LeftRelease()
        {
            return m.LeftButton == ButtonState.Released && mO.LeftButton == ButtonState.Pressed;
        }
        public static bool LeftHold()
        {
            return m.LeftButton == ButtonState.Pressed;
        }

        public static bool RightClickInside(Rectangle rec)
        {
            return m.RightButton == ButtonState.Pressed && mO.RightButton == ButtonState.Released && rec.Contains(mPos);
        }
        public static bool RightClick()
        {
            return m.RightButton == ButtonState.Pressed && mO.RightButton == ButtonState.Released;
        }
        public static bool RightRelease()
        {
            return m.RightButton == ButtonState.Released && mO.RightButton == ButtonState.Pressed;
        }
        public static bool RightHold()
        {
            return m.RightButton == ButtonState.Pressed;
        }
    }
}
