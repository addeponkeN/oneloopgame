using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace one_loop_game
{
    class SpriteManager
    {

        private float _transitionTime;
        public Vector2 Position { get; set; }
        public int[] Sequence { get; set; }
        public bool Continuous { get; set; }
        public bool Pause { get; set; }
        public int Time { get; set; }
        public Point Tile { get; set; }
        public int Index { get; set; }
        public bool IsDone { get; private set; }
        public Texture2D Texture { get; set; }
        public Vector2 Origin { get; set; }
        public float Rotation { get; set; }
        public Rectangle Destination { get; set; } = Rectangle.Empty;
        public Rectangle Source { get; set; } = Rectangle.Empty;
        public SpriteEffects SpriteEffects { get; set; } = SpriteEffects.None;
        public Color Color { get; set; } = Color.White;
        public float Depth { get; set; } = 1f;
        public float Scale { get; set; } = 1f;

        public Point Size
        {
            get
            {
                if (Texture == null)
                    return Point.Zero;

                return GetMaxFrame(Texture.Width, Tile.X, Texture.Height, Tile.Y);
            }
        }

        private bool IsSequenceNull
        {
            get { return (Sequence == null || Tile == Point.Zero); }
        }

        public void Reset()
        {
            Index = 0;
            IsDone = false;
        }

        public void Update(GameTime gameTime)
        {
            if (!IsSequenceNull && !Pause)
            {
                _transitionTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (_transitionTime > Time)
                {
                    _transitionTime = 0;
                    Index++;
                }

                if (Index > Sequence.Length - 1)
                {
                    IsDone = true;

                    if (Continuous)
                        Index = 0;
                    else
                        Index = Sequence.Length - 1;
                }
                else
                    IsDone = false;
            }
        }

        #region Private Methods

        private Point GetMaxFrame(int width, int tileX, int height, int tileY)
        {
            return new Point(width / tileX, height / tileY);
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsSequenceNull)
            {
                if (Index > Sequence.Length - 1)
                    Index = Sequence.Length - 1;

                Rectangle frameRect;
                if (Source != Rectangle.Empty)
                    frameRect = Source;
                else
                {
                    var fSequence = Sequence[Index] % Tile.X;
                    var fIndex = Sequence[Index] / Tile.X;

                    frameRect = new Rectangle(
                        fSequence * Size.X, fIndex * Size.Y, Size.X, Size.Y);
                }

                if (Destination != Rectangle.Empty)
                {
                    spriteBatch.Draw(Texture, null, Destination, frameRect, Origin,
                        Rotation, new Vector2(Scale), Color, SpriteEffects, Depth);
                }
                else
                {
                    spriteBatch.Draw(Texture, Position, frameRect, Color, Rotation, Origin, Scale,
                        SpriteEffects, Depth);
                }
            }
        }
        #endregion

        #region Public Methods

        public static int[] CreateSequence(int[] values, int increasement)
        {
            var iResult = new int[values.Length];
            var csIndex = 0;

            foreach (var value in values)
            {
                iResult[csIndex] = value + increasement;
                csIndex++;
            }

            return iResult;
        }

        public static int[] AddToSequence(int[] values, int value, int count)
        {
            var iResult = new int[count];

            for (var i = 0; i < count; i++)
                iResult[i] = value;

            return AddToInject(values, iResult);
        }

        public static int[] Inject(params int[] iParams)
        {
            return iParams;
        }

        public static int[] AddToInject(int[] values, params int[] iParams)
        {
            ////////////////////////////////////////////////////////
            // Using a faster method by copying bytes onto two
            // arrays. This is needed for special animations where
            // I need to add extra pause at the start or end of
            // an animation.
            ////////////////////////////////////////////////////////

            var newArray = new int[values.Length + iParams.Length];
            var valueBytes = values.Length * sizeof(int);
            var iParamsBytes = iParams.Length * sizeof(int);

            Buffer.BlockCopy(values, 0, newArray, 0, valueBytes);
            Buffer.BlockCopy(iParams, 0, newArray, valueBytes, iParamsBytes);

            return newArray;
        }

        #endregion

    }
}
