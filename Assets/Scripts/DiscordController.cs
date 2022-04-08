using Discord;
using UnityEngine;

public class DiscordController : MonoBehaviour
{
#if UNITY_EDITOR || UNITY_STANDALONE

    const long clientId = 962092229512531968;

    public static DiscordController Instance { get; private set; }
    public static Discord.Discord discord;
    public static Discord.Activity presence;

    void Start()
    {
        try
        {
            discord = new Discord.Discord(clientId, (ulong)CreateFlags.NoRequireDiscord);
            Instance = this;
            discord.GetActivityManager().UpdateActivity(presence, (c) => {});
            Debug.Log("Discord Online");
        }
        catch
        {
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        discord.RunCallbacks();
    }

    public static void ChangeLobbyStatus(int current, int max)
    {
        try
        {
            presence.Party.Size.CurrentSize = current;
            presence.Party.Size.MaxSize = max;
            discord.GetActivityManager().UpdateActivity(presence, (c) => {});
        }
        catch
        { }
    }

#endif
}
