using UnityEngine;
using UnityEngine.Tilemaps;

using Fusion;
using NSMB.Utils;

[RequireComponent(typeof(GameManager))]
public class GameEventRpcs : NetworkBehaviour {

    //---Static Variables
    private static readonly Vector3 OneFourth = new(0.25f, 0.25f, 0f);

    //---Private Variables
    private GameManager gm;

    public void Awake() {
        gm = GetComponent<GameManager>();
    }

    //---TILES
    public void BumpBlock(short x, short y, ushort oldTile, ushort newTile, bool downwards, Vector2 offset, bool spawnCoin, NetworkPrefabRef spawnPrefab) {
        Vector3Int loc = new(x, y, 0);

        Vector3 spawnLocation = Utils.TilemapToWorldPosition(loc) + OneFourth;

        Runner.Spawn(PrefabList.Instance.Obj_BlockBump, spawnLocation, onBeforeSpawned: (runner, obj) => {
            obj.GetComponentInChildren<BlockBump>().OnBeforeSpawned(loc, oldTile, newTile, spawnPrefab, downwards, spawnCoin, offset);
        });
    }

    public void BumpBlock(short x, short y, TileBase oldTile, TileBase newTile, bool downwards, Vector2 offset, bool spawnCoin, NetworkPrefabRef spawnPrefab) {
        Vector3Int loc = new(x, y, 0);

        Vector3 spawnLocation = Utils.TilemapToWorldPosition(loc) + OneFourth;

        Runner.Spawn(PrefabList.Instance.Obj_BlockBump, spawnLocation, onBeforeSpawned: (runner, obj) => {
            obj.GetComponentInChildren<BlockBump>().OnBeforeSpawned(loc, oldTile, newTile, spawnPrefab, downwards, spawnCoin, offset);
        });
    }

    //---GAME STATE
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void Rpc_EndGame(int team) {
        if (gm.GameEnded)
            return;

        // TODO: don't use a coroutine?
        // eh, it should be alrite, since it's an RPC and isn't predictive.
        StartCoroutine(gm.EndGame(team));
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void Rpc_LoadingComplete() {
        // Populate scoreboard
        ScoreboardUpdater.Instance.CreateEntries(gm.AlivePlayers);
        if (Settings.Instance.genericScoreboardAlways)
            ScoreboardUpdater.Instance.SetEnabled();

        GlobalController.Instance.loadingCanvas.EndLoading();
    }
}
