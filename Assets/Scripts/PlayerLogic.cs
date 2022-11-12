using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    [SerializeField] private List<PlayerInputMap> PlayerInputMaps;
    [SerializeField] private float maxHealth = 10f;

    private float _health;

    private void Start()
    {
        _health = maxHealth;
    }

    private void Update()
    {
        for (int i = 0; i < PlayerInputMaps.Count; i++)
        {
            var current = PlayerInputMaps[i];
            if (current.Command == null)
            {
                Debug.LogError("no command set");
                continue;
            }

            switch (current.InputType)
            {
                case InputType.ButtonDown:
                    if (Input.GetKeyDown(current.KeyCode))
                    {
                        current.Command.DoCommand();
                    }

                    break;
                case InputType.ButtonUp:
                    if (Input.GetKeyUp(current.KeyCode))
                    {
                        current.Command.DoCommand();
                    }

                    break;
                default:
                    Debug.LogError("not supported yet");
                    break;
            }
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.transform.GetComponent<EnemyLogic>() != null)
        {
            // TODO: let enemy logic configure their attack strength
            SetHealth(_health - 2f * Time.deltaTime);
        }
    }

    private void SetHealth(float health)
    {
        _health = health;
        if (_health <= 0f)
        {
            Destroy(gameObject);
        }
    }
}

[Serializable]
public class PlayerInputMap
{
    public ICommand Command;
    public InputType InputType;
    public KeyCode KeyCode;
}

public enum InputType
{
    ButtonDown,
    ButtonUp,
    ButtonHold,
}