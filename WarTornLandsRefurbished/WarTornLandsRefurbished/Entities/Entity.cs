using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLandsRefurbished.Infrastructure.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarTornLandsRefurbished.Infrastructure.Systeme.AnimationsSystem;

namespace WarTornLandsRefurbished.Entities
{
    public class Entity
    {
        /* TODO
         * Interface als Events oder Methoden?
         * Events:
         *  Andere Klassen (Implementationen) definieren Verhalten und binden sich an 
         *  die Hooks der Entity Klasse. Events werden von außerhalb (z.B. onCollide) oder intern 
         *  (z.B. onDie) aufgerufen. 
         *      Probleme: Wie werden Daten durchgereicht? Große EventArgs?
         * 
         * Methoden:
         *  Andere Klassen sprechen direkt Methoden an, Entity entält die abstrakten Methoden
         *  (ist praktisch ein Inferface) und Implementation füllen Logik.
         *      Probleme: Wie verknpüfen wir in beide Richtungen (Spieler kennt Item-Entity
         *      aus Collision Manager, aber wie weiß das Item in wessen Inv. es soll)
         */

        /// <summary>
        /// The _draw executer for this entity.
        /// It is either a  <see cref="AnimationSystem" /> or a  <see cref="StaticDrawer" />
        /// </summary>
        IDrawExecuter _drawExecuter;


        public Entity(IDrawExecuter executer)
        {
            _drawExecuter = executer;
        }
        public void Update(GameTime gameTime)
        {
            if (_drawExecuter is AnimationSystem)
                ((AnimationSystem)_drawExecuter).Update(gameTime);
        }
        public void Draw(SpriteBatch batch, GameTime gameTime)
        {
            //TODO Calc information
            DrawInformation info = new DrawInformation();
      
            _drawExecuter.Draw(batch, info);
        }


    }
}
