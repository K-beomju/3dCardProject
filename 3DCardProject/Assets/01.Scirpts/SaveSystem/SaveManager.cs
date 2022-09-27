using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System;

[Serializable]
public class GenericClass<T>
{
    public List<T> list;

    public GenericClass()
    {

    }
    public GenericClass(List<T> list1)
    {
        this.list = list1;
    }

    public void SetList(List<T> list)
    {
        this.list = list;
    }

}

public class SaveManager : Singleton<SaveManager>
{

    const string saveFileName = "NotMagic.sav";

    public List<ISerializeble> ObjToSaveList = new List<ISerializeble>();


    public PlayerGameData gameData = new PlayerGameData();
    Rijndael myRijndael;

    public Action OnEndOfLoadGame;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);

        myRijndael = Rijndael.Create();
        ObjToSaveList.Add(gameData);

        LoadGameData();

    }
    private void Start()
    {

        //InvokeRepeating("AutoSave", 0, 30);
        //암호화 복호화 테슽으
        /*string msg = "HelloWorld";

        byte[] encryptedMsg = Encrypt(msg, myRijndael.Key, myRijndael.IV);
        string decryptedMsg = Decrypt(encryptedMsg, myRijndael.Key, myRijndael.IV);
        print(decryptedMsg);*/

        //LoadGameData();
    }
    public void NewGame()
    {
        gameData = new PlayerGameData();
        gameData.isFirst = false;
        SaveGameData();
    }
    string GetFilePath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }
   /* void Update()
    {
        //save
        if (Input.GetKeyDown(KeyCode.K))
        {
            SaveGameData();
        }
        //Load
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadGameData();
        }
    }*/

    byte[] Encrypt(string message, byte[] key, byte[] IV)
    {
        AesManaged aes = new AesManaged();
        ICryptoTransform encrypto = aes.CreateEncryptor(key, IV);

        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptoStream = new CryptoStream(memoryStream, encrypto, CryptoStreamMode.Write);
        StreamWriter streamWriter = new StreamWriter(cryptoStream);

        streamWriter.WriteLine(message);

        streamWriter.Close();
        cryptoStream.Close();
        memoryStream.Close();

        return memoryStream.ToArray();
    }

    string Decrypt(byte[] message, byte[] key, byte[] IV)
    {
        AesManaged aes = new AesManaged();
        ICryptoTransform decrypto = aes.CreateDecryptor(key, IV);

        MemoryStream memoryStream = new MemoryStream(message);
        CryptoStream cryptoStream = new CryptoStream(memoryStream, decrypto, CryptoStreamMode.Read);
        StreamReader streamReader = new StreamReader(cryptoStream);

        string decryptedMessage = streamReader.ReadToEnd();

        streamReader.Close();
        cryptoStream.Close();
        memoryStream.Close();

        return decryptedMessage;
    }

    public void SaveGameData()
    {
        print("Save to : " + GetFilePath(saveFileName));
        //적 리스트 저장
        /*JObject jsaveGame = new JObject();
        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy curEnemy = enemies[i];
            JObject jEnemy = curEnemy.Serialize();
            jsaveGame.Add(curEnemy.gameObject.name, jEnemy);
        }*/
        //인터페이스를 이용한 저장
        JObject jSaveGame = new JObject();
        for (int i = 0; i < ObjToSaveList.Count; i++)
        {
            jSaveGame.Add(ObjToSaveList[i].GetJsonKey(), ObjToSaveList[i].Serialize());
        }
        //파일 저장
        StreamWriter sw = new StreamWriter(GetFilePath(saveFileName));
         sw.WriteLine(jSaveGame.ToString());
         sw.Close();
        //암호화 저장
        /*byte[] encryptedSaveGame = Encrypt(jSaveGame.ToString(), myRijndael.Key, myRijndael.IV);
        File.WriteAllBytes(GetFilePath(saveFileName), encryptedSaveGame);*/
    }
    public void LoadGameData()
    {

        print("Load to : " + GetFilePath(saveFileName));

        string fileStr = GetFilePath(saveFileName);
        if (File.Exists(fileStr))
        {
            //파일읽어옴
             StreamReader sr = new StreamReader(fileStr);
             string jsonString = sr.ReadToEnd();
             sr.Close();
            //복호화해서 읽어옴
            /*byte[] decryptedSaveGame = File.ReadAllBytes(GetFilePath(saveFileName));
            string jsonString = Decrypt(decryptedSaveGame, myRijndael.Key, myRijndael.IV);

            print(jsonString);*/

            //Json형식으로 채워줌
            /*JObject jSaveData = JObject.Parse(jsonString);
            for (int i = 0; i < enemies.Length; i++)
            {
                Enemy curEnemy = enemies[i];
                string enemyJsonString = jSaveData[curEnemy.gameObject.name].ToString();
                curEnemy.Desirialize(enemyJsonString);
            }*/

            //인터페이스
            JObject jSaveGame = JObject.Parse(jsonString);
           
            for (int i = 0; i < ObjToSaveList.Count; i++)
            {
                JToken jToken = jSaveGame[ObjToSaveList[i].GetJsonKey()];
                if (jToken != null)
                {
                    string objJsonData = jToken.ToString();
                    ObjToSaveList[i].Desirialize(objJsonData);
                }
                else
                {
                    gameData = new PlayerGameData();

                    SaveGameData();
                    return;
                }
            }


            // 덮어쓰기 할때
            if(gameData.isFirst) // 처음 시작했거나 리셋 했을경우 
            {

            }

            
            OnEndOfLoadGame?.Invoke();
        }
        else
        {
            print("파일이 존재하지 않음");
            SaveGameData();
            LoadGameData();
        }
    }
    private void OnApplicationQuit()
    {
        if (TutorialManager.Instance != null && TutorialManager.Instance.isTutorial)
            gameData = new PlayerGameData();

        SaveGameData();
    }

/*    private void OnApplicationPause(bool pause)
    {
        SaveGameData();
    }*/

    private void AutoSave()
    {
        SaveGameData();
        Debug.Log("Save Complete");
    }


}
