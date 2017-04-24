using System;
using UnityEngine;
using System.Collections;   
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Square
{
    public Square(Vector2 min, Vector2 max)
    {
        this.min = min;
        this.max = max;
    }

    public Vector2 min;
    public Vector2 max;
}

public class GenerateStructure : MonoBehaviour
{
    private int heightOfLevel;
    private int noLevels;
    private float maxSquareX;
    private float maxSquareZ;
    public float randomFlucuationX;
    public float randomFlucuationZ;

    public int noSupportPillars = 8; //

    public GameObject[] buildingBlocks;
    public GameObject platform;
    public GameObject floor; //attached to the same parent object

    public enum GENERATION_METHOD
    {
        STACK_CUBES_IN_PILLARS
    }

    private float blockWidth;

    public GENERATION_METHOD generationMethod;

    private float squareHeight;

	// Use this for initialization
	void Start ()
	{
	    noSupportPillars = 8;
	    heightOfLevel = GameManager.Instance.heightOfLevel;
	    noLevels = GameManager.Instance.noOfLevels;
	    buildingBlocks[0].GetComponent<Rigidbody>().mass = GameManager.Instance.massOfBlock;
        buildingBlocks[0].transform.localScale = new Vector3(GameManager.Instance.sizeOfBlock, GameManager.Instance.sizeOfBlock, GameManager.Instance.sizeOfBlock);
	    platform.transform.localScale = new Vector3(GameManager.Instance.sizeOfPlatform, platform.transform.localScale.y, GameManager.Instance.sizeOfPlatform);
        floor.transform.localScale = new Vector3(GameManager.Instance.sizeOfPlatform, floor.transform.localScale.y, GameManager.Instance.sizeOfPlatform);
        floor.transform.position += new Vector3((GameManager.Instance.sizeOfPlatform - 10)/2.0f, 0, (GameManager.Instance.sizeOfPlatform - 10) / 2.0f);
        platform.GetComponent<Rigidbody>().mass = GameManager.Instance.massOfPlatform;

	    randomFlucuationX = GameManager.Instance.randomFluctuationX;
	    randomFlucuationZ = GameManager.Instance.randomFluctuationZ;
	    blockWidth = buildingBlocks[0].transform.localScale.x/2.0f;

        if (generationMethod == GENERATION_METHOD.STACK_CUBES_IN_PILLARS)
        {
            maxSquareX = platform.transform.localScale.x;   
	        maxSquareZ = platform.transform.localScale.z;
            //GameObject basePlatform = (GameObject)GameObject.Instantiate(platform, transform.position + new Vector3(maxSquareX/2.0f, -0.2f, maxSquareZ/2.0f), Quaternion.identity);
            //Destroy(basePlatform.GetComponent<Rigidbody>());
            Vector2 averagePosLastLevel = new Vector2(-1,-1);

	        for (int i = 0; i < noLevels; i++)
	        {
                List<Square> squares = PartitionSpaceIntoSquares(maxSquareX, maxSquareZ, noSupportPillars);

	            int extraPillar = 0;
	            if (i == 0) extraPillar = 1;
	            float averageXThisLevel = 0;
	            float averageZThisLevel = 0;
                List<Vector2> pillarsThisFloor = new List<Vector2>(); //for storing 4 pillars to calculate position of 5th pillar
                for (int ii = 0; ii < noSupportPillars + extraPillar; ii++)
	            {
	                float randomFlucX = randomFlucuationX;
	                float randomFlucZ = randomFlucuationZ;
	                //spawn random support pillars
	                float randomX = 0;
	                float randomZ = 0;
	                if (noSupportPillars%2 == 0 && ii != noSupportPillars) //if even amount of pillars
	                {
	                    Square square = squares[ii];
                           
                        //Debug.Log("Min: " + square.Min);
                        //Debug.Log("Max: " + square.Max);
	                    square.min.x = Mathf.Clamp(square.min.x, transform.position.x, Mathf.Infinity);
	                    square.min.y = Mathf.Clamp(square.min.y, transform.position.z, Mathf.Infinity);

	                    square.max.x -= blockWidth + randomFlucX;
	                    square.max.y -= blockWidth + randomFlucZ;
	                    square.min.x += blockWidth + randomFlucX;
	                    square.min.y += blockWidth + randomFlucZ;
                     //   if (square.max.x < transform.position.x + (maxSquareX/2.0f))
	                    //    square.max.x = Mathf.Clamp(square.max.x, -Mathf.Infinity, transform.position.x + (maxSquareX /2.0f) - blockWidth);
                     //   else
                     //       square.max.x = Mathf.Clamp(square.max.x, -Mathf.Infinity, transform.position.x + (maxSquareX / 2.0f) - blockWidth);

                     //   if (square.max.y < transform.position.z + (maxSquareZ/2.0f))
	                    //    square.max.y = Mathf.Clamp(square.max.y, -Mathf.Infinity,
	                    //        transform.position.z + (maxSquareZ/2.0f) - blockWidth);
	                    //else
	                    //    square.max.y = Mathf.Clamp(square.max.y, -Mathf.Infinity,
	                    //        transform.position.z + (maxSquareZ) - blockWidth);


                        Debug.Log("Max X: " + square.max.x);
                        randomX = Random.Range(square.min.x, square.max.x);
                        randomZ = Random.Range(square.min.y, square.max.y);

                        Debug.Log(randomX + ", " + randomZ);

                        //ADD BIAS
	                    if (i != 0)
	                    {
	                        Vector2 distanceFromCentre = averagePosLastLevel -
	                                                     new Vector2(transform.position.x + maxSquareX/2.0f, transform.position.z + maxSquareZ/2.0f);
	                        distanceFromCentre *= 1;
	                        randomX += distanceFromCentre.x;
	                        randomZ += distanceFromCentre.y;

	                        randomX = Mathf.Clamp(randomX, transform.position.x + blockWidth,
	                            transform.position.x + maxSquareX - blockWidth);
                            randomZ = Mathf.Clamp(randomZ, transform.position.z + blockWidth,
                             transform.position.z + maxSquareZ - blockWidth);
                        }
                        pillarsThisFloor.Add(new Vector2(randomX, randomZ));
                    }

	                else
	                {
	                    float averageX = 0;
	                    float averageZ = 0;
	                    foreach (Vector2 pillarPos in pillarsThisFloor)
	                    {
	                        averageX += pillarPos.x;
	                        averageZ += pillarPos.y;
	                    }

	                    averageX /= pillarsThisFloor.Count;
	                    averageZ /= pillarsThisFloor.Count;

	                    randomX = averageX;
	                    randomZ = averageZ;

	                    //randomX = transform.position.x + Random.Range(transform.position.x, transform.position.x + maxSquareX);
	                    //randomZ = transform.position.z + Random.Range(transform.position.z, transform.position.z + maxSquareZ);
	                    //    Debug.Log(transform.position.x + ", " + transform.position.z);
	                }
	                averageXThisLevel += randomX;
	                averageZThisLevel += randomZ;


                    for (int iii = 0; iii < heightOfLevel; iii++)
                    {
                        float blockY = buildingBlocks[0].transform.localScale.y;
                        GameObject newBlock = (GameObject) GameObject.Instantiate(buildingBlocks[0],
                            new Vector3(randomX + Random.Range(-randomFlucX, randomFlucX),
                                (i*heightOfLevel*blockY + platform.transform.localScale.y) + (iii * blockY),
	                            randomZ + Random.Range(-randomFlucZ, randomFlucZ)), Quaternion.identity);

	                    newBlock.transform.parent = transform;

	                    randomFlucX /= 2.0f;
	                    randomFlucZ /= 2.0f;
	                }
	            }

	            if (i == 0)
	            {
                    averageXThisLevel /= noSupportPillars + 1;
                    averageZThisLevel /= noSupportPillars + 1;
                }
	            else
	            {
                    averageXThisLevel /= noSupportPillars;
                    averageZThisLevel /= noSupportPillars;
                }

                Debug.DrawRay(new Vector3(averageXThisLevel, (i + 1) * heightOfLevel, averageZThisLevel), Vector2.up, Color.green, 100);
	            averagePosLastLevel.x = averageXThisLevel;
	            averagePosLastLevel.y = averageZThisLevel;

                float blockHeightY = buildingBlocks[0].transform.localScale.y;

                //when we are done spawning all the pillars for this level
                GameObject newPlatform = (GameObject)GameObject.Instantiate(platform,
	                new Vector3(transform.position.x + maxSquareX/2.0f, (i + 1)*heightOfLevel* blockHeightY,
	                    transform.position.z + maxSquareZ/2.0f),
	                Quaternion.identity);

	            newPlatform.transform.parent = transform;
	        }
	    }
	}

    List<Square> PartitionSpaceIntoSquares(float width, float height, int noSquares)
    {
        List<Square> squares = new List<Square>();
        float increment = width/(noSquares/2.0f);
        float minX = 0;
        float maxX = 0;
        float minZ = 0;
        float maxZ = 0;

        for (float i = 0; i < width; i += increment)
        {
            for (float ii = 0; ii < height; ii += increment)
            {
                minX = i;
                maxX = i + increment;
                minZ = ii;
                maxZ = ii + increment;
                Debug.Log("Min: " + minX + ", " + minZ);
                Debug.Log("Max: " + maxX + ", " + maxZ);
                squares.Add(new Square(new Vector2(transform.position.x + minX, transform.position.z + minZ), new Vector2(transform.position.x + maxX, transform.position.z + maxZ)));
            }
        }

        return squares;
    } 
}
