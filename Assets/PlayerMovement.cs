using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float speed;
    [SerializeField] float RotationSpeed;
    [SerializeField] Animator anim;  
    public Joystick joystick;
    public Vector2 direction;

    public Stack[] stacks;
    public Transform newStackPosTransform;
    public Vector3 newStackPos;
    public List<GameObject> currentStack;


    public Transform bridgePos;
    private void Start()
    {
        stacks = GameManager.Instance.stacks;
    }
    void Update()
    {    
        JoystickMovement();
        //foreach (GameObject item in currentStack)
        //{
        //    if (!item)
        //    {
        //        currentStack.Remove(item);
        //    }
        //}
        NewPos();
        
        //for (int i = 0; i < currentStack.Count; i++)
        //{
        //    if (!currentStack[i])
        //    {
        //        currentStack.RemoveAt(i);
        //    }
        //}
    }


    public void JoystickMovement()
    {
        if(Input.GetMouseButton(0))
        {
            Vector3 joystickDirection = joystick.Direction;
            Vector3 _direction = new Vector3(joystickDirection.x, 0, joystickDirection.y);
            direction = joystickDirection;
            if (_direction.magnitude != 0)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_direction), RotationSpeed * Time.deltaTime);
                transform.position += transform.forward * speed * Time.deltaTime;
            }
            anim.SetBool("IsRunning", true);
        }
        else
        {
            anim.SetBool("IsRunning", false);
        }
   

    }
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.tag=="Red")
        {           
            IncreaseStack("Red");
            StartCoroutine(other.GetComponent<Ball>().Regenerate());
        }
        else if (other.tag=="Green")
        {
            IncreaseStack("Green");
            StartCoroutine(other.GetComponent<Ball>().Regenerate());
        }
        else if (other.tag=="Yellow")
        {
            IncreaseStack("Yellow");
            StartCoroutine(other.GetComponent<Ball>().Regenerate());   
        }
        if (other.tag == "Gate")
        {
            for (int i = 4; i < currentStack.Count; i++)
            {
                currentStack[i].transform.parent = null;
                currentStack[i].GetComponent<Rigidbody>().AddForce(-transform.forward * 100);
                currentStack[i].GetComponent<Rigidbody>().isKinematic = false ;
                currentStack[i].GetComponent<Collider>().enabled = true;
                currentStack[i].GetComponent<Trigger>().isExtra = true;          
                Destroy(currentStack[i], 2);
                currentStack.Remove(currentStack[i]);
            }
  
        }
        if (other.tag == "Bridge")
        {
            if (other.GetComponent<Trigger>().isTriggered)
            {
                return;
            }

            if (currentStack.Count > 0)
            {
                other.GetComponent<Trigger>().isTriggered = true;
                BuildBridge(currentStack[currentStack.Count - 1]);
            }
        }
        else if (other.tag == "Finish")
        {
            GameManager.Instance.GameOver();
        }
    }
    public GameObject lastStack;
    private void NewPos()
    {
        if (currentStack.Count > 0)
        {
            lastStack = currentStack[currentStack.Count - 1];
            newStackPos = lastStack.transform.position + Vector3.up * (0.5f / 2);
        }
        else
        {
            newStackPos = newStackPosTransform.position;
        }
    }

    private void IncreaseStack(string color)
    {
        for (int i = 0; i < stacks.Length; i++)
        {
            if (stacks[i].color==color)
            {
                if (currentStack.Count>0)
                {
                    currentStack.Add(Instantiate(stacks[i].obj, newStackPos, currentStack[currentStack.Count - 1].transform.rotation, newStackPosTransform));                 
                }
                else
                {
                    currentStack.Add(Instantiate(stacks[i].obj, newStackPos, Quaternion.identity, newStackPosTransform));          
                }
                
            }
           
        }
    }

    public List<GameObject> bridge;
    private void BuildBridge(GameObject newBrick)
    {
        if (currentStack.Count>0)
        {
            bridge.Add(newBrick);
            newBrick.transform.parent = null;
            newBrick.transform.rotation = Quaternion.identity;
            newBrick.transform.position = bridgePos.position;
            newBrick.transform.localScale += Vector3.right * 0.5f; 
            newBrick.transform.localScale += Vector3.forward * 0.3f; 
            newBrick.GetComponent<Collider>().enabled = true;
            bridgePos.position += Vector3.forward * 0.62f;
            currentStack[currentStack.Count - 1].GetComponent<Rigidbody>().useGravity = false;
            currentStack.Remove(currentStack[currentStack.Count - 1]);
        }
       
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Bridge")
        {
            Debug.Log("Here");
            if (collision.collider.GetComponent<Trigger>())
            {
                if (!collision.collider.GetComponent<Trigger>().isExtra && !collision.collider.GetComponent<Trigger>().isTriggered)
                {
                   
                    if (currentStack.Count>0)
                    {
                        BuildBridge(currentStack[currentStack.Count - 1]);
                        collision.collider.GetComponent<Trigger>().isTriggered = true;
                    }
                 
                }
            }
        }
    }
    
}