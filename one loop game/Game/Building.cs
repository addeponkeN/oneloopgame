using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace one_loop_game
{
    public enum BuildingType
    {
        House,
        TownCenter,
        Storage,
        Barracks

    }
    public class Building : Entity
    {
        public bool Exist => hp > 0 || !built;
        public bool building;
        public BuildingType Type;
        Bar hpBar;
        Bar progressBar;

        public Texture2D texture32, texture64, texture96, boxbox;
        public Rectangle TargetBox { get { return new Rectangle((int)Position.X - 2, (int)Position.Y - 2, SourceSize + 4, SourceSize + 4); } }

        #region gamestats
        public double hp, maxHp, percentHp, buildProgress;
        public int percentText, buildTime, costWood, costStone, action;
        public bool built = false;
        #endregion

        public bool Targeted;

        public Building(BuildingType type, Texture2D texture, Vector2 spawnPosition, Texture2D barBox, Texture2D boxbox, bool isBuilt) : base()
        {
            eType = EntityType.Building;
            Type = type;
            Position = spawnPosition;
            buildProgress = 0;
            this.boxbox = boxbox;
            switch (Type)
            {
                case BuildingType.House:
                    Texture = texture;
                    Size = new Vector2(64);
                    SourceSize = 64;
                    buildTime = 10;
                    maxHp = 30;
                    costWood = 50;
                    action = 2;

                    break;
                case BuildingType.TownCenter:
                    Texture = texture;
                    row = 1;
                    Size = new Vector2(96);
                    SourceSize = 96;
                    buildTime = 30;
                    maxHp = 100;
                    costWood = 250;
                    costStone = 64;
                    action = 5;

                    break;
                case BuildingType.Storage:
                    Texture = texture;
                    Color = Color.White;
                    Size = new Vector2(64);
                    SourceSize = 64;
                    buildTime = 15;
                    maxHp = 40;
                    costWood = 100;
                    row = 1;

                    break;
                case BuildingType.Barracks:
                    Texture = texture;
                    Size = new Vector2(96);
                    SourceSize = 96;
                    buildTime = 25;
                    maxHp = 55;
                    costWood = 150;
                    costStone = 50;

                    break;
                default:
                    break;
            }
            hpBar = new Bar(barBox, Position, Size);
            progressBar = new Bar(barBox, Position, Size)
            {
                distanceBetweenObjectY = 4,
                HpColor = Color.LightYellow,
                HpMaxColor = Color.DarkGoldenrod
            };
            if (isBuilt)
                IsBuilt();
        }
        public void IsBuilt()
        {
            column = 3;
            built = true;
            hp = maxHp;
            buildProgress = buildTime;
        }

        public override void Update(GameTime gameTime, ScreenPlaying playing)
        {
            hpBar.UpdatePercent(hp, maxHp, (int)Size.X);
            progressBar.UpdatePercent(buildProgress, buildTime, (int)Size.X);

            if (Input.KeyClick(Keys.D))
            {
                if (Rectangle.Contains(Input.mPos))
                    hp -= 20;
            }
            if (hp > maxHp)
                hp = maxHp;

            if (built)
                column = 3;
            else
            {
                percentText = (int)(buildProgress / buildTime) * 100;
                if (buildProgress >= buildTime)
                {
                    column = 3;
                    built = true;
                    hp = maxHp;
                }
                else if (buildProgress > buildTime * 0.66)
                    column = 2;
                else if (buildProgress > buildTime * 0.33)
                    column = 1;
                else column = 0;
            }

            if (Input.LeftRelease())
                if (!Rectangle.Contains(Input.mPos))
                    Targeted = false;

            base.Update(gameTime, playing);
        }

        public override void Draw(SpriteBatch sb)
        {
            // draw hp bar and progress bar
            if (hp < maxHp)
                hpBar.Draw(sb);
            else if (Targeted && built)
                hpBar.Draw(sb);
            if (Targeted)
                sb.Draw(boxbox, TargetBox, Color.Black);
            if (!built)
                progressBar.Draw(sb);

            // draw building
            sb.Draw(Texture, Rectangle, SourceRectangle, Color);

            base.Draw(sb);
        }
    }
}
