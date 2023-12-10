using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// The data structure of a player.
/// </summary>
[System.Serializable]
public class PlayerData
{
    public GameObject playerParentObj;
    public int playerIdx;
    public string playerId;
    public string playerName;
    public bool isInCheck;

    public int money;
    public List<Structures> structs = new();

}

public class PlayerTurnHandler : MonoBehaviour
{
    [SerializeField] private PlayerData playerOne = null;
    [SerializeField] private PlayerData playerTwo = null;

    public int PlayerTurnIdx { get; set; }
    public Piece CurrentClickedPiece { get; set; }
    public bool GameIsOver { get; set; }
    public int MoveCount { get; set; }

    public void Init()
    {
        PlayerTurnIdx = 1;
        MoveCount = 1;
        GameIsOver = false;
        InitPlayersData();

        // No need to validate the moves, no player is in check at the gaem start.
        SetAllPiecesMoves(false);
    }

    /// <summary>
    /// Init the players data.
    /// </summary>
    private void InitPlayersData()
    {
        playerOne.playerIdx = 1;
        playerOne.playerId = "White";
        playerOne.playerName = "White";
        playerOne.isInCheck = false;
        playerOne.money = 1000;
        InitPlayerStructs(playerOne.playerIdx);

        playerTwo.playerIdx = -1;
        playerTwo.playerId = "Black";
        playerTwo.playerName = "Black";
        playerTwo.isInCheck = false;
        playerTwo.money = 1000;
        InitPlayerStructs(playerTwo.playerIdx);
    }

    public void AddMoneyToCurrentPlayer()
    {
        AddMoney(PlayerTurnIdx);
    }

    public void AddMoney(int playerIdx)
    {
        for (int i = 0; i < GetPlayer(playerIdx).structs.Count; i++)
        {
            print("count " + GetPlayer(playerIdx).structs.Count + " id " + i);
            GetPlayer(playerIdx).money += GetPlayer(playerIdx).structs[i].GetMoneyPerTurn();
        }
    }

    /// <summary>
    /// Conversion from the player's index of 1, -1 to the phayer's name.
    /// </summary>
    public string GetPlayerName(int playerIdx)
    {
        return playerOne.playerIdx == playerIdx ? playerOne.playerName : playerTwo.playerName;
    }

    /// <summary>
    /// Get the name of the current player.
    /// </summary>
    public string GetCurrentPlayerName()
    {
        return GetPlayerName(PlayerTurnIdx);
    }

    public string GetPlayerMoney(int playerIdx)
    {
        return playerOne.playerIdx == playerIdx ? playerOne.money.ToString() : playerTwo.money.ToString();
    }

    public string GetCurrentPlayerMoney()
    {
        return GetPlayerMoney(PlayerTurnIdx);
    }

    /// <summary>
    /// Conversion from the player's index of 1, -1 to the phayer's id.
    /// </summary>
    public string GetPlayerId(int playerIdx)
    {
        return playerOne.playerIdx == playerIdx ? playerOne.playerId : playerTwo.playerId;
    }

    /// <summary>
    /// Conversion from the player's index of 1, -1 to the player.
    /// </summary>
    public PlayerData GetPlayer(int playerIdx)
    {
        return playerOne.playerIdx == playerIdx ? playerOne : playerTwo;
    }

    /// <summary>
    /// Conversion from the player's index of 1, -1 to the player's parent object.
    /// </summary>
    public GameObject GetPlayerParentObj(int playerIdx)
    {
        return playerOne.playerIdx == playerIdx ? playerOne.playerParentObj : playerTwo.playerParentObj;
    }

    /// <summary>
    /// Get a list of all the pieces of a player.
    /// </summary>
    public List<Piece> GetPlayerPieces(int playerIdx)
    {
        List<Piece> pieces = new List<Piece>();
        foreach (Transform child in GetPlayerParentObj(playerIdx).transform)
        {
            pieces.Add(child.GetComponent<Piece>());
        }

        return pieces;
    }

