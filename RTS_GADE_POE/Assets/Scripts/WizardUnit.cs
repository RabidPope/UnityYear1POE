using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


    [Serializable]
    class WizardUnit : Unit
    {

        
        public WizardUnit(int xPos, int yPos, int faction, bool attacking) : base("Wizard",xPos, yPos, 20, 2, 3, 3, faction, 'T', attacking)
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
        Attacking = false;
        Random rng = new Random();
        int[] tempArray = { -1, 1 };
        if (GameEngine.Rounds % base.speed == 0)
            {
                double percHP = (double)base.hp / base.maxHP * 100;
                if (percHP > 50)//Hunt
                {
                    
                    base.yPos = base.yPos + (Math.Max(-1, Math.Min(position[0] - base.yPos, 1)));
                    base.xPos = base.xPos + (Math.Max(-1, Math.Min(position[1] - base.xPos, 1)));
                }
                else//Flee
                {
                    base.yPos = Math.Max(0, Math.Min(Map.MapSizeY -1, base.yPos + tempArray[rng.Next(0, 2)]));
                    base.xPos = Math.Max(0, Math.Min(Map.MapSizeX -1, base.xPos + tempArray[rng.Next(0, 2)]));
                }
            GameManager.UnitsOnField[ownIndex] = this;
            }
        }
        public override void Engage(int index, string type)
        {
            
            MeleeUnit tempMUnit;
            RangedUnit tempRUnit;
            
            if (GameManager.UnitsOnField[index].ToString() == "Knight")
            {
                    tempMUnit = (MeleeUnit)GameManager.UnitsOnField[index];
                    tempMUnit.Hp = tempMUnit.Hp - base.attack;
                GameManager.UnitsOnField[index] = tempMUnit;//Do damage to unit at index
            }
            else
            {
                    tempRUnit = (RangedUnit)GameManager.UnitsOnField[index];
                    tempRUnit.Hp = tempRUnit.Hp - base.attack;
                    GameManager.UnitsOnField[index] = tempRUnit;// Do damage to unit at index
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
            int[] listOfTargets = new int[0];
            attacking = false;
            string type = "";
           
            int nearest = 999;
            int[] positionOfNearest = new int[] { this.yPos, this.xPos };//Store Position of nearest unit
            
            bool act = false;
            
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
                            if(CheckRange(distance))
                            {
                                Array.Resize(ref listOfTargets, listOfTargets.Length + 1);
                                listOfTargets[listOfTargets.Length - 1] = i;
                            }
                                                        
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
                            if (CheckRange(distance))
                            {
                                Array.Resize(ref listOfTargets, listOfTargets.Length + 1);
                                listOfTargets[listOfTargets.Length - 1] = i;
                            }                                                       
                            positionOfNearest[1] = tempRUnit.XPos;
                            positionOfNearest[0] = tempRUnit.YPos;
                        }
                    }
                }
            }
            if (act == true)
            {
                if (listOfTargets.Length > 0)
                {
                    attacking = true;
                    for (int i = 0; i < listOfTargets.Length; ++i)
                    {
                        Engage(listOfTargets[i], type);
                    }
                    
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
            return "Wizard";
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

