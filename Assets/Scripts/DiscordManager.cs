using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscordManager : MonoBehaviour
{
    Discord.Discord discord;
    // Start is called before the first frame update
    void Start()
    {
        discord = new Discord.Discord(1270383527174602752, (ulong)Discord.CreateFlags.Default);
        ChangeActivity();
    }

    void OnDisable() {
        discord.Dispose();
    }

    public void ChangeActivity() {
        var activityManager = discord.GetActivityManager();
        if(LevelInfo.Instance != null){
            var activity = new Discord.Activity {
            
            State = "Playing" + " " + LevelInfo.Instance.level.LevelName,
            Assets =
            {
                LargeImage = "logo_anima_s_light", // Larger Image Asset Value
                LargeText = ">:D",
            }
            
            };
            activityManager.UpdateActivity(activity, (res) => {
                Debug.Log("Activity updated!");
            });
        }else{
           var activity = new Discord.Activity {
            
            State = "Main Menu",
            Assets =
            {
                LargeImage = "logo_anima_s_light", // Larger Image Asset Value
                LargeText = ":D",
            }
            
            };
            activityManager.UpdateActivity(activity, (res) => {
                Debug.Log("Activity updated!");
            }); 
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        discord.RunCallbacks();
    }
}