    /// <summary>
    /// Set the given player's check status to the given status.
    /// </summary>
    /// <param name="playerIdx"> The player that's check status has changed. </param>
    /// <param name="checkStatus"> The updated check status of the given player. </param>
    public void SetCheckStatus(int playerIdx, bool checkStatus)
    {
        GetPlayer(playerIdx).isInCheck = checkStatus;
    }

    /// <summary>
    /// Get the check status of a player.
    /// </summary>
    public bool GetCheckStatus(int playerIdx)
    {
        return GetPlayer(playerIdx).isInCheck;
    }

    /// <summary>
    /// Get a list of all the pieces.
    /// </summary>
    public List<Piece> GetAllPieces()
    {
        List<Piece> pieces = new List<Piece>();
        Transform parentTransform = playerOne.playerParentObj.transform;
        foreach (Transform child in parentTransform)
        {
            pieces.Add(child.GetComponent<Piece>());
        }

        parentTransform = playerTwo.playerParentObj.transform;
        foreach (Transform child in parentTransform)
        {
            pieces.Add(child.GetComponent<Piece>());
        }

        return pieces;
    }

    /// <summary>
    /// Get a list of all the pieces data.
    /// </summary>
    public List<PieceData> GetAllPiecesData()
    {
        List<PieceData> piecesData = new List<PieceData>();
        Transform parentTransform = playerOne.playerParentObj.transform;

        Piece currentPiece;
        foreach (Transform child in parentTransform)
        {
            currentPiece = child.GetComponent<Piece>();
            piecesData.Add(currentPiece.pieceData);
        }

        parentTransform = playerTwo.playerParentObj.transform;
        foreach (Transform child in parentTransform)
        {
            currentPiece = child.GetComponent<Piece>();
            piecesData.Add(currentPiece.pieceData);
        }

        return piecesData;
    }

    /// <summary>
    /// Set the moves of all the pieces.
    /// </summary>
    /// <param name="validateMoves"> Whether to also validate the moves on the chack status or not. </param>
    public void SetAllPiecesMoves(bool validateMoves = false)
    {
        Transform parentTransform = playerOne.playerParentObj.transform;
        List<Piece> units = new();
        List<Piece> structures = new();
        foreach (Transform child in parentTransform)
        {
            if (true)//(!child.GetComponent<Piece>().pieceData.isDead)
            {
                print("name: " + child.name);
                if (child.name == "Units")
                {                    
                    foreach (Transform unit in child)
                    {
                        units.Add(unit.GetComponent<Piece>());
                    }                        
                }
                if (child.name == "Structures")
                {
                    foreach (Transform unit in child)
                    {
                        structures.Add(unit.GetComponent<Piece>());
                    }
                }
            }
        }

        for (int i = 0; i < units.Count; i++)
        {
            print("name = " + units[i].name);
        }
        for (int i = 0; i < structures.Count; i++)
        {
            print("name = " + structures[i].name);
            print("at x" + structures[i].transform.position.x + " y " + structures[i].transform.position.y + " z " + structures[i].transform.position.z);
        }

        parentTransform = playerTwo.playerParentObj.transform;
        foreach (Transform child in parentTransform)
        {
            if (true) //(!child.GetComponent<Piece>().pieceData.isDead)
            {
                foreach (Transform childs in child)
                {
                   // childs.GetComponent<Piece>().SetMoves(validateMoves);
                }
            }
        }
    }

    void InitPlayerStructs(int playerIdx)
    {
        Transform parentTransform = GetPlayer(playerIdx).playerParentObj.transform;
        foreach (Transform child in parentTransform)
        {
                print("name: " + child.name);
                if (child.name == "Structures")
                {
                    foreach (Transform unit in child)
                    {
                        print("add type: " + unit.GetComponent<Structures>().StructType());
                        GetPlayer(playerIdx).structs.Add(unit.GetComponent<Structures>());
                    }
                }
        }
    }
}
