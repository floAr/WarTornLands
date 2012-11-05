using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.World.Layers;
using Microsoft.Xna.Framework;
using WarTornLands;
using WarTornLandsRefurbished.Entities;
using WarTornLands.Entities;

namespace WarTornLandsRefurbished.World
{

    /*
     *Level enthält 9 Layer:
     *UNTEN
     * Layer0: Bodentextur   | wird im Editor erstellt
     *  TileBlendLayer       | wird aus Layer0 im Programm erzeugt
     * Layer1: Animation     | wird im Editor erstellt (optional)
     * Layer2: LowFoliage    | wird im Editor erstellt (optional)
     * LowEntities           | werden im Editor oder per Script plaziert
     * Spieler               | 
     * HighEntities          | werden im Editor oder per Script 
     * Layer3: HighFoliage   | wird im Editor erstellt (optional)
     * Layer4: HighAnimation | wird im Editor erstellt (optional)
     *OBEN
     *
     * LayerX: Collision     
     */
    public class Level : GameComponent
    {
        private Layer _layer0_Ground;
        private Layer _layer1_AnimGround;
        private Layer _layer2_LowFoliage;
        private Layer _layer3_LowEntities;
        private Layer _layer4_HighEntities;
        private Layer _layer5_HighFoliage;
        private Layer _layer6_HighAnimation;
        private Layer _layer7_Ceiling;

        private Layer _layerX_Collision;

        public Level(Game game)
            : base(game)
        {
            _layer0_Ground = new TileLayer(game);
            _layer1_AnimGround = new TileLayer(game);
            _layer2_LowFoliage = new TileLayer(game);

            _layer3_LowEntities = new DynamicsLayer(game);
            _layer4_HighEntities = new DynamicsLayer(game);

            _layer5_HighFoliage = new TileLayer(game);
            _layer6_HighAnimation = new TileLayer(game);
            _layer7_Ceiling = new TileLayer(game);

            //_layerX_Collision = ?
        }

        public void AddGround(int[,] data)
        {
            (_layer0_Ground as TileLayer).LoadGrid(data, false, "");
        }

        public void AddAnimGround(int[,] data)
        {
            (_layer1_AnimGround as TileLayer).LoadGrid(data, true, "");
        }

        public void AddLowFoliage(int[,] data)
        {
            (_layer2_LowFoliage as TileLayer).LoadGrid(data, false, "");
        }

        public void AddLowEntity(Entity entity)
        {
            (_layer3_LowEntities as DynamicsLayer).AddEntity(entity);
        }

        public void AddLowEntityRange(List<Entity> entities)
        {
            (_layer3_LowEntities as DynamicsLayer).AddRange(entities);
        }

        public void AddHighEntity(Entity entity)
        {
            (_layer4_HighEntities as DynamicsLayer).AddEntity(entity);
        }

        public void AddHighEntityRange(List<Entity> entities)
        {
            (_layer4_HighEntities as DynamicsLayer).AddRange(entities);
        }

        public void AddHighFoliage(int[,] data)
        {
            (_layer5_HighFoliage as TileLayer).LoadGrid(data, false, "");
        }

        public void AddHighAnimation(int[,] data)
        {
            (_layer6_HighAnimation as TileLayer).LoadGrid(data, true, "");
        }

        public void Draw(GameTime gameTime)
        {
            Vector2 center = (Game as Game1).Player.Position;

            _layer0_Ground.Draw(gameTime);
            _layer1_AnimGround.Draw(gameTime);
            _layer2_LowFoliage.Draw(gameTime);

            _layer3_LowEntities.Draw(gameTime);

            (Game as Game1).Player.Draw(gameTime);

            _layer4_HighEntities.Draw(gameTime);

            _layer5_HighFoliage.Draw(gameTime);
            _layer6_HighAnimation.Draw(gameTime);
            _layer7_Ceiling.Draw(gameTime);
        }

        public bool IsPositionAccessible(Vector2 position, Entity source)
        {
            Entity entityAt = (_layer3_LowEnitites as DynamicsLayer).GetEntityAt(position);

            return
            ((entityAt.Equals(source)) || (entityAt == null))
            // && Collision layer says yes
            ;
        }

        public Entity GetEntityAt(Vector2 worldPosition)
        {
            return (_layer3_LowEnitites as DynamicsLayer).GetEntityAt(worldPosition);
        }
    }
}
