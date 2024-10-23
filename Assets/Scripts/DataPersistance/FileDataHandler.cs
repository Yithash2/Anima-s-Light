using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";
    private bool  useEncryption = false;
    private readonly string encryptionCodeWord = "Superplatypus2000VSGLOOOO";

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption){
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;

    }

    public GameData Load(){
        //get  the full path
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if(File.Exists(fullPath)){
            try{
                //Load the serialized data from the file
                string dataToLoad = "";
                using(FileStream stream = new FileStream(fullPath, FileMode.Open)){
                    using(StreamReader reader = new StreamReader(stream)){
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if(useEncryption){
                dataToLoad = EncryptDecrypt(dataToLoad);
                Debug.Log(dataToLoad);
            }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }catch(Exception e){
                Debug.LogError("Error occured when trying to load the file" + fullPath + "\n" + e);
            }
        }
        
        return loadedData;
    }

    public void Reset(){
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        File.Delete(fullPath);
    }

    public void Save(GameData data){
        //get  the full path
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try{
            //create the directory the file will be written in if it doesnt already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //Serialize the C# game data into a json file
            string dataToStore = JsonUtility.ToJson(data, true);

            //Encrypt the data
            if(useEncryption){
                dataToStore = EncryptDecrypt(dataToStore);
            }

            //write the serialized data to the file
            using(FileStream stream = new FileStream(fullPath, FileMode.Create)){
                using(StreamWriter writer = new StreamWriter(stream)){
                    writer.Write(dataToStore);
                }
            }
        }catch(Exception e){
            Debug.LogError("Error occured when trying to save data to file : " + fullPath + "\n" + e);
        }
    }

    private string EncryptDecrypt(string data){
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++){
            modifiedData += (char) (data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }

    public bool GameDataExist(){
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        return File.Exists(fullPath);
    }
}
