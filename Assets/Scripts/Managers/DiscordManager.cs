using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscordManager : MonoBehaviour
{
    private Discord.Discord discord;
    private bool isDiscordOpen = false;

    void Start()
    {
        // Try to initialize Discord only if it's already running
        if (DiscordIsRunning())
        {
            try
            {
                discord = new Discord.Discord(1270383527174602752, (ulong)Discord.CreateFlags.Default);
                isDiscordOpen = true;
                ChangeActivity();
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Error initializing Discord: " + e.Message);
                isDiscordOpen = false;
            }
        }
        else
        {
            Debug.LogWarning("Discord is not running. Skipping Discord initialization.");
            isDiscordOpen = false;
        }
    }

    // Check if Discord is running before initializing
    private bool DiscordIsRunning()
    {
        // Use the Discord API to check if Discord is running
        try
        {
            var discordProcess = System.Diagnostics.Process.GetProcessesByName("Discord");
            return discordProcess.Length > 0;
        }
        catch
        {
            return false;
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
                State = "Playing " + LevelInfo.Instance.Level.Name,
                Assets =
                {
                    LargeImage = "logo_anima_s_light", // Larger Image Asset Value
                    LargeText = ">:D",
                }
            };
            activityManager.UpdateActivity(activity, (res) =>
            {
                if (res != Discord.Result.Ok)
                {
                    Debug.LogWarning("Failed to update Discord activity.");
                }
                else
                {
                    Debug.Log("Activity updated!");
                }
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
                if (res != Discord.Result.Ok)
                {
                    Debug.LogWarning("Failed to update Discord activity.");
                }
                else
                {
                    Debug.Log("Activity updated!");
                }
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
