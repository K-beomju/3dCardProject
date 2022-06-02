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

    const string saveFileName = "Magical.sav";

    public List<ISerializeble> ObjToSaveList = new List<ISerializeble>();

    public DeckData saveDeckData = new DeckData();
    public PlayerGameData gameData = new PlayerGameData();
    Rijndael myRijndael;

    public Action OnEndOfLoadGame;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);

        myRijndael = Rijndael.Create();
        ObjToSaveList.Add(saveDeckData);
    }
    private void Start()
    {
        //InvokeRepeating("AutoSave", 0, 30);
        //��ȣȭ ��ȣȭ �ך���
        /*string msg = "HelloWorld";

        byte[] encryptedMsg = Encrypt(msg, myRijndael.Key, myRijndael.IV);
        string decryptedMsg = Decrypt(encryptedMsg, myRijndael.Key, myRijndael.IV);
        print(decryptedMsg);*/

        LoadGameData();
    }
    string GetFilePath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }
    void Update()
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
    }

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
        //�� ����Ʈ ����
        /*JObject jsaveGame = new JObject();
        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy curEnemy = enemies[i];
            JObject jEnemy = curEnemy.Serialize();
            jsaveGame.Add(curEnemy.gameObject.name, jEnemy);
        }*/
        //�������̽��� �̿��� ����
        JObject jSaveGame = new JObject();
        for (int i = 0; i < ObjToSaveList.Count; i++)
        {
            jSaveGame.Add(ObjToSaveList[i].GetJsonKey(), ObjToSaveList[i].Serialize());
        }
        //���� ����
        StreamWriter sw = new StreamWriter(GetFilePath(saveFileName));
         sw.WriteLine(jSaveGame.ToString());
         sw.Close();
        //��ȣȭ ����
        /*byte[] encryptedSaveGame = Encrypt(jSaveGame.ToString(), myRijndael.Key, myRijndael.IV);
        File.WriteAllBytes(GetFilePath(saveFileName), encryptedSaveGame);*/
    }
    public void LoadGameData()
    {

        print("Load to : " + GetFilePath(saveFileName));

        string fileStr = GetFilePath(saveFileName);
        if (File.Exists(fileStr))
        {
            //�����о��
             StreamReader sr = new StreamReader(fileStr);
             string jsonString = sr.ReadToEnd();
             sr.Close();
            //��ȣȭ�ؼ� �о��
            /*byte[] decryptedSaveGame = File.ReadAllBytes(GetFilePath(saveFileName));
            string jsonString = Decrypt(decryptedSaveGame, myRijndael.Key, myRijndael.IV);

            print(jsonString);*/

            //Json�������� ä����
            /*JObject jSaveData = JObject.Parse(jsonString);
            for (int i = 0; i < enemies.Length; i++)
            {
                Enemy curEnemy = enemies[i];
                string enemyJsonString = jSaveData[curEnemy.gameObject.name].ToString();
                curEnemy.Desirialize(enemyJsonString);
            }*/

            //�������̽�
            JObject jSaveGame = JObject.Parse(jsonString);
           
            for (int i = 0; i < ObjToSaveList.Count; i++)
            {
                string objJsonData = jSaveGame[ObjToSaveList[i].GetJsonKey()].ToString();
                ObjToSaveList[i].Desirialize(objJsonData);
            }


            // ����� �Ҷ�
            if(gameData.isFirst) // ó�� �����߰ų� ���� ������� 
            {
                saveDeckData.CurDeck = new Deck();
                
            }
            OnEndOfLoadGame?.Invoke();
        }
        else
        {
            print("������ �������� ����");
            SaveGameData();
            LoadGameData();
        }
    }
    /*private void OnApplicationQuit()
    {
        SaveGameData();
    }

    private void OnApplicationPause(bool pause)
    {
        SaveGameData();
    }*/

    private void AutoSave()
    {
        SaveGameData();
        Debug.Log("Save Complete");
    }


}
