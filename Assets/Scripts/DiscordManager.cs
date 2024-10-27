using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscordManager : MonoBehaviour
{
    private Discord.Discord discord;
    private bool isDiscordOpen = false;

    void Start()
    {
        try
        {
            discord = new Discord.Discord(1270383527174602752, (ulong)Discord.CreateFlags.Default);
            isDiscordOpen = true;
            ChangeActivity();
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Discord is not open or could not initialize: " + e.Message);
            isDiscordOpen = false;
        }
    }

    void OnDisable()
    {
        if (isDiscordOpen)
        {
            discord.Dispose();
        }
    }

    public void ChangeActivity()
    {
        if (!isDiscordOpen) return; // Exit if Discord isn't open

        var activityManager = discord.GetActivityManager();
        if (LevelInfo.Instance != null)
        {
            var activity = new Discord.Activity
            {
                State = "Playing " + LevelInfo.Instance.level.LevelName,
                Assets =
                {
                    LargeImage = "logo_anima_s_light", // Larger Image Asset Value
                    LargeText = ">:D",
                }
            };
            activityManager.UpdateActivity(activity, (res) =>
            {
                Debug.Log("Activity updated!");
            });
        }
        else
        {
            var activity = new Discord.Activity
            {
                State = "Main Menu",
                Assets =
                {
                    LargeImage = "logo_anima_s_light", // Larger Image Asset Value
                    LargeText = ":D",
                }
            };
            activityManager.UpdateActivity(activity, (res) =>
            {
                Debug.Log("Activity updated!");
            });
        }
    }

    void Update()
    {
        if (isDiscordOpen)
        {
            discord.RunCallbacks();
        }
    }
}
