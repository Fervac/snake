using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
	enum Direction
	{
		up,
		down,
		left,
		right
	}

	Direction direction;

	public List<Transform> tail = new List<Transform>();

	public float frameRate = 0.2f;
	public float step = 0.16f;

	public GameObject TailPrefab;

	public Vector2 horizontalRange;
	public Vector2 verticalRange;

	Vector3 lastPos;

    void Start()
    {
		Invoke("Move", frameRate);
    }

	void Move()
	{
		lastPos = transform.position;
		Vector3 nextPos = Vector3.zero;
		if (direction == Direction.up)
			nextPos = Vector3.up;
		else if (direction == Direction.down)
			nextPos = Vector3.down;
		else if (direction == Direction.left)
			nextPos = Vector3.left;
		else if (direction == Direction.right)
			nextPos = Vector3.right;
		nextPos *= step;
		transform.position += nextPos;
		MoveTail();
	}

	void MoveTail()
	{
		for (int i = 0; i < tail.Count; i++)
		{
			Vector3 tmp = tail[i].position;
			tail[i].position = lastPos;
			lastPos = tmp;
		}
		Invoke("Move", frameRate);
	}

    void Update()
    {
		if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && direction != Direction.down)
			direction = Direction.up;
		else if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && direction != Direction.up)
			direction = Direction.down;
		else if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && direction != Direction.right)
			direction = Direction.left;
		else if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && direction != Direction.left)
			direction = Direction.right;
    }

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Block"))
		{
			print("Game Over");
			UnityEngine.SceneManagement.SceneManager.LoadScene(0);
		}
		else if (col.CompareTag("Food"))
		{
			tail.Add(Instantiate(TailPrefab, tail[tail.Count - 1].position, Quaternion.identity).transform);
			int x = Mathf.RoundToInt(Random.Range(horizontalRange.x, horizontalRange.y));
			int y = Mathf.RoundToInt(Random.Range(verticalRange.x, verticalRange.y));
			col.transform.position = new Vector2(x, y);
			if (frameRate > 0.1f)
				frameRate -= 0.01f;
			else if (frameRate > 0.02f)
				frameRate -= 0.001f;
		}
	}
}
