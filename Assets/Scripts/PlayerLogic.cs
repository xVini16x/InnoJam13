using System;
using System.Collections.Generic;
using Events;
using UniRx;
using UnityEngine;

public class PlayerLogic : MonoBehaviour, CommandExecuter
{
    [SerializeField] private List<PlayerInputMap> PlayerInputMaps;
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] public Transform PickUpHostAnchor;

    private float _health;

    private void Start()
    {
        startPosition = transform.position;
        SetHealth(maxHealth);
        MessageBroker.Default.Receive<PlayerHealthChanged>()
            .TakeUntilDestroy(this)
            .Subscribe(HealthChanged);
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
                        if (current.Command.DoCommand(this))
                        {
                            return;
                        }
                    }

                    break;
                case InputType.ButtonUp:
                    if (Input.GetKeyUp(current.KeyCode))
                    {
                        if (current.Command.DoCommand(this))
                        {
                            return;
                        }
                    }
                    break;
                case InputType.ButtonHold:
                    if (Input.GetKey(current.KeyCode))
                    {
                        if (current.Command.DoCommand(this))
                        {
                            return;    
                        }
                        
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

    public ExecuterType GetExecuterType()
    {
        return ExecuterType.Player;
    }

    public Transform GetExecuterTransform()
    {
        return this.transform;
    }

    public void SetHealth(float health)
    {
        _health = health;
        MessageBroker.Default.Publish(new PlayerHealthChanged
        {
            NewPlayerHealth = _health,
            MaxPlayerHealth = maxHealth
        });

        if (_health <= 0f)
        {
            Destroy(gameObject);
        }
    }
    
    private void HealthChanged(PlayerHealthChanged data)
    {
        if (data.NewPlayerHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        MessageBroker.Default.Publish(new PlayerDeath());
        transform.position = startPosition;
        SetHealth(maxHealth);
    }

    private Vector3 startPosition;
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
