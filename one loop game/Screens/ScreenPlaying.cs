using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace one_loop_game
{
    public class ScreenPlaying
    {
        public ScreenGui gui;
        public EntityManager eManager;
        public TileManager tileManager;
        public Pathfind finder;
        public Player player;
        public Camera cam;

        public Texture2D tileSheet, box, boxbox, unitSheet, buildingSheet64, buildingSheet96;
        public SpriteFont buttonFont;

        public SoundManager sm;

        //Rectangle nRec;

        public bool loadComplete;
        bool entered;

        int prevScroll;

        public void Load(ContentManager content)
        {
            cam = new Camera();
            prevScroll = Input.m.ScrollWheelValue;
            sm = new SoundManager();
            sm.Load(content);
            eManager = new EntityManager();
            box = content.Load<Texture2D>("box");
            boxbox = content.Load<Texture2D>("boxbox");
            tileSheet = content.Load<Texture2D>("tileSheet");
            buttonFont = content.Load<SpriteFont>("buttonFont");
            unitSheet = content.Load<Texture2D>("unitSheet");
            buildingSheet64 = content.Load<Texture2D>("buildingSheet64new");
            buildingSheet96 = content.Load<Texture2D>("buildingSheet96");
            player = new Player(box, boxbox);
            gui = new ScreenGui();
            gui.Load(content, this);

            tileManager = new TileManager(tileSheet, buttonFont, 150, 150);
            tileManager.GenerateMap();
            tileManager.PlaceTiles(44, 1, this, true);
            finder = new Pathfind(this);

            //tileManager.CheckAllBuildingsIfOnTileAndInitNodes(eManager.buildings.ToArray(), this);

            cam.Pos = new Vector2(eManager.units.Last().Position.X, eManager.units.Last().Position.Y);
            //tileManager.CheckIfTreeAboveAll();
            CheckEverything();
            player.UpdateBuildingAction(this);
            loadComplete = true;
        }
        public void Enter()
        {
            cam.Pos = new Vector2(eManager.units.Last().Position.X, eManager.units.Last().Position.Y);
            sm.songForestInstance.IsLooped = true;
            sm.songForestInstance.Play();
            entered = true;
        }
        public void Exit()
        {
            sm.songForestInstance.Stop();
            entered = false;
        }
        public void InitMapNodes()
        {
            finder.InitWalkNodes(this);
        }
        public void CheckEverything()
        {
            tileManager.CheckIfTreeAboveAll();
            tileManager.CheckAllTileTypes(this);
            tileManager.CheckAllBuildingsIfOnTileAndInitNodes(eManager.buildings.ToArray(), this);
            InitMapNodes();
        }

        public void Update(GameTime gameTime, GraphicsDevice gd)
        {
            if (!entered)
                Enter();
            Input.mPos = Vector2.Transform(new Vector2(Input.m.X, Input.m.Y), Matrix.Invert(cam.get_transformation(gd)));
            if (Input.KeyClick(Keys.Escape))
            {
                Exit();
                Globals.gameState = "menu";
            }

            eManager.Update(gameTime, this);
            tileManager.Update(gameTime, this);
            player.Update(gameTime, this);
            gui.Update(gameTime, this, gd);

            Globals.screenRectangle = cam.rectangle;

            if (Input.KeyClick(Keys.Enter))
            {

            }
            if (Input.KeyClick(Keys.Space))
            {
                CheckEverything();
            }
            UpdateCamera();
        }
        public void UpdateCamera()
        {
            if (Input.KeyHold(Keys.Up) || Globals.screenTop.Contains(Input.mPos))
                cam.pos.Y -= 10;
            if (Input.KeyHold(Keys.Down) || Globals.screenBot.Contains(Input.mPos))
                cam.pos.Y += 10;
            if (Input.KeyHold(Keys.Left) || Globals.screenLeft.Contains(Input.mPos))
                cam.pos.X -= 10;
            if (Input.KeyHold(Keys.Right) || Globals.screenRight.Contains(Input.mPos))
                cam.pos.X += 10;

            if (Input.m.ScrollWheelValue < prevScroll/* && cam.Zoom > 1*/)
            {
                cam.Zoom -= (float)0.2;
                prevScroll = Input.m.ScrollWheelValue;
                Console.WriteLine(cam.Zoom + "   UT");

                //if (cam.Zoom < 1f)
                    //cam.Zoom = 1f;
            }
            else if (Input.m.ScrollWheelValue > prevScroll/* && cam.Zoom < .2f*/)
            {
                cam.Zoom += (float)0.2;
                prevScroll = Input.m.ScrollWheelValue;
                Console.WriteLine(cam.Zoom + "  IN");
                //if (cam.Zoom > 1.6f)
                    //cam.Zoom = 1.6f;
            }

            if (cam.pos.X < 500)
                cam.pos.X = 500;
            if (cam.pos.X > tileManager.mapX * 32)
                cam.pos.X = tileManager.mapX * 32;
            if (cam.pos.Y > tileManager.mapY * 32)
                cam.pos.Y = tileManager.mapY * 32;
            if (cam.pos.Y < 250)
                cam.pos.Y = 250;
        }
        public void Draw(SpriteBatch sb, GraphicsDevice graphics)
        {
            /// render with gpu - to fix

            sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default,
                    RasterizerState.CullNone, null, cam.get_transformation(graphics));

            tileManager.Draw(sb, cam);
            eManager.Draw(sb);
            player.Draw(sb);

            //sb.Draw(box, nRec, Color.MonoGameOrange);
            gui.Draw(sb, this);

            sb.End();
            sb.Begin();

            //tileManager.DrawString(sb);

            sb.End();
        }
    }
}