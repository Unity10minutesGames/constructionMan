using UnityEngine;

public class Brick : MonoBehaviour {

    enum BrickState { Good, Brocken, Destroyed};

    BrickState brickState;

	// Use this for initialization
	void Start () {
        brickState = BrickState.Good;
    }
    
	// Update is called once per frame
	void Update () {
		
	}

}
