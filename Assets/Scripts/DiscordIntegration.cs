using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Discord;
using UnityEngine.InputSystem;
using Photon.Pun;
using System;

public class DiscordIntegration : MonoBehaviour //This is probably going to be horrible and I will likely abandon it.
{

	public Discord.Discord discord;
	public Discord.Activity activity;
	public string LevelName; 
	long currenttime = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
	long endingtime = 0;

	// Use this for initialization
	void Start()
	{
		discord = new Discord.Discord(961314848145285161, (System.UInt64)Discord.CreateFlags.Default);
		var activityManager = discord.GetActivityManager();
		print(SceneManager.GetActiveScene().buildIndex);
		activity = new Discord.Activity
		{
			Details = "On The Main Menu",
			Assets =
			{
				LargeImage = "title"
			},
			Instance = false,
		};
		activityManager.UpdateActivity(activity, (res) =>
		{
			if (res == Discord.Result.Ok)
			{
				Debug.Log("Discord Status Changed!");
			}
			else
			{
				Debug.Log("Discord Status Failed!");
			}
		});
	}

	// Update is called once per frame
	void Update()
	{
		discord.RunCallbacks();
		//TODO: Clear status when exiting game
	}

    private void OnDestroy()
    {
        discord.Dispose();
    }

}