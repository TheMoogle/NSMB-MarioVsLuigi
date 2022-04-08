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
	public string LevelName; 
	long currenttime = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
	long endingtime = 0;

	// Use this for initialization
	void Start()
	{
		discord = new Discord.Discord(961314848145285161, (System.UInt64)Discord.CreateFlags.Default);
		var activityManager = discord.GetActivityManager();
		print(SceneManager.GetActiveScene().buildIndex);
		endingtime = currenttime + (int)PhotonNetwork.CurrentRoom.CustomProperties[Enums.NetRoomProperties.Time] + 7; //TODO: Make this time start when the scene is fully loaded, the extra 7 seconds is a very temporary workaround
		var activity = new Discord.Activity
			{
			Details = "Playing on the " + LevelName + " map",
			State = "Players:",
			Assets =
			{
			LargeImage = "title"
			},
			Timestamps =
			{
			End = endingtime
			},
			Party =
            {
				Size =
                {
					MaxSize = 10, //TODO: Make this accurate to the maximum lobby size
					CurrentSize = PhotonNetwork.CurrentRoom.PlayerCount
				}
            }
		};
		activityManager.ClearActivity((result) =>
		{
			if (result == Discord.Result.Ok)
			{
				Debug.Log("Success!");
			}
			else
			{
				Debug.Log("Failed");
			}
		});
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
}