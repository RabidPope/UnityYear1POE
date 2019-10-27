
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

    class Map : MonoBehaviour
    {
        //protected static Unit[] unitsOnField;
        //protected static Building[] buildingsOnField;
        protected int armySize;
        protected static char[,] mapSymbols;
        protected System.Random rng = new System.Random();
        protected int buildings;
        private static int mapSizeY;
        private static int mapSizeX;
        private int nArmySize;
        

        public Map(int armySize, int nArmySize, int mapSizeY, int mapSizeX, int buildings)
        {
            this.buildings = buildings;
            this.armySize = armySize;
            this.nArmySize = nArmySize;
            Map.mapSizeY = mapSizeY;
            Map.mapSizeX = mapSizeX;
            InitialiseMap();
        }

        //public static Unit[] UnitsOnField { get => unitsOnField; set => unitsOnField = value; }
        //public static Building[] BuildingsOnField { get => buildingsOnField; set => buildingsOnField = value; }
        public static int MapSizeY { get => mapSizeY; }
        public static int MapSizeX { get => mapSizeX; }

        /*public void LoadGame()
        {
            BinaryFormatter bFormat = new BinaryFormatter();

            int index = 0;
            long position = 0;
            buildingsOnField = new Building[0];
            unitsOnField = new Unit[0];
            while (true)
            {
                using (FileStream stream = new FileStream("saves/unit_save.game", FileMode.Open, FileAccess.Read))
                {
                    if (position < stream.Length)
                    {
                        Array.Resize(ref unitsOnField, unitsOnField.Length + 1);
                        stream.Seek(position, SeekOrigin.Begin);
                        unitsOnField[index] = (Unit)bFormat.Deserialize(stream);
                        position = stream.Position;
                        index++;
                    }
                    else
                        break;
                }
            }
            position = 0;
            index = 0;
            while (true)
            {
                using (FileStream stream = new FileStream("saves/building_save.game", FileMode.Open, FileAccess.Read))
                {
                    if (position < stream.Length)
                    {
                        Array.Resize(ref buildingsOnField, buildingsOnField.Length + 1);
                        stream.Seek(position, SeekOrigin.Begin);
                        buildingsOnField[index] = (Building)bFormat.Deserialize(stream);
                        position = stream.Position;
                        index++;
                    }
                    else
                        break;
                }
            }

        }
        */

        public void InitialiseMap()
        {
            mapSymbols = new char[mapSizeY, mapSizeX];
            for (int y = 0; y < mapSizeY; y++)
            {
                for (int x = 0; x < mapSizeX; x++)
                {
                    mapSymbols[y, x] = '.';
                }
            }
        }

        public Unit[] SpawnUnits()
        {
            int type;
            int xRoll;
            int yRoll;
            Unit []unitsOnField = new Unit[armySize + nArmySize];
            int unitCount = 0; //how many units intside array
                               //Spawn Attack Units

            for (int unitPlaced = 0; unitPlaced < armySize; unitPlaced++)
            {
                xRoll = rng.Next((0), mapSizeX);
                yRoll = rng.Next((0), mapSizeY);

                if (mapSymbols[yRoll, xRoll] == '.')//Use 2d symbols to determine open positions
                {
                    int team = rng.Next(0, 2);
                    type = rng.Next(0, 2);


                    if (type == 0)
                    {
                        unitsOnField[unitCount] = new MeleeUnit(xRoll, yRoll, team, false);
                        unitCount++;
                    }
                    else
                    {
                        unitsOnField[unitCount] = new RangedUnit(xRoll, yRoll, team, false);
                        unitCount++;
                    }
                }
                else
                {
                    unitPlaced--;//No open position, redo
                }
            
            }
            //Spawn Wizards
            for (int unitPlaced = 0; unitPlaced < nArmySize; unitPlaced++)
            {
                xRoll = rng.Next(0, mapSizeX);
                yRoll = rng.Next((0), mapSizeY);
                if (mapSymbols[yRoll, xRoll] == '.')//Use 2d symbols to determine open positions
                {
                    int team = 2;
                    unitsOnField[unitCount] = new WizardUnit(xRoll, yRoll, team, false);
                    unitCount++;
                }
                else
                {
                    unitPlaced--;//No open position, redo
                }
            }
            return unitsOnField;
        }


        public Building [] SpawnBuildings()
        {

            int xRoll;
            int yRoll;
            Building [] buildingsOnField = new Building[buildings];
            int buildingCount = 0;
            //Spawn Attack Units
            for (int buildingsPlaced = 0; buildingsPlaced < buildings; buildingsPlaced++)
            {
                xRoll = rng.Next(0, mapSizeX);
                yRoll = rng.Next(0, mapSizeY);

                if (mapSymbols[yRoll, xRoll] == '.')//Use 2d symbols to determine open positions
                {
                    int team = rng.Next(0, 2);
                    int buildingType = rng.Next(0, 2);


                    if (buildingType == 0)
                    {
                        int resourceType = rng.Next(0, 2);

                        buildingsOnField[buildingCount] = new ResourceBuilding(xRoll, yRoll, team, resourceType, 100, 1);
                        buildingCount++;
                    }
                    else
                    {
                        int unitType = rng.Next(0, 2);
                        int spawnPoint = yRoll + 1;
                        if (yRoll == mapSizeY - 1)
                        {
                            spawnPoint = yRoll - 1;//Places spawn point above building if needed
                        }
                        buildingsOnField[buildingCount] = new FactoryBuilding(xRoll, yRoll, team, unitType, spawnPoint);
                        buildingCount++;
                    }
                    
                }
                else
                {
                    buildingsPlaced--;//No open position, redo
                }
            }
        return buildingsOnField;
        }


        public void AddUnit(Unit newUnit)
        {
            
            Unit[] tempArray = GameManager.UnitsOnField;
            GameManager.UnitsOnField = new Unit[tempArray.Length + 1];

            for (int i = 0; i < tempArray.Length; i++)
            {
            GameManager.UnitsOnField[i] = tempArray[i];
            }
            GameManager.UnitsOnField[GameManager.UnitsOnField.Length - 1] = newUnit;
        
        }
        
        /*
        
    
        
            
        }
        public string UpdateMap()
        {
            //variables
            string map = "";
            Unit tempUnit;
            MeleeUnit tempMUnit;
            RangedUnit tempRUnit;
            WizardUnit tempWUnit;
            Building tempBuilding;
            ResourceBuilding tempRBuilding;
            FactoryBuilding tempFBuilding;

            //Clean map so old unit symbols are removed
             //InitialiseMap();

            //Re-fill mapSymbols array with units
            var clones = GameObject.FindGameObjectsWithTag("clone");
            foreach (var clone in clones)
            {
                Destroy(clone);
            }
            for (int i = 0; i < unitsOnField.Length; i++)
            {
                tempUnit = unitsOnField[i];
                if (tempUnit.ToString() == "Knight")
                {
                    
                    tempMUnit = (MeleeUnit)tempUnit;
                    if (tempMUnit.Hp > 0)
                    {
                        mapSymbols[tempMUnit.YPos, tempMUnit.XPos] = tempMUnit.Shape;
                        Instantiate(meleePrefab, new Vector3(tempMUnit.XPos, 0, tempMUnit.YPos), new Quaternion(0, 0, 0, 0));
                    }
                }
                else if (tempUnit.ToString() == "Archer")
                {
                    tempRUnit = (RangedUnit)tempUnit;
                    if (tempRUnit.Hp > 0)
                    {
                        mapSymbols[tempRUnit.YPos, tempRUnit.XPos] = tempRUnit.Shape;
                    }
                }
                if (tempUnit.ToString() == "Wizard")
                {
                    tempWUnit = (WizardUnit)tempUnit;
                    if (tempWUnit.Hp > 0)
                    {
                        mapSymbols[tempWUnit.YPos, tempWUnit.XPos] = tempWUnit.Shape;
                    }
                }
            }

            for (int i = 0; i < buildingsOnField.Length; i++)
            {
                tempBuilding = buildingsOnField[i];
                if (tempBuilding.ToString() == "Resource Building")
                {

                    tempRBuilding = (ResourceBuilding)tempBuilding;
                    if (tempRBuilding.HP > 0)
                    {
                        mapSymbols[tempRBuilding.YPos, tempRBuilding.XPos] = tempRBuilding.Shape;
                    }
                }
                else
                {
                    tempFBuilding = (FactoryBuilding)tempBuilding;
                    if (tempFBuilding.HP > 0)
                    {
                        mapSymbols[tempFBuilding.YPos, tempFBuilding.XPos] = tempFBuilding.Shape;
                    }
                }
            }

            //Convert mapSymbols array to a string
            for (int y = 0; y < mapSizeY; y++)
            {
                for (int x = 0; x < mapSizeX; x++)
                {
                    map += mapSymbols[y, x];
                }
                map += "\n";

            }
            return map;
        }*/
    }

