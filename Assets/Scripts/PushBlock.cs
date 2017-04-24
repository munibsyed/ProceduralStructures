using UnityEngine;
using System.Collections;

public class PushBlock : MonoBehaviour
{
    public float force;
    public float speed;

 
	// Update is called once per frame
	void Update () {

	    if (Input.GetMouseButtonDown(0))
	    {
	        RaycastHit hit;
	        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
	        {
	            GameObject hitBlock = hit.collider.gameObject;
                if (hitBlock.CompareTag("Block"))
	            {
	                hitBlock.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * force);
	            }
	        }
	    }

	    if (Input.GetKey(KeyCode.W))
	    {
	        transform.position += transform.forward * speed;
	    }

	    if (Input.GetKey(KeyCode.S))
	    {
	        transform.position -= transform.forward*speed;
	    }

	    if (Input.GetKey(KeyCode.A))
	    {
	        transform.position -= transform.right*speed;
	    }

	    if (Input.GetKey(KeyCode.D))
	    {
	        transform.position += transform.right*speed;
	    }

	    if (Input.GetKey(KeyCode.Q))
	    {
	        transform.position += transform.up*speed;
	    }

	    if (Input.GetKey(KeyCode.E))
	    {
	        transform.position -= transform.up*speed;
	    }

	    if (Input.GetKeyDown(KeyCode.KeypadEnter))
	    {
	        Application.LoadLevel(0);
	    }
	}
}
