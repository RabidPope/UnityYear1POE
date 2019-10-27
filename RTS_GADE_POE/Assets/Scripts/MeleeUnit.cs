using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


    [Serializable]
    class MeleeUnit : Unit
    {

        private enum UnitShape { X, M }
        public MeleeUnit(int xPos, int yPos, int faction, bool attacking) : base("Knight", xPos, yPos, 40, 1, 1, 1, faction, Convert.ToChar(Enum.GetName(typeof(UnitShape), faction)), attacking)
        {

        }

        public int XPos { get => xPos; set => xPos = value; }
        public int YPos { get => yPos; set => yPos = value; }
        public int Hp { get => hp; set => hp = value; }
        public int MaxHP { get => maxHP; }
        public int Speed { get => speed; set => speed = value; }
        public int Attack { get => attack; set => attack = value; }
        public int Range { get => range; set => range = value; }
        public int Faction { get => faction; set => faction = value; }
        public char Shape { get => shape; set => shape = value; }
        public bool Attacking { get => attacking; set => attacking = value; }


        public override void Move(int[] position, int ownIndex)
        {
            Random rng = new Random();

            if (GameEngine.Rounds % base.speed == 0)
            {
                double percHP = (double)base.hp / base.maxHP * 100;
                if (percHP > 25)//Hunt
                {

                    base.yPos = base.yPos + (Math.Max(-1, Math.Min(position[0] - base.yPos, 1)));
                    base.xPos = base.xPos + (Math.Max(-1, Math.Min(position[1] - base.xPos, 1)));
                }
                else//Flee
                {
                    base.yPos = Math.Max(0, Math.Min(Map.MapSizeY - 1, base.yPos + rng.Next(-1, 2)));
                    base.xPos = Math.Max(0, Math.Min(Map.MapSizeX - 1, base.xPos + rng.Next(-1, 2)));
                }
                GameManager.UnitsOnField[ownIndex] = this;
            }
        }

        public override void Engage(int index, string type)
        {

            if (type == "Unit")
            {
                MeleeUnit tempMUnit;
                RangedUnit tempRUnit;
                if (GameManager.UnitsOnField[index].ToString() == "Knight")
                {
                    tempMUnit = (MeleeUnit)GameManager.UnitsOnField[index];
                    tempMUnit.Hp = tempMUnit.Hp - base.attack;
                    GameManager.UnitsOnField[index] = tempMUnit;//Do damage to unit at index
                }
                else if (GameManager.UnitsOnField[index].ToString() == "Archer")
                {
                    tempRUnit = (RangedUnit)GameManager.UnitsOnField[index];
                    tempRUnit.Hp = tempRUnit.Hp - base.attack;
                    GameManager.UnitsOnField[index] = tempRUnit;// Do damage to unit at index
                }
                else if (GameManager.UnitsOnField[index].ToString() == "Wizard")
                {
                    WizardUnit tempWUnit = (WizardUnit)GameManager.UnitsOnField[index];
                    tempWUnit.Hp = tempWUnit.Hp - base.attack;
                    GameManager.UnitsOnField[index] = tempWUnit;// Do damage to unit at index
                }
            }
            else
            {
                ResourceBuilding tempRBuilding;
                FactoryBuilding tempFBuilding;
                if (GameManager.BuildingsOnField[index].ToString() == "Resource Building")
                {
                    tempRBuilding = (ResourceBuilding)GameManager.BuildingsOnField[index];
                    tempRBuilding.HP = tempRBuilding.HP - base.attack;
                    GameManager.BuildingsOnField[index] = tempRBuilding;
                }
                else
                {
                    tempFBuilding = (FactoryBuilding)GameManager.BuildingsOnField[index];
                    tempFBuilding.HP = tempFBuilding.HP - base.attack;
                    GameManager.BuildingsOnField[index] = tempFBuilding;
                }
            }

        }

        public override bool CheckRange(int range)
        {
            bool inRange = false;
            if (base.range >= range)
            {
                inRange = true;
            }
            return inRange;
        }

    public override void FindEnemy(int ownIndex)
    {
        MeleeUnit tempMUnit;
        RangedUnit tempRUnit;
        ResourceBuilding tempRBuilding;
        FactoryBuilding tempFBuilding;
        attacking = false;
        string type = "";
        int indexOfNearest = 0;
        int nearest = 999;
        int[] positionOfNearest = new int[] { this.yPos, this.xPos };//Store Position of nearest unit

        bool act = false;
        //Find closest building
        for (int i = 0; i < GameManager.BuildingsOnField.Length; i++)
        {
            if (GameManager.BuildingsOnField[i].ToString() == "Resource Building")
            {
                tempRBuilding = (ResourceBuilding)GameManager.BuildingsOnField[i];
                if (tempRBuilding.Faction != base.faction && tempRBuilding.HP > 0)
                {
                    act = true;
                    int xDistance = Math.Abs(base.xPos - tempRBuilding.XPos);
                    int yDistance = Math.Abs(base.yPos - tempRBuilding.YPos);
                    double trueDistance = Math.Sqrt(Math.Pow(xDistance, 2) + Math.Pow(yDistance, 2));
                    int distance = (int)Math.Round(trueDistance, 0);
                    if (distance < nearest && tempRBuilding.HP >= 0)
                    {
                        type = "Building";
                        nearest = distance;
                        indexOfNearest = i;
                        positionOfNearest[1] = tempRBuilding.XPos;
                        positionOfNearest[0] = tempRBuilding.YPos;
                    }
                }
            }
            else
            {
                tempFBuilding = (FactoryBuilding)GameManager.BuildingsOnField[i];
                if (tempFBuilding.Faction != base.faction && tempFBuilding.HP > 0)
                {
                    act = true;
                    int xDistance = Math.Abs(base.xPos - tempFBuilding.XPos);
                    int yDistance = Math.Abs(base.yPos - tempFBuilding.YPos);
                    double trueDistance = Math.Sqrt(Math.Pow(xDistance, 2) + Math.Pow(yDistance, 2));
                    int distance = (int)Math.Round(trueDistance, 0);
                    if (distance < nearest && tempFBuilding.HP >= 0)
                    {
                        type = "Building";
                        nearest = distance;
                        indexOfNearest = i;
                        positionOfNearest[1] = tempFBuilding.XPos;
                        positionOfNearest[0] = tempFBuilding.YPos;
                    }
                }
            }
        }
        //Find closest unit
        for (int i = 0; i < GameManager.UnitsOnField.Length; i++)
        {
            if (GameManager.UnitsOnField[i].ToString() == "Knight")
            {
                tempMUnit = (MeleeUnit)GameManager.UnitsOnField[i];
                if (tempMUnit.Faction != base.faction && tempMUnit.Hp > 0)
                {
                    act = true;
                    int xDistance = Math.Abs(base.xPos - tempMUnit.XPos);
                    int yDistance = Math.Abs(base.yPos - tempMUnit.YPos);
                    double trueDistance = Math.Sqrt(Math.Pow(xDistance, 2) + Math.Pow(yDistance, 2));
                    int distance = (int)Math.Round(trueDistance, 0);
                    if (distance < nearest && tempMUnit.Hp >= 0)
                    {
                        type = "Unit";
                        nearest = distance;
                        indexOfNearest = i;
                        positionOfNearest[1] = tempMUnit.XPos;
                        positionOfNearest[0] = tempMUnit.YPos;
                    }
                }
            }
            else if (GameManager.UnitsOnField[i].ToString() == "Archer")
            {
                tempRUnit = (RangedUnit)GameManager.UnitsOnField[i];
                if (tempRUnit.Faction != base.faction && tempRUnit.Hp > 0)
                {
                    act = true;
                    int xDistance = Math.Abs(base.xPos - tempRUnit.XPos);
                    int yDistance = Math.Abs(base.yPos - tempRUnit.YPos);
                    double trueDistance = Math.Sqrt(Math.Pow(xDistance, 2) + Math.Pow(yDistance, 2));
                    int distance = (int)Math.Round(trueDistance, 0);
                    if (distance < nearest && tempRUnit.Hp >= 0)
                    {
                        type = "Unit";
                        nearest = distance;
                        indexOfNearest = i;
                        positionOfNearest[1] = tempRUnit.XPos;
                        positionOfNearest[0] = tempRUnit.YPos;
                    }
                }
            }
            else if (GameManager.UnitsOnField[i].ToString() == "Wizard")
            {
                WizardUnit tempWUnit = (WizardUnit)GameManager.UnitsOnField[i];
                if (tempWUnit.Faction != base.faction && tempWUnit.Hp > 0)
                {
                    act = true;
                    int xDistance = Math.Abs(base.xPos - tempWUnit.XPos);
                    int yDistance = Math.Abs(base.yPos - tempWUnit.YPos);
                    double trueDistance = Math.Sqrt(Math.Pow(xDistance, 2) + Math.Pow(yDistance, 2));
                    int distance = (int)Math.Round(trueDistance, 0);
                    if (distance < nearest && tempWUnit.Hp >= 0)
                    {
                        type = "Unit";
                        nearest = distance;
                        indexOfNearest = i;
                        positionOfNearest[1] = tempWUnit.XPos;
                        positionOfNearest[0] = tempWUnit.YPos;
                    }
                }
            }
        }
        if (act == true)
        {
            if (CheckRange(nearest) == true)
            {
                Engage(indexOfNearest, type);
            }
            else
            {
                Move(positionOfNearest, ownIndex);

            }
        }
    }

    public override void Death(int index)
        {

        }

        public override string ToString()
        {
            return "Knight";
        }

        public override void Save()
        {

            BinaryFormatter bFormat = new BinaryFormatter();
            using (FileStream stream = new FileStream("saves/unit_save.game", FileMode.Append, FileAccess.Write))
            {
                bFormat.Serialize(stream, this);
            }
        }
    }

   
