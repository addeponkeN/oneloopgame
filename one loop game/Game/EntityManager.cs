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
    public enum EntityType
    {
        Building,
        Unit,

    }

    public class EntityManager
    {
        public List<Building> buildings = new List<Building>();
        public List<Unit> units = new List<Unit>();
        public List<Entity> list = new List<Entity>();
        public List<Unit> idleUnit = new List<Unit>();

        public void AddUnit(Unit u)
        {
            units.Add(u);
            list.Add(u);
        }
        public void AddBuildingNotInit(Building b)
        {
            buildings.Add(b);
            list.Add(b);
        }
        public void AddBuilding(Building b, ScreenPlaying p)
        {
            buildings.Add(b);
            list.Add(b);
            p.tileManager.CheckABuildingIfOnTileAndInitNodes(b, p);
        }

        public void Update(GameTime gameTime, ScreenPlaying playing)
        {
            foreach (var u in units)
            {
                u.Update(gameTime, playing);

                //if(u.Targeted)
                //{
                //    playing.gui.currentTB = TBType.Worker;
                //}
                //else playing.gui.currentTB = TBType.None;

            }
            foreach (var b in buildings)
            {
                b.Update(gameTime, playing);

                //if (b.Targeted)
                //{
                //    playing.gui.currentTB = TBType.TownCenter;
                //}
                //else playing.gui.currentTB = TBType.None;

            }
            buildings.RemoveAll(b => !b.Exist);
            units.RemoveAll(b => !b.Exist);

        }
        public void Draw(SpriteBatch sb)
        {
            foreach (var b in buildings)
            {
                b.Draw(sb);
            }
            foreach (var u in units)
            {
                u.Draw(sb);
            }

        }
    }
}