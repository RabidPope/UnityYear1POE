using System;
using UnityEngine;


    [Serializable]
    abstract class Unit : MonoBehaviour
{

        protected int xPos;
        protected int yPos;
        protected int hp;
        protected int maxHP;
        protected int speed;
        protected int attack;
        protected int range;
        protected int faction;
        protected char shape;
        protected bool attacking;
        

        public Unit(string name, int xPos, int yPos, int hp, int speed, int attack, int range, int faction, char shape, bool attacking)
        {
            this.xPos = xPos;
            this.yPos = yPos;
            this.hp = hp;
            this.maxHP = hp;
            this.speed = speed;
            this.attack = attack;
            this.range = range;
            this.faction = faction;
            this.shape = shape;
            this.attacking = attacking;
            
        }

        public abstract void Move(int[] position, int ownIndex);
        public abstract void Engage(int index, string type);
        public abstract bool CheckRange(int range);
        public abstract void FindEnemy(int ownIndex);
        public abstract void Death(int index);
        public abstract override string ToString();
        public abstract void Save();

    }

