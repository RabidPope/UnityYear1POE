using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public GameObject hpBar;
    static Unit[] unitsOnField;
    static Building[] buildingsOnField;
    Map map;
    GameEngine engine;
    float nextRoundTime = 0.0f;
    float period = 1.0f;
    public GameObject blueBuilding;
    public GameObject redBuilding;
    public GameObject meleePrefabRed;
    public GameObject rangedPrefabRed;
    public GameObject meleePrefabBlue;
    public GameObject rangedPrefabBlue;
    public GameObject wizardPrefab;
    internal static Unit[] UnitsOnField { get => unitsOnField; set => unitsOnField = value; }
    internal static Building[] BuildingsOnField { get => buildingsOnField; set => buildingsOnField = value; }


    // Start is called before the first frame update

    void Start()
    {
        
        engine = new GameEngine();//create GameEngine instance
        map = new Map(6, 3, 20, 20, 6);//create Map instance
        engine.MapHandle = map;
        GameEngine.Rounds = 0;//Reset rounds counter

        UnitsOnField = map.SpawnUnits();
        BuildingsOnField = map.SpawnBuildings();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextRoundTime)
        {
            nextRoundTime += period;
            // execute block of code here
            engine.GameUpdater();
            UpdatePositions();
            
        }
    }

    public void UpdatePositions()
    {
        var clones = GameObject.FindGameObjectsWithTag("clone");
        foreach (var clone in clones)
        {
            Destroy(clone);
        }
        
        for (int i = 0; i < unitsOnField.Length; i++)
        {
            
            if (unitsOnField[i].ToString() == "Knight")
            {

                MeleeUnit tempMUnit = (MeleeUnit)unitsOnField[i];
                if (tempMUnit.Hp > 0)
                {
                    //GameObject tempObject;
                    
                    if (tempMUnit.Faction == 0)
                    {
                        Instantiate(meleePrefabBlue, new Vector3(tempMUnit.XPos -10, 0, tempMUnit.YPos -10), new Quaternion(0, 0, 0, 0));
                        
                    }
                    else
                    {
                        Instantiate(meleePrefabRed, new Vector3(tempMUnit.XPos -10, 0, tempMUnit.YPos - 10), new Quaternion(0, 0, 0, 0));
                    }
                    GameObject healthBar = Instantiate(hpBar, new Vector3(tempMUnit.XPos - 10, 1, tempMUnit.YPos - 10), hpBar.transform.rotation);
                    healthBar.transform.Find("FillHB").GetComponent<Image>().fillAmount = (float)tempMUnit.Hp / tempMUnit.MaxHP;
                    healthBar.transform.Find("Name").GetComponent<Text>().text = "Knight";
                }
            }
            else if (unitsOnField[i].ToString() == "Archer")
            {
                RangedUnit tempRUnit = (RangedUnit)unitsOnField[i];
                if (tempRUnit.Hp > 0)
                {
                   
                    if (tempRUnit.Faction == 0)
                    {
                        Instantiate(rangedPrefabBlue, new Vector3(tempRUnit.XPos-10, 0, tempRUnit.YPos-10), new Quaternion(0, 0, 0, 0));
                    }
                    else
                    {
                        Instantiate(rangedPrefabRed, new Vector3(tempRUnit.XPos-10, 0, tempRUnit.YPos -10), new Quaternion(0, 0, 0, 0));
                    }
                    GameObject healthBar = Instantiate(hpBar, new Vector3(tempRUnit.XPos - 10, 1, tempRUnit.YPos - 10), hpBar.transform.rotation);
                    healthBar.transform.Find("FillHB").GetComponent<Image>().fillAmount = (float)tempRUnit.Hp / tempRUnit.MaxHP;
                    healthBar.transform.Find("Name").GetComponent<Text>().text = "Archer";
                }
            }
            if (unitsOnField[i].ToString() == "Wizard")
            {                
                WizardUnit tempWUnit = (WizardUnit)unitsOnField[i];
                if (tempWUnit.Hp > 0)
                {
                    Instantiate(wizardPrefab, new Vector3(tempWUnit.XPos - 10, 0, tempWUnit.YPos - 10), new Quaternion(0, 0, 0, 0));
                    GameObject healthBar = Instantiate(hpBar, new Vector3(tempWUnit.XPos - 10, 1, tempWUnit.YPos - 10), hpBar.transform.rotation);
                    healthBar.transform.Find("FillHB").GetComponent<Image>().fillAmount = (float)tempWUnit.Hp / tempWUnit.MaxHP;
                    healthBar.transform.Find("Name").GetComponent<Text>().text = "Wizard";
                }
            }

        }
        for (int i = 0; i < buildingsOnField.Length; i++)
        {
            if (buildingsOnField[i].ToString() == "Resource Building")
            {
                ResourceBuilding tempBuild = (ResourceBuilding)buildingsOnField[i];
                if (tempBuild.HP > 0)
                {
                    if (tempBuild.Faction == 0)
                    {
                        Instantiate(blueBuilding, new Vector3(tempBuild.XPos - 10, -1, tempBuild.YPos - 10), new Quaternion(0, 0, 0, 0));
                    }
                    else
                    {
                        Instantiate(redBuilding, new Vector3(tempBuild.XPos - 10, -1, tempBuild.YPos - 10), new Quaternion(0, 0, 0, 0));
                    }
                    GameObject healthBar = Instantiate(hpBar, new Vector3(tempBuild.XPos - 10, 1, tempBuild.YPos - 10), hpBar.transform.rotation);
                    healthBar.transform.Find("FillHB").GetComponent<Image>().fillAmount = (float)tempBuild.HP / tempBuild.MaxHP;
                    healthBar.transform.Find("Name").GetComponent<Text>().text = "Resource";
                }
            }
            else
            {
                
                FactoryBuilding tempBuild = (FactoryBuilding)buildingsOnField[i];
                if (tempBuild.HP > 0)
                {

                    if (tempBuild.Faction == 0)
                    {
                        Instantiate(blueBuilding, new Vector3(tempBuild.XPos - 10, -1, tempBuild.YPos - 10), new Quaternion(0, 0, 0, 0));
                    }
                    else
                    {
                        Instantiate(redBuilding, new Vector3(tempBuild.XPos - 10, -1, tempBuild.YPos - 10), new Quaternion(0, 0, 0, 0));
                    }
                    GameObject healthBar = Instantiate(hpBar, new Vector3(tempBuild.XPos - 10, 1, tempBuild.YPos - 10), hpBar.transform.rotation);
                    healthBar.transform.Find("FillHB").GetComponent<Image>().fillAmount = (float)tempBuild.HP / tempBuild.MaxHP;
                    healthBar.transform.Find("Name").GetComponent<Text>().text = "Factory";
                }
            }
        }
    }
    
}
