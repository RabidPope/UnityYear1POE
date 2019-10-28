using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


    [Serializable]
    abstract class Building
    {
        protected int xPos;
        protected int yPos;
        protected int hP;
        protected int maxHP;
        protected int faction;
        protected char shape;

        protected Building(int xPos, int yPos, int hP, int faction, char shape)
        {
            this.xPos = xPos;
            this.yPos = yPos;
            this.hP = hP;
            this.maxHP = hP;
            this.faction = faction;
            this.shape = shape;
        }


        public abstract void Save();
        public abstract void Death();
        public abstract override string ToString();
    }

    [Serializable]
    class ResourceBuilding : Building
    {
        private string resourceType;
        protected int generated;
        private int genPerRound;
        private int remaining;
        private int max;
        private enum ResourceUsed { Wood, Steel}
        
        
       
        public int XPos { get => xPos; set => xPos = value; }
        public int YPos { get => yPos; set => yPos = value; }
        public int HP { get => hP; set => hP = value; }        
        public int Faction { get => faction; set => faction = value; }
        public char Shape { get => shape; set => shape = value; }
        public string ResourceType { get => resourceType; }
        public int GenPerRound { get => genPerRound; }
        public int Remaining { get => remaining;  }
        public int MaxHP { get => maxHP; }    

        public ResourceBuilding(int xPos, int yPos, int faction, int resourceType, int remaining, int genPerRound) : base(xPos, yPos, 20, faction, '۩')
        {
            this.resourceType = Enum.GetName(typeof(ResourceUsed), resourceType);
            generated = 0;
            this.genPerRound = genPerRound;
            this.remaining = remaining;
            max = remaining;
        }

        public void generateResource()
        {
            remaining = Math.Max(0, remaining - genPerRound);

            generated = Math.Min(max, generated + genPerRound);

        }

        public void Replenish(int amount)
        {
            remaining = remaining + amount;
        }

        public int ResourcePool()
        {
            return remaining;
        }

        public override string ToString()
        {
            string returnData;
            returnData = "Resource Building";
            return returnData;
        }

        public override void Death()
        {

        }

        public override void Save()
        {
            BinaryFormatter bFormat = new BinaryFormatter();
            using (FileStream stream = new FileStream("saves/building_save.game", FileMode.Append, FileAccess.Write))
            {
                bFormat.Serialize(stream, this);
            }
        }

    }

    [Serializable]
    class FactoryBuilding : Building
    {
        protected string unitType;
        protected int prodSpeed;
        protected int spawnPoint;
        private enum UnitType { Knight, Archer}

        public int XPos { get => xPos; set => xPos = value; }
        public int YPos { get => yPos; set => yPos = value; }
        public int HP { get => hP; set => hP = value; }
        
        public int Faction { get => faction; set => faction = value; }
        public char Shape { get => shape; set => shape = value; }

        public int ProdSpeed { get => prodSpeed; }
        public int MaxHP { get => maxHP; }

        public FactoryBuilding(int xPos, int yPos, int faction, int unitType, int spawnPoint) : base (xPos, yPos, 20, faction, '۝')
        {
            this.unitType = Enum.GetName(typeof(UnitType), unitType);
            this.prodSpeed = 1;
            this.spawnPoint = spawnPoint;
        }

        public Unit SpawnUnit()
        {
            MeleeUnit tempMUnit;
            RangedUnit tempRUnit;
            Unit tempUnit;

            if (unitType == "Knight")
            {
                tempMUnit = new MeleeUnit(xPos, spawnPoint, faction, false);
                tempUnit = tempMUnit;
            }
            else
            {
                tempRUnit = new RangedUnit(xPos, spawnPoint, faction, false);
                tempUnit = tempRUnit;
            }
            return tempUnit;
        }

        public override string ToString()
        {
            string returnData;
            returnData = "Factory Building";
            return returnData;
        }

        public override void Death()
        {

        }

        public override void Save()
        {
            BinaryFormatter bFormat = new BinaryFormatter();
            using (FileStream stream = new FileStream("saves/building_save.game", FileMode.Append, FileAccess.Write))
            {
                bFormat.Serialize(stream, this);
            }
        }
    }


