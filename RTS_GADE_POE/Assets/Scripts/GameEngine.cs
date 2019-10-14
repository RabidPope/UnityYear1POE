using System;
using System.IO;



    class GameEngine
    {
        
       
        private Map mapHandle;
        private static int rounds;
        //private frmGame form;
        Random rng = new Random();

        
        public Map MapHandle { get => mapHandle; set => mapHandle = value; }
        public static int Rounds { get => rounds; set => rounds = value; }
        public string TeamResources { get => teamResources; set => teamResources = value; }
        public string UnitInfo { get => unitInfo; set => unitInfo = value; }
        public string BuildingInfo { get => buildingInfo; set => buildingInfo = value; }
        //Constants
        private int unitCost = 10;
        //Initialised variables
        private int team1Wood = 0;
        private int team1Steel = 0;
        private int team2Wood = 0;
        private int team2Steel = 0;
        
        protected string teamResources = "Not initialised";
        protected string unitInfo = "Not initialised";
        protected string buildingInfo = "Not initialised";

        public void SaveGame()
        {
            MeleeUnit tempMUnit;
            RangedUnit tempRUnit;
            ResourceBuilding tempRBuilding;
            FactoryBuilding tempFBuilding;
            //Create saves dir and delete old files
            if (!Directory.Exists("saves"))
            {
                Directory.CreateDirectory("saves");
            }
            if (File.Exists("saves/unit_save.game"))
            {
                File.Delete("saves/unit_save.game");
            }
            if (File.Exists("saves/building_save.game"))
            {
                File.Delete("saves/building_save.game");
            }
            
            //Save buildings
            for (int i = 0; i < Map.BuildingsOnField.Length; i++)
            {
               if(Map.BuildingsOnField[i].ToString() == "Resource Building")
                {
                    tempRBuilding = (ResourceBuilding)Map.BuildingsOnField[i];
                    tempRBuilding.Save();
                }
               else
                {
                    tempFBuilding = (FactoryBuilding)Map.BuildingsOnField[i];
                    tempFBuilding.Save();
                }
            }
            //Save units
            for (int i = 0; i < Map.UnitsOnField.Length; i++)
            {
                if (Map.UnitsOnField[i].ToString() == "Knight")
                {
                    tempMUnit = (MeleeUnit)Map.UnitsOnField[i];
                    tempMUnit.Save();
                }
                else if (Map.UnitsOnField[i].ToString() == "Archer")
                {
                    tempRUnit = (RangedUnit)Map.UnitsOnField[i];
                    tempRUnit.Save();
                }
                else if (Map.UnitsOnField[i].ToString() == "Wizard")
                {
                    WizardUnit tempWUnit = (WizardUnit)Map.UnitsOnField[i];
                    tempWUnit.Save();
                }
            }
        }

        public void GameUpdater()
        {
            Unit tempUnit;
            MeleeUnit tempMUnit;
            RangedUnit tempRUnit;
            Building tempBuilding;
            ResourceBuilding tempRBuilding;
            FactoryBuilding tempFBuilding;
          
            
                       
            int faction;
            unitInfo = "";
            int unitsLeft = Map.UnitsOnField.Length;
            int buildingsLeft = Map.BuildingsOnField.Length;
            


            //Update Unit Actions
            for (int i = 0; i < unitsLeft; i++)//For each unit
            {                
                tempUnit = Map.UnitsOnField[i]; //Melee               
                if (tempUnit.ToString() == "Knight")
                {
                    tempMUnit = (MeleeUnit)tempUnit;
                    faction = tempMUnit.Faction;
                    double percHP = (double)tempMUnit.Hp/tempMUnit.MaxHP * 100;
                    if (percHP < 25 & percHP> 0)
                    {
                        tempMUnit.Attacking = false;
                        tempMUnit.Move(new int[] { 0, 0 }, i);
                        unitInfo += tempMUnit.Shape + " | HP:" + tempMUnit.Hp + " | Fleeing | " + "Team: " + (tempMUnit.Faction + 1) + " | " + tempMUnit.ToString() + " | " + "X,Y : " + tempMUnit.XPos + "," + tempMUnit.YPos + "\n";
                    }
                    else if (tempMUnit.Hp <= 0)
                    {
                        tempMUnit.Death(i);
                       
                    }
                    else //if not dead or fleeing, find enemy
                    {
                        
                        tempMUnit.FindEnemy(i);
                        unitInfo += tempMUnit.Shape + " | HP:" + tempMUnit.Hp + " | Hunting | "  + "Team: " + (tempMUnit.Faction +1 ) + " | " + tempMUnit.ToString() + " | "+ "X,Y : " + tempMUnit.XPos+ ","+ tempMUnit.YPos + "\n";
                    }

                    
                }
                else if (tempUnit.ToString() == "Archer")
                {
                    tempRUnit = (RangedUnit)tempUnit;
                    faction = tempRUnit.Faction;
                    double percHP = (double)tempRUnit.Hp / tempRUnit.MaxHP * 100;
                    if (percHP < 25 & percHP > 0)
                    {
                        tempRUnit.Attacking = false;
                        tempRUnit.Move(new int[] { 0, 0 },i);
                        unitInfo += tempRUnit.Shape + " | HP:" + tempRUnit.Hp + " | Fleeing | " + "Team " + (tempRUnit.Faction + 1) + " | " + tempRUnit.ToString() + " | " + "X,Y : " + tempRUnit.XPos + "," + tempRUnit.YPos + "\n";
                    }
                    else if (tempRUnit.Hp <= 0)
                    {
                        tempRUnit.Death(i);
                        
                    }
                    else
                    {
                        
                        tempRUnit.FindEnemy(i);
                        unitInfo += tempRUnit.Shape + " | HP:" + tempRUnit.Hp + " | Hunting | " + "Team " + (tempRUnit.Faction + 1) + " | " + tempRUnit.ToString() + " | " + "X,Y : " + tempRUnit.XPos + "," + tempRUnit.YPos + "\n";
                    }

                    
                }
                else if (tempUnit.ToString() == "Wizard")
                {
                    WizardUnit tempWUnit = (WizardUnit)tempUnit;
                    faction = tempWUnit.Faction;
                    double percHP = (double)tempWUnit.Hp / tempWUnit.MaxHP * 100;
                    if (percHP < 50 & percHP > 0)
                    {
                        tempWUnit.Move(new int[] { 0, 0 }, i);
                        unitInfo += tempWUnit.Shape + " | HP:" + tempWUnit.Hp + " | Fleeing | " + "Team " + (tempWUnit.Faction + 1) + " | " + tempWUnit.ToString() + " | " + "X,Y : " + tempWUnit.XPos + "," + tempWUnit.YPos + "\n";
                    }
                    else if (tempWUnit.Hp <= 0)
                    {
                        tempWUnit.Death(i);

                    }
                    else
                    {

                        tempWUnit.FindEnemy(i);
                        unitInfo += tempWUnit.Shape + " | HP:" + tempWUnit.Hp + " | Hunting | " + "Team " + (tempWUnit.Faction + 1) + " | " + tempWUnit.ToString() + " | " + "X,Y : " + tempWUnit.XPos + "," + tempWUnit.YPos + "\n";
                    }
                }


            }
            buildingInfo = "";
            //Update Building Actions
            for (int i = 0; i < buildingsLeft; i++)
            {
                tempBuilding = Map.BuildingsOnField[i];

                if (tempBuilding.ToString() == "Resource Building")
                {
                 //if resource building
                    tempRBuilding = (ResourceBuilding)tempBuilding;
                    if (tempRBuilding.HP > 0 && tempRBuilding.Remaining > 0)
                    {
                        tempRBuilding.generateResource();

                        if (tempRBuilding.Faction == 0)
                        {
                            if (tempRBuilding.ResourceType == "Wood")
                            {
                                //If wood
                                team1Wood = team1Wood + tempRBuilding.GenPerRound;
                                buildingInfo += tempRBuilding.Shape + " | HP:" + tempRBuilding.HP + " | Wood:" + tempRBuilding.Remaining + " | " + "Team " + (tempRBuilding.Faction + 1) + " | " + tempRBuilding.ToString() + " | " + "X,Y : " + tempRBuilding.XPos + "," + tempRBuilding.YPos + "\n";
                            }
                            else
                            {
                                //If steel
                                team1Steel = team1Steel + tempRBuilding.GenPerRound;
                                buildingInfo += tempRBuilding.Shape + " | HP:" + tempRBuilding.HP + " | Steel:" + tempRBuilding.Remaining + " | " + "Team " + (tempRBuilding.Faction + 1) + " | " + tempRBuilding.ToString() + " | " + "X,Y : " + tempRBuilding.XPos + "," + tempRBuilding.YPos + "\n";
                            }
                        }
                        else
                        {
                            if (tempRBuilding.ResourceType == "Wood")
                            {
                                //If wood
                                team2Wood = team2Wood + tempRBuilding.GenPerRound;
                                buildingInfo += tempRBuilding.Shape + " | HP:" + tempRBuilding.HP + " | Wood:" + tempRBuilding.Remaining + " | " + "Team " + (tempRBuilding.Faction + 1) + " | " + tempRBuilding.ToString() + " | " + "X,Y : " + tempRBuilding.XPos + "," + tempRBuilding.YPos + "\n";
                            }
                            else
                            {
                                //If steel
                                team2Steel = team2Steel + tempRBuilding.GenPerRound;
                                buildingInfo += tempRBuilding.Shape + " | HP:" + tempRBuilding.HP + " | Steel:" + tempRBuilding.Remaining + " | " + "Team " + (tempRBuilding.Faction + 1) + " | " + tempRBuilding.ToString() + " | " + "X,Y : " + tempRBuilding.XPos + "," + tempRBuilding.YPos + "\n";
                            }
                        }
                    }
                }
                else
                {
                    //For factory building
                    tempFBuilding = (FactoryBuilding)tempBuilding;
                    if (tempFBuilding.HP > 0)
                    {
                        buildingInfo += tempFBuilding.Shape + " | HP:" + tempFBuilding.HP + " | Team " + (tempFBuilding.Faction + 1) + " | " + tempFBuilding.ToString() + " | " + "X,Y : " + tempFBuilding.XPos + "," + tempFBuilding.YPos + "\n";
                        if (rounds % tempFBuilding.ProdSpeed == 0)
                        {
                            if (tempFBuilding.Faction == 0 && team1Steel + team1Wood > unitCost)
                            {
                                mapHandle.AddUnit(tempFBuilding.SpawnUnit());
                                team1Wood = Math.Max(0, team1Wood - Math.Max(0, unitCost - team1Steel));
                                team1Steel = Math.Max(0, team1Steel - unitCost);
                            }
                            else if (tempFBuilding.Faction == 1 && team2Steel + team2Wood > unitCost)
                            {
                                mapHandle.AddUnit(tempFBuilding.SpawnUnit());
                                team2Wood = Math.Max(0,team2Wood - Math.Max(0,unitCost - team2Steel));
                                team2Steel = Math.Max(0, team2Steel - unitCost);
                            }

                        }
                    }
                }
                
            }

            teamResources = "Team 1" + "\n" + "Wood: " + team1Wood + "\n" + "Steel: " + team1Steel + "\n" + "\n" + "Team 2" + "\n" + "Wood: " + team2Wood + "\n" + "Steel: " + team2Steel;
            rounds = rounds + 1;//Update rounds
            
        }

       
    }

