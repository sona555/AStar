using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Class that holds information about a 
//certain position, so it is used in a 
//pathfinding algorithm.

class Node
{
    //Every node may have different values,
    //according to your game/application
    public enum Value
    {
        FREE,
        BLOCKED
    }
    //Nodes have X and Y positions
    //(Horizontal and vertical).
    public int posX;
    public int posY;

    //G is a basic *cost* value to go from one node to another
    public int g = 0;

    //H is a heuristic that *estimates* the cost of the closest path.
    public int f =0;

    //Nodes have references to other nodes so it is possible to build a path.
    public Node parent = null;

    //The value of the node
    public Value value;

    //Constructor.
    public Node(int posX, int posY)
    {
        this.posX = posX;
        this.posY = posY;

        value = Value.FREE;
    }


}
public class AStar : MonoBehaviour
{
    //Constants.
    private const int MAP_SIZE = 6;


    //Variables.
    private List<string> map;
    private Node[,] nodeMap;


    // Start is called before the first frame update
    void Start()
    {
        map = new List<string>();
        map.Add("G-----");
        map.Add("XXXXX-");
        map.Add("S-X-X-");
        map.Add("--X-X-");
        map.Add("--X-X-");
        map.Add("------");
    
        //parse the map.
        nodeMap = new Node[MAP_SIZE, MAP_SIZE];
        Node start = null;
        Node goal = null;

        for(int y= 0; y< MAP_SIZE; y++){
            for(int x = 0; x<MAP_SIZE; x++) {
              Node node = new Node(x, y);

              char currentChar = map[y] [x];
              if(currentChar == "X") {
                    node.value = Node.Value.BLOCKED;
               } else if (currentChar == 'G') {
                    goal = node;
               } else if (currentChar == "S") {
                    start = node;
                }
              nodeMap[x,y] = node;
            }
        }

        //Print the map.
        string mapString = "";
        foreach (string mapRow in map)
        {
            mapString += mapRow + "\n";
        }
        Debug.Log(mapString);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
