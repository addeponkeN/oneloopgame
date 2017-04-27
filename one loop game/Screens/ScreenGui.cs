using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace one_loop_game
{
    public class ScreenGui
    {

        public GuiPanel panelBot;
        public GuiPanel panelTop;

        Sprite foodIcon, woodIcon, stoneIcon, unitIcon, buildingIcon;
        //Sprite panelBotBoxbox;
        Sprite panelBotBox, panelBotBoxOut;

        SpriteFont font;

        Texture2D panelBotTex;
        Texture2D panelTopTex;
        Texture2D resourceSheet;

        public bool showBuildingButtons;
        public bool showUnitButtons;

        //Button btnHouse;

        public TBType currentTB;
        TargetButton button1, button2, button3, button4, button5, button6, button7, button8;

        List<Sprite> sprites = new List<Sprite>();
        List<TargetButton> tButtons = new List<TargetButton>();

        //int workerCD = 2000;

        public Unit currentUnit;
        public Building currentBuilding;
        public Tile currentTile;
        private Rectangle currentTargetRectangle;
        private Vector2 currentTargetTextPosition;
        Vector2 btnPos;

        #region fix later
        int workerCD = 400;

        #endregion

        public ScreenGui()
        {

        }
        public void Load(ContentManager content, ScreenPlaying playing)
        {
            panelBotTex = content.Load<Texture2D>("uiPanelBot");
            panelTopTex = content.Load<Texture2D>("UiPanelTop");
            resourceSheet = content.Load<Texture2D>("resourceSheet");
            font = content.Load<SpriteFont>("uiFont");

            panelBot = new GuiPanel(panelBotTex, new Vector2(0, Globals.screenY - panelBotTex.Height + 32), new Vector2(Globals.screenX, panelBotTex.Height - 32));
            panelTop = new GuiPanel(panelTopTex, new Vector2(0, 0), new Vector2(Globals.screenX, 32));

            foodIcon = new Sprite(resourceSheet)
            {
                row = 0,
                Position = new Vector2(panelTop.Position.X + (32 * 1), panelTop.Position.Y + 2),
                Size = new Vector2(28)
            };
            sprites.Add(foodIcon);
            woodIcon = new Sprite(resourceSheet)
            {
                column = 1,
                Position = new Vector2(panelTop.Position.X + (32 * 4), panelTop.Position.Y + 2),
                Size = new Vector2(28)
            };
            sprites.Add(woodIcon);
            stoneIcon = new Sprite(resourceSheet)
            {
                column = 2,
                Position = new Vector2(panelTop.Position.X + (32 * 7), panelTop.Position.Y + 2),
                Size = new Vector2(28)
            };
            sprites.Add(stoneIcon);
            unitIcon = new Sprite(playing.unitSheet)
            {
                Position = new Vector2(panelTop.Position.X + (32 * 10), panelTop.Position.Y + 2),
                Size = new Vector2(28)
            };
            sprites.Add(unitIcon);
            buildingIcon = new Sprite(playing.buildingSheet96)
            {
                SourceSize = 96,
                column = 3,
                row = 1,
                Position = new Vector2(panelTop.Position.X + (32 * 13), panelTop.Position.Y + 2),
                Size = new Vector2(28)
            };
            sprites.Add(buildingIcon);

            button1 = new TargetButton(playing.box, Vector2.One, 0, 0, 0, TBType.None);
            button2 = new TargetButton(playing.box, Vector2.One, 0, 0, 0, TBType.None);
            button3 = new TargetButton(playing.box, Vector2.One, 0, 0, 0, TBType.None);
            button4 = new TargetButton(playing.box, Vector2.One, 0, 0, 0, TBType.None);


            Vector2 panelPos = panelBot.Position;

            //Console.WriteLine(panelBot.Position.Y);
            currentTargetRectangle = new Rectangle((int)panelBot.Position.X + 256 + 128, (int)panelBot.Position.Y + 48, 64, 64);

            panelBotBoxOut = new Sprite(playing.box) { Color = new Color(25, 25, 25, 80), Position = new Vector2(panelBot.Position.X + 28, panelBot.Position.Y + 28), Size = new Vector2(205, 104) };
            sprites.Add(panelBotBoxOut);
            panelBotBox = new Sprite(playing.box) { Color = new Color(30, 30, 50, 30), Position = new Vector2(panelBot.Position.X + 30, panelBot.Position.Y + 30), Size = new Vector2(204, 103) };
            sprites.Add(panelBotBox);
            currentTargetTextPosition = new Vector2(currentTargetRectangle.X + currentTargetRectangle.Width + 4, currentTargetRectangle.Y);

        }
        public void ClearTButtons()
        {
            currentTB = TBType.None;
            tButtons.Clear();
        }
        public void GetWorkerButtons(ScreenPlaying playing)
        {
            ClearTButtons();
            currentTB = TBType.Worker;
            int distance = 0;
            Vector2 btnPos = new Vector2(panelBotBox.Position.X + (distance * 48) + (2 * distance) + 2, panelBotBox.Position.Y + 2);
            button1 = new TargetButton(playing.buildingSheet64, btnPos, 0, 3, 64, TBType.Worker);
            tButtons.Add(button1);

            distance++;
            btnPos = new Vector2(panelBotBox.Position.X + (distance * 48) + (2 * distance) + 2, panelBotBox.Position.Y + 2);
            button2 = new TargetButton(playing.buildingSheet96, btnPos, 1, 3, 96, TBType.Worker);
            tButtons.Add(button2);

            distance++;
            btnPos = new Vector2(panelBotBox.Position.X + (distance * 48) + (2 * distance) + 2, panelBotBox.Position.Y + 2);
            button3 = new TargetButton(playing.buildingSheet64, btnPos, 1, 3, 64, TBType.Worker);
            tButtons.Add(button3);

            distance++;
            btnPos = new Vector2(panelBotBox.Position.X + (distance * 48) + (2 * distance) + 2, panelBotBox.Position.Y + 2);
            button4 = new TargetButton(playing.buildingSheet96, btnPos, 0, 3, 96, TBType.Worker);
            tButtons.Add(button4);

        }
        public void GetTownCenterButtons(ScreenPlaying playing)
        {
            ClearTButtons();
            currentTB = TBType.TownCenter;
            int distance = 0;
            Vector2 btnPos = new Vector2(panelBotBox.Position.X + (distance * 48) + (2 * distance), panelBotBox.Position.Y + 2);
            button1 = new TargetButton(playing.unitSheet, btnPos, 0, 0, 32, TBType.TownCenter);
            tButtons.Add(button1);
        }
        public void GetbarracksButtons(ScreenPlaying playing)
        {
            ClearTButtons();
            currentTB = TBType.Barracks;
            int distance = 0;
            Vector2 btnPos = new Vector2(panelBotBox.Position.X + (distance * 48) + (2 * distance), panelBotBox.Position.Y + 2);
            button1 = new TargetButton(playing.unitSheet, btnPos, 4, 0, 32, TBType.Barracks);
            tButtons.Add(button1);
        }

        public void Update(GameTime gameTime, ScreenPlaying playing, GraphicsDevice gd)
        {
            foreach (var b in tButtons)
                b.Update(gameTime);

            #region top panel updat eposition
            panelTop.Position = new Vector2(Globals.screenRectangle.X, Globals.screenRectangle.Y);
            woodIcon.Position = new Vector2(panelTop.Position.X + (32 * 1), panelTop.Position.Y + 2);
            foodIcon.Position = new Vector2(panelTop.Position.X + (32 * 4), panelTop.Position.Y + 2);
            stoneIcon.Position = new Vector2(panelTop.Position.X + (32 * 7), panelTop.Position.Y + 2);
            unitIcon.Position = new Vector2(panelTop.Position.X + (32 * 10), panelTop.Position.Y + 2);
            buildingIcon.Position = new Vector2(panelTop.Position.X + (32 * 13), panelTop.Position.Y + 2);
            #endregion

            #region bot panel update poisition
            panelBot.Position = new Vector2(Globals.screenRectangle.X, Globals.screenRectangle.Y + Globals.screenRectangle.Height - 160);
            panelBotBoxOut.Position = new Vector2(panelBot.Position.X + 28, panelBot.Position.Y + 28);
            panelBotBox.Position = new Vector2(panelBot.Position.X + 30, panelBot.Position.Y + 30);
            if (button1 != null && button2 != null && button3 != null && button4 != null)
            {
                button1.Position = new Vector2(panelBotBox.Position.X + (0 * 48) + (2 * 0) + 2, panelBotBox.Position.Y + 2);
                button2.Position = new Vector2(panelBotBox.Position.X + (1 * 48) + (2 * 1) + 2, panelBotBox.Position.Y + 2);
                button3.Position = new Vector2(panelBotBox.Position.X + (2 * 48) + (2 * 2) + 2, panelBotBox.Position.Y + 2);
                button4.Position = new Vector2(panelBotBox.Position.X + (3 * 48) + (2 * 3) + 2, panelBotBox.Position.Y + 2);
            }
            currentTargetRectangle = new Rectangle((int)panelBot.Position.X + 256 + 128, (int)panelBot.Position.Y + 48, 64, 64);
            currentTargetTextPosition = new Vector2(currentTargetRectangle.X + currentTargetRectangle.Width + 4, currentTargetRectangle.Y);
            #endregion

            //fix later
            if (workerCD >= 0)
                workerCD--;

            if (currentBuilding != null)
            {
                if (currentBuilding.Type == BuildingType.TownCenter)
                {
                    GetTownCenterButtons(playing);
                    if (Input.LeftClickInside(button1.Rectangle) && playing.player.maxUnits > playing.player.currentUnits)
                    {
                        if (playing.player.food >= 50)
                        {
                            playing.eManager.AddUnit(new Unit(UnitType.Worker, playing.unitSheet, new Vector2(currentBuilding.Position.X - 32, currentBuilding.Position.Y), playing.box, playing.boxbox));
                            workerCD = 400;
                            playing.player.food -= 50;
                        }
                    }
                }
                if (currentBuilding.Type == BuildingType.Barracks)
                {
                    GetbarracksButtons(playing);
                    if (playing.player.food <= 75 && playing.player.wood <= 50)
                        button1.Color = Color.PaleVioletRed;
                    else button1.Color = Color.White;
                    if (Input.LeftClickInside(button1.Rectangle) && playing.player.maxUnits > playing.player.currentUnits)
                    {
                        if (playing.player.food >= 75 && playing.player.wood >= 50)
                        {
                            playing.eManager.AddUnit(new Unit(UnitType.Warrior, playing.unitSheet, new Vector2(currentBuilding.Position.X - 32, currentBuilding.Position.Y), playing.box, playing.boxbox));
                            workerCD = 400;
                            playing.player.food -= 75;
                            playing.player.wood -= 50;
                        }
                    }
                }
            }
            if (currentUnit != null)
            {
                if (currentUnit.Type == UnitType.Worker)
                {
                    if (currentTB != TBType.Worker)
                        GetWorkerButtons(playing);

                    if (Input.LeftClickInside(button1.Rectangle))
                    {
                        playing.player.bTemplate = new Building(BuildingType.House, playing.buildingSheet64, Input.mPos, playing.box, playing.boxbox, true) { Color = new Color(230, 230, 230, 0.75f) };
                    }
                    if (Input.LeftClickInside(button2.Rectangle))
                    {
                        playing.player.bTemplate = new Building(BuildingType.TownCenter, playing.buildingSheet96, Input.mPos, playing.box, playing.boxbox, true) { Color = new Color(230, 230, 230, 0.75f) };
                    }
                    if (Input.LeftClickInside(button3.Rectangle))
                    {
                        playing.player.bTemplate = new Building(BuildingType.Storage, playing.buildingSheet64, Input.mPos, playing.box, playing.boxbox, true) { Color = new Color(230, 230, 230, 0.75f) };
                    }
                    if (Input.LeftClickInside(button4.Rectangle))
                    {
                        playing.player.bTemplate = new Building(BuildingType.Barracks, playing.buildingSheet96, Input.mPos, playing.box, playing.boxbox, true) { Color = new Color(230, 230, 230, 0.75f) };
                    }
                }
            }

            if (currentUnit == null && currentBuilding == null)
                ClearTButtons();
        }

        public void ClearTargets()
        {
            currentUnit = null;
            currentBuilding = null;
            currentTile = null;
        }

        public void Draw(SpriteBatch sb, ScreenPlaying playing)
        {
            panelBot.Draw(sb);
            panelTop.Draw(sb);

            var row = 0;
            if (currentUnit != null)
            {
                sb.Draw(currentUnit.Texture, currentTargetRectangle, currentUnit.SourceRectangle, Color.White);
                row = 0;
                sb.DrawString(font, $"{currentUnit.Type}", new Vector2(currentTargetTextPosition.X + 6, currentTargetTextPosition.Y + (row * 12)), Color.Black);

                row = 1;
                sb.DrawString(font, $"HP: {currentUnit.hp}/{currentUnit.maxHp}", new Vector2(currentTargetTextPosition.X, currentTargetTextPosition.Y + (row * 12)), Color.Black);

                row = 2;
                sb.DrawString(font, $"Damage: {currentUnit.damage}", new Vector2(currentTargetTextPosition.X, currentTargetTextPosition.Y + (row * 12)), Color.Black);

                row = 3;
                sb.DrawString(font, $"{currentUnit.RType}: {currentUnit.bagSpace}", new Vector2(currentTargetTextPosition.X, currentTargetTextPosition.Y + (row * 12)), Color.Black);

                //row = 4;
                //sb.DrawString(font, $"{currentUnit.State}", new Vector2(currentTargetTextPosition.X, currentTargetTextPosition.Y + (row * 12)), Color.Black);


            }
            if (currentBuilding != null)
            {
                sb.Draw(currentBuilding.Texture, currentTargetRectangle, currentBuilding.SourceRectangle, Color.White);
                row = 0;
                sb.DrawString(font, $"{currentBuilding.Type}", new Vector2(currentTargetTextPosition.X + 6, currentTargetTextPosition.Y + (row * 12)), Color.Black);

                row = 1;
                sb.DrawString(font, $"HP: {currentBuilding.hp}/{currentBuilding.maxHp}", new Vector2(currentTargetTextPosition.X, currentTargetTextPosition.Y + (row * 12)), Color.Black);
                if (!currentBuilding.built)
                {
                    row = 2;
                    sb.DrawString(font, $"Progress: {(int)((currentBuilding.buildProgress / currentBuilding.buildTime) * 100)}%", new Vector2(currentTargetTextPosition.X, currentTargetTextPosition.Y + (row * 12)), Color.Black);

                    row = 3;
                    sb.DrawString(font, $"{currentBuilding.buildProgress}/{currentBuilding.buildTime}", new Vector2(currentTargetTextPosition.X, currentTargetTextPosition.Y + (row * 12)), Color.Black);
                }
            }
            if (currentTile != null)
            {
                sb.Draw(currentTile.Texture, currentTargetRectangle, currentTile.SourceRectangle, Color.White);
                row = 0;
                sb.DrawString(font, $"{currentTile.Type}", new Vector2(currentTargetTextPosition.X + 6, currentTargetTextPosition.Y + (row * 12)), Color.Black);

                row = 1;
                sb.DrawString(font, $"Value: {currentTile.Value}", new Vector2(currentTargetTextPosition.X, currentTargetTextPosition.Y + (row * 12)), Color.Black);

                //row = 2;
                //sb.DrawString(font, $"P: {currentTile.Point}", new Vector2(currentTargetTextPosition.X, currentTargetTextPosition.Y + (row * 12)), Color.Black);

                //row = 3;
                //sb.DrawString(font, $"row: {currentTile.row} | column: {currentTile.column}", new Vector2(currentTargetTextPosition.X, currentTargetTextPosition.Y + (row * 12)), Color.Black);
            }

            foreach (var s in sprites)
                s.DrawSheet(sb);


            foreach (var b in tButtons)
            {
                b.DrawTB(sb, currentTB);
            }

            //Vector2 resS = new Vector2(foodIcon.Position.X + foodIcon.Size.X, foodIcon.Position.Y);
            sb.DrawString(font, "" + playing.player.food, new Vector2(foodIcon.Position.X + (foodIcon.Size.X + 4), foodIcon.Position.Y + 4), Color.Black);
            sb.DrawString(font, "" + playing.player.wood, new Vector2(woodIcon.Position.X + (woodIcon.Size.X + 4), woodIcon.Position.Y + 4), Color.Black);
            sb.DrawString(font, "" + playing.player.stone, new Vector2(stoneIcon.Position.X + (stoneIcon.Size.X + 4), stoneIcon.Position.Y + 4), Color.Black);

            sb.DrawString(font, $"{playing.player.currentUnits}/{playing.player.maxUnits}", new Vector2(unitIcon.Position.X + (unitIcon.Size.X + 4), unitIcon.Position.Y + 4), Color.Black);
            sb.DrawString(font, $"{playing.player.currentBuildings}", new Vector2(buildingIcon.Position.X + (buildingIcon.Size.X + 4), buildingIcon.Position.Y + 4), Color.Black);

            //sb.Draw(playing.box, new Rectangle((int)Input.mPos.X, (int)Input.mPos.Y, 10, 10), Color.MediumVioletRed);
        }


    }
}
