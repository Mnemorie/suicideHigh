using System;
using UnityEngine;
using System.Collections.Generic;

public class ShootOut : MonoBehaviour
{
    public enum EState
    {
        PlayersJoining,
        Playing,
        WaitingForNextRound,
    }

    public EState State;

    // Use this for initialization
    void Start()
    {
        State = EState.PlayersJoining;
        LevelResult = ELevelResult.None;
    }

    public enum ELevelResult
    {
        None,
        Draw,
        Won
    }
    public ELevelResult LevelResult;

    // Update is called once per frame
    void Update()
    {
        if (State == EState.PlayersJoining)
        {
            if (Characters.Count > 1)
            {
                LevelStartTimer -= Time.deltaTime;
            }

            if (LevelStartTimer < 0)
            {
                State = EState.Playing;
                foreach (Character c in Characters)
                {
                    c.Go = true;
                }

                // FindObjectOfType<TrafficLight>().Go();
            }
        }
        else if (State == EState.Playing)
        {
            List<Character> deadCharacters = Characters.FindAll(c => c.IsDead);
            if (deadCharacters.Count == Characters.Count)
            {
                LevelResult = ELevelResult.Draw;
                TriggerWaitingForNextLevel();
            }
            else if (deadCharacters.Count == Characters.Count - 1)
            {
                Character winner = Characters.Find(c => !c.IsDead);
                LevelResult = ELevelResult.Won;
                TriggerWaitingForNextLevel();
            }
        }
        else if (State == EState.WaitingForNextRound)
        {
            NextLevelTimer -= Time.deltaTime;
            if (NextLevelTimer <= 0)
            {
                SpawnNextLevel();
                State = EState.PlayersJoining;
                LevelStartTimer = TimeToStartLevelWhenPlayersReady;
            }
        }
    }

    public float TimeBetweenLevels = 5;
    private float NextLevelTimer;

    public float TimeToStartLevelWhenPlayersReady = 3;
    private float LevelStartTimer;

    void TriggerWaitingForNextLevel()
    {
        NextLevelTimer = TimeBetweenLevels;
        State = EState.WaitingForNextRound;
    }

    void SpawnNextLevel()
    {
        List<Character> droppedCharacters = Characters.FindAll(c => !c.IsReadyForNextRound && c.IsDead);
        Characters.RemoveAll(droppedCharacters.Contains);
        droppedCharacters.ForEach(c => Destroy(c.gameObject));

        for (int i = 0; i < Characters.Count; ++i)
        {
            Character character = Characters[i];
            if (!character.IsDead)
            {
                continue;
            }

            Character respawnedCharacter = (Character)Instantiate(CharacterTemplate, character.transform.position, character.transform.rotation);

            respawnedCharacter.KeyCode = character.KeyCode;
            respawnedCharacter.Body.material.color = character.Body.material.color;

            Characters[i] = respawnedCharacter;
            Destroy(character.gameObject);
        }

        RepositionCharacters();
    }

    void OnGUI()
    {
        if (State != EState.PlayersJoining ||
            Event.current == null ||
            Event.current.keyCode == KeyCode.None ||
            Event.current.keyCode == KeyCode.F1 ||
            Event.current.type != EventType.KeyDown ||
            IsOccupied(Event.current.keyCode))
        {
            return;
        }

        Debug.Log("new contender! " + Event.current.keyCode);
        CreatePlayer(Event.current.keyCode);
    }

    public Character CharacterTemplate;
    public List<Character> Characters = new List<Character>();

    public void CreatePlayer(KeyCode keyCode)
    {
        Character newCharacter = Instantiate(CharacterTemplate);
        newCharacter.KeyCode = keyCode;

        Characters.Add(newCharacter);

        RepositionCharacters();

        LevelStartTimer = TimeToStartLevelWhenPlayersReady;
    }

    public bool IsOccupied(KeyCode key)
    {
        foreach (Character c in Characters)
        {
            if (c.KeyCode == key)
            {
                return true;
            }
        }
        return false;
    }

    public float DistanceFromCenter = 5;

    private void RepositionCharacters()
    {
        float positionalAngle = 360f / Characters.Count;

        for (int i = 0; i < Characters.Count; ++i)
        {
            Character c = Characters[i];

            c.transform.position = transform.position + (Quaternion.AngleAxis(i * positionalAngle, Vector3.up) * Vector3.forward * DistanceFromCenter);
            c.transform.LookAt(transform);
        }
    }
}
