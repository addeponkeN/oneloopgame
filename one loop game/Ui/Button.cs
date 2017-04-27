using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace one_loop_game
{
    class Button : Properties
    {
        public Building building;
        public SpriteFont Font { get; set; }
        //Vector2 textPosition { get { return new Vector2(Position.X + (Size.X * 0.5f) - textSize, Position.Y,)} }
        Color textColor;
        public Texture2D textureInside, boxbox;
        public Vector2 textureInsideSize;
        public Rectangle textureInsideRec { get { return new Rectangle((int)Position.X/* + (int)(Size.X / 2) - (textureInside.Width / 2)*/, (int)Position.Y/* + (int)(Size.Y / 2) - (textureInside.Height / 2)*/, (int)textureInsideSize.X, (int)textureInsideSize.Y); } }
        public Rectangle textureInsideSourceRec;
        public Color textureInsideColor;
        public Color baseColor = new Color(50, 150, 250);
        Sprite outline;
        public string text;

        public bool ifOutline = true;

        public bool textBoxActive;

        bool isTextureInside;
        bool isTextureTextInside;

        /// <summary>
        /// BUTTON with TEXT <see cref="Button"/> class.
        /// </summary>
        public Button(Vector2 size, Vector2 position, Color btnColor, string text, Color textColor, SpriteFont font)
        {
            this.Font = font;
            Position = position;
            Size = size;
            baseColor = btnColor;
            this.text = text;
            this.textColor = textColor;
        }

        /// <summary>
        /// BUTTON with TEXTURE <see cref="Button"/> class.
        /// </summary>
        public Button(Vector2 size, Vector2 position, Color btnColor, Texture2D textureInside, Color textureInsideColor, Vector2 textureInsideSize)
        {
            Position = position;
            Size = size;
            baseColor = btnColor;
            this.textureInside = textureInside;
            this.textureInsideColor = textureInsideColor;
            this.textureInsideSize = textureInsideSize;
            isTextureInside = true;
            ifOutline = false;
        }

        /// <summary>
        /// BUTTON with TEXTURE && BUILDING <see cref="Button"/> class.
        /// </summary>
        public Button(Vector2 size, Vector2 position, Color btnColor, Texture2D textureInside, Color textureInsideColor, Vector2 textureInsideSize, Texture2D boxbox, BuildingType type)
        {
            building = new Building(type, textureInside, position, Texture, boxbox, true);
            Position = position;
            Size = size;
            baseColor = btnColor;
            this.boxbox = boxbox;
            this.textureInside = textureInside;
            this.textureInsideColor = textureInsideColor;
            this.textureInsideSize = textureInsideSize;
            isTextureInside = true;
            ifOutline = false;
        }

        /// <summary>
        /// BUTTON with TEXT and TEXTURE <see cref="Button"/> class.
        /// </summary>
        public Button(Vector2 size, Vector2 position, Color btnColor, Texture2D textureInside, Color textureInsideColor, Vector2 textureInsideSize, string text, Color textColor, SpriteFont font)
        {
            this.Font = font;
            Position = position;
            Size = size;
            baseColor = btnColor;
            this.text = text;
            this.textColor = textColor;
            this.textureInside = textureInside;
            this.textureInsideColor = textureInsideColor;
            this.textureInsideSize = textureInsideSize;
            isTextureTextInside = true;
        }

        public void Load(ContentManager content)
        {
            Texture = content.Load<Texture2D>("box");
            outline = new Sprite(Texture) { Position = new Vector2(Position.X - 1, Position.Y - 1), Size = new Vector2(Size.X + 2, Size.Y + 2), Color = Color.Black };
        }
        public void Update(GameTime gameTime)
        {
            if (ifOutline)
            {
                if (Rectangle.Contains(Input.mPos) && Input.LeftHold())
                {
                    var clickColor = new Color(45, 45, 45);
                    outline.Size = new Vector2(Size.X + 3, Size.Y + 3);
                    Color = clickColor;
                }
                else if (Rectangle.Contains(Input.mPos))
                {
                    var hoverColor = new Color(110, 110, 110);
                    Color = hoverColor;
                    outline.Color = new Color(outline.Color, 150);
                }
                else
                {
                    outline.Size = new Vector2(Size.X + 2, Size.Y + 2);
                    outline.Color = new Color(outline.Color, 255);
                    Color = baseColor;
                }

                //if (Rectangle.Contains(Input.mPos) && !Input.LeftHold())
                //    Color = baseColor;
            }
            else
            {
                if (Rectangle.Contains(Input.mPos) && Input.LeftHold())
                    textureInsideColor = Color.DeepSkyBlue;
                else
                    textureInsideColor = baseColor;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {

            if (ifOutline)
                spriteBatch.Draw(outline.Texture, outline.Rectangle, outline.Color);

            spriteBatch.Draw(Texture, Rectangle, Color);

            if (isTextureInside)
                spriteBatch.Draw(textureInside, textureInsideRec, textureInsideSourceRec, textureInsideColor);

            if (isTextureTextInside || !isTextureInside)
            {
                var textSize = Font.MeasureString(text);
                spriteBatch.DrawString(Font, text, new Vector2(
                    Position.X + (Size.X * 0.5f) - (textSize.X * 0.5f),
                    Position.Y + (Size.Y * 0.5f) - (textSize.Y * 0.5f)),
                    textColor);
            }
        }
    }
}