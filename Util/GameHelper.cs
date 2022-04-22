using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 游戏帮助类，单例
/// </summary>
public sealed class GameHelper
{
    private static volatile GameHelper instance;
    private static object syncRoot = new object();
    public static GameHelper Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new GameHelper();

                }
            }
            return instance;
        }
    }
    public Doctor doctor;//当前登录的医生信息
    public Patient patient;//进行训练的病人信息
    public string gameName;//游戏名称
    public int level;//游戏难度(0:简单，1：中等，2：困难)
    public int time;//游戏时间(秒)
    public int senseCount;//漂流勇士游戏场景数
    public string socketMsg = "";//收到的socket内容
    /// <summary>
    /// 获取游戏配置信息，包括游戏难度，游戏时间
    /// </summary>
    /// <returns></returns>
    public string GetStartGameInfo()
    {
        string gameinfo = "";
        switch (gameName)
        {
            case "漂流勇士":
                gameinfo = RiverGameInfo();
                break;
            case "愉快假期":
                gameinfo = HolidayGameInfo();
                break;
            case "猫捉老鼠":
                gameinfo = CaughtMouseGameInfo();
                break;
            case "水果超市":
                gameinfo = FruitGameInfo();
                break;
            case "动物世界":
                gameinfo = FruitGameInfo();
                break;
            case "打地鼠":
                gameinfo = ShrewMouseGameInfo();
                break;
            case "擦黑板":
                gameinfo = ClearBoardGameInfo();
                break;
            case "划船游戏":
                gameinfo = BoatingGameInfo();
                break;
            case "我的庄园":
                gameinfo = NewShrewMouse();
                break;
            default:
                break;
        }
        MessageEntity messageEntity = new MessageEntity
        {
            totalPageCount = 1,
            currentPageNo = 1,
            command = "A001",
            messageBody = gameinfo
        };
        return JsonConvert.SerializeObject(messageEntity);
    }
    /// <summary>
    /// 漂流勇士游戏开始时发送的数据
    /// </summary>
    /// <returns></returns>
    private string RiverGameInfo()
    {
        string gameinfo = "";
        RequestRiverEntity river = new RequestRiverEntity();
        for (int i = 1; i <= senseCount; i++)
        {
            river.ExSharps.Add(i);
        }
        if (level == 0)
        {
            river.level = "Easy";
        }
        else if (level == 1)
        {
            river.level = "Medium";
        }
        else if (level == 2)
        {
            river.level = "Hard";
        }
        river.userId = patient.patientName + "";
        river.time = GameHelper.instance.time;
        gameinfo = JsonConvert.SerializeObject(river).Replace("\"", "'");
        return gameinfo;
    }
    /// <summary>
    /// 愉快假期游戏开始时发送的数据
    /// </summary>
    private string HolidayGameInfo()
    {
        string gameinfo = "";
        RequestHolidayEntity holiday = new RequestHolidayEntity();

        if (level == 0)
        {
            holiday.level = "easy";
        }
        else if (level == 1)
        {
            holiday.level = "medium";
        }
        else if (level == 2)
        {
            holiday.level = "hard";
        }
        holiday.time = GameHelper.instance.time;
        holiday.userId = patient.patientName + "";
        gameinfo = JsonConvert.SerializeObject(holiday).Replace("\"", "'");
        return gameinfo;
    }
    /// <summary>
    /// 猫捉老鼠游戏开始时发送的数据
    /// </summary>
    /// <returns></returns>
    private string CaughtMouseGameInfo()
    {
        string gameinfo = "";
        RequestMouseEntity mouse = new RequestMouseEntity();

        if (level == 0)
        {
            mouse.level = 1;
        }
        else if (level == 1)
        {
            mouse.level = 2;
        }
        else if (level == 2)
        {
            mouse.level = 3;
        }
        mouse.time = GameHelper.instance.time;
        mouse.userId = patient.patientName + "";
        gameinfo = JsonConvert.SerializeObject(mouse).Replace("\"", "'");
        return gameinfo;
    }
    /// <summary>
    /// 水果超市,动物世界游戏开始时发送的数据
    /// </summary>
    /// <returns></returns>
    private string FruitGameInfo()
    {
        string gameinfo = "";
        RequestFruitEntity fruit = new RequestFruitEntity();
        if (level == 0)
        {
            fruit.level = 0;
        }
        else if (level == 1)
        {
            fruit.level = 1;
        }
        else if (level == 2)
        {
            fruit.level = 2;
        }
        fruit.time = GameHelper.instance.time;
        fruit.userId = patient.patientName + "";
        gameinfo = JsonConvert.SerializeObject(fruit).Replace("\"", "'");
        return gameinfo;
    }

    /// <summary>
    /// 打地鼠游戏开始时发送的数据
    /// </summary>
    /// <returns></returns>
    private string ShrewMouseGameInfo()
    {
        string gameinfo = "";
        RequestShrewMouseEntity fruit = new RequestShrewMouseEntity();
        if (level == 0)
        {
            fruit.level = 1;
        }
        else if (level == 1)
        {
            fruit.level = 2;
        }
        else if (level == 2)
        {
            fruit.level = 3;
        }
        fruit.time = GameHelper.instance.time;
        fruit.userId = patient.patientName + "";
        gameinfo = JsonConvert.SerializeObject(fruit).Replace("\"", "'");
        return gameinfo;
    }

    /// <summary>
    /// 擦黑板游戏启动时发送的数据
    /// </summary>
    /// <returns></returns>
    private string ClearBoardGameInfo()
    {
        string gameinfo = "";
        RequestClearEntity clear = new RequestClearEntity();
        if (level == 0)
        {
            clear.level = "easy";
        }
        else if (level == 1)
        {
            clear.level = "medium";
        }
        else if (level == 2)
        {
            clear.level = "hard";
        }
        clear.time = GameHelper.instance.time;
        clear.user = (new TrainresultDal()).GetUserScoreInfo("擦黑板", patient.patientID, patient.patientName);
        clear.brush = (new BrushDal()).GetPatientBrush(patient.patientID);
        clear.singleRanks = (new TrainresultDal()).GetSingleFive("擦黑板");
        clear.totalRanks = (new TrainresultDal()).GetTotalFive("擦黑板");
        gameinfo = JsonConvert.SerializeObject(clear).Replace("\"", "'");
        return gameinfo;
    }

    /// <summary>
    /// 划船游戏启动时发送的数据
    /// </summary>
    /// <returns></returns>
    private string BoatingGameInfo()
    {
        string gameinfo = "";
        RequestBoatingEntity boat = new RequestBoatingEntity();
        if (level == 0)
        {
            boat.level = "Easy";
        }
        else if (level == 1)
        {
            boat.level = "Medium";
        }
        else if (level == 2)
        {
            boat.level = "Hard";
        }
        boat.time = GameHelper.instance.time;
        boat.user = (new TrainresultDal()).GetUserScoreInfo("划船游戏", patient.patientID, patient.patientName);
        boat.singleRanks = (new TrainresultDal()).GetSingleFive("划船游戏");
        boat.totalRanks = (new TrainresultDal()).GetTotalFive("划船游戏");
        gameinfo = JsonConvert.SerializeObject(boat).Replace("\"", "'");
        return gameinfo;
    }

    /// <summary>
    /// 我的庄园启动时发送的数据
    /// </summary>
    /// <returns></returns>
    private string NewShrewMouse()
    {
        string gameinfo = "";
        RequestWaterEntity requestWaterEntity = new RequestWaterEntity();
        requestWaterEntity.time = GameHelper.instance.time;
        requestWaterEntity.userId = patient.patientName;
        requestWaterEntity.waterCount = 0;//(new TrainresultDal()).GetLastScore("我的庄园", patient.patientID);
        requestWaterEntity.treeTypeCount = (new TrainresultDal()).GetLastTreeTypeCount("我的庄园", patient.patientID);
        gameinfo = JsonConvert.SerializeObject(requestWaterEntity).Replace("\"", "'");
        return gameinfo;
    }

    /// <summary>
    /// 我的庄园游戏返回前五名信息
    /// </summary>
    /// <returns></returns>
    public string NewShrewMouseTopFive()
    {
        List<RankEntity> singleRanks = (new TrainresultDal()).GetSingleFive("我的庄园");
        WaterPaiMing waterPaiMing = new WaterPaiMing();
        if (singleRanks.Count > 0)
        {
            for (int i = 0; i < singleRanks.Count; i++)
            {
                if (i == 0)
                {
                    waterPaiMing.One.userId = singleRanks[0].playerName;
                    waterPaiMing.One.waterCount = singleRanks[0].playerScore;
                    waterPaiMing.One.treeTypeCount = singleRanks[0].treeTypeCount;
                }
                else if (i == 1)
                {
                    waterPaiMing.Two.userId = singleRanks[1].playerName;
                    waterPaiMing.Two.waterCount = singleRanks[1].playerScore;
                    waterPaiMing.Two.treeTypeCount = singleRanks[1].treeTypeCount;
                }
                else if (i == 2)
                {
                    waterPaiMing.Three.userId = singleRanks[2].playerName;
                    waterPaiMing.Three.waterCount = singleRanks[2].playerScore;
                    waterPaiMing.Three.treeTypeCount = singleRanks[2].treeTypeCount;
                }
                else if (i == 3)
                {
                    waterPaiMing.Four.userId = singleRanks[3].playerName;
                    waterPaiMing.Four.waterCount = singleRanks[3].playerScore;
                    waterPaiMing.Four.treeTypeCount = singleRanks[3].treeTypeCount;
                }
                else if (i == 4)
                {
                    waterPaiMing.Five.userId = singleRanks[4].playerName;
                    waterPaiMing.Five.waterCount = singleRanks[4].playerScore;
                    waterPaiMing.Five.treeTypeCount = singleRanks[4].treeTypeCount;
                }
            }
        }
        return JsonConvert.SerializeObject(waterPaiMing).Replace("\"", "'");
    }
}