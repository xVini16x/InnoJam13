using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour, CommandExecuter
{
    [SerializeField] private List<PlayerInputMap> PlayerInputMaps;
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] public Transform PickUpHostAnchor;

    private float _health;
    public ExecuterType GetExecuterType()
    {
        return ExecuterType.Player;
    }

    public Transform GetExecuterTransform()
    {
        return this.transform;
    }

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

    public void SetHealth(float health)
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
