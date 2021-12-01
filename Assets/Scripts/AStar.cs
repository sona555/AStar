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
    public int g = int.MaxValue;

    //H is a heuristic that *estimates* the cost of the closest path.
    public int f =int.MaxValue;

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

        for (int y = 0; y < MAP_SIZE; y++) {
            for (int x = 0; x < MAP_SIZE; x++) {
                Node node = new Node(x, y);

                char currentChar = map[y][x];
                if (currentChar.Equals('X')) {
                    node.value = Node.Value.BLOCKED;
                } else if (currentChar.Equals('G')) {
                    goal = node;
                } else if (currentChar.Equals('S')) {
                    start = node;
                }
                nodeMap[x, y] = node;
            }
        }
        //Execute the A-Start algorithm.
        List<Node> nodePath = ExecuteAStar(start, goal);

        // Burn the path of the map
        foreach(Node node in nodePath)
        {
            char[] charArray = map[node.posY].ToCharArray ();
            charArray[node.posX] = '@';
            map[node.posY] = new string(charArray);
        }


        //Print the map.
        string mapString = "";
        foreach (string mapRow in map)
        {
            mapString += mapRow + "\n";
        }
        Debug.Log(mapString);
    }

    private List<Node> ExecuteAStar(Node start, Node goal)
    {
        //This list holds the potential best path nodes that should be visited. It always starts with the origin.
        List<Node> openList = new List<Node>() { start };

        // This list keeeps track of  all nodes that have been visited.
        List<Node> closedList = new List<Node>();

        //Initialze the start node.
        start.g = 0;
        start.f = start.g + CalculateHeuristicValue(start, goal);

        while (openList.Count > 0) {
            Node current = openList[0];
            foreach (Node node in openList) {
                if (node.f < current.f) {
                    current = node;
                }
            }

            //Check if the target has been reached
            if (current == goal)
            {
                return BuildPath(goal);
            }

            //Make sure the current node will not be visited again.
            openList.Remove(current);
            closedList.Add(current);

            //Execute the algorithm in the current node's neighbours
            List<Node> neighbours = GetNeighbourNodes(current);
            foreach (Node neighbour in neighbours)
            {
                if (closedList.Contains(neighbour))
                {
                    continue;
                }

                if (!openList.Contains(neighbour))
                {
                    openList.Add(neighbour);
                }

                // Calculate a new G value and verify if this value
                //is better than whatever is stored in the neighbour.
                int candidateG = current.g + 1;
                if (candidateG >= neighbour.g)
                {
                    //if the G value is greater or equal, then this does not belong
                    //to a good path(there was a better path calculated)
                    continue;
                } else
                {
                    // Otherwise,we found a better way to reach the neighbour.
                    //Initialize the values
                    neighbour.parent = current;
                    neighbour.g = candidateG;
                    neighbour.f = neighbour.g + CalculateHeuristicValue(neighbour, goal);
                    //f = g + h
                }

            }
        }
        //If reached this point, it means that there are no more nodes to search. The algorithm failed
        return new List<Node>();
    }    

    private List<Node> GetNeighbourNodes (Node node)
    { 
        List<Node> neighbours = new List<Node>();

        if(node.posX - 1 >=0)
        {
            Node candidate = nodeMap[node.posX - 1, node.posY];
            if (candidate.value != Node.Value.BLOCKED)
            {
                neighbours.Add(candidate);
            }
        }
        if (node.posX + 1 <= MAP_SIZE - 1)
        {
            Node candidate = nodeMap[node.posX + 1, node.posY];
            if (candidate.value != Node.Value.BLOCKED)
            {
                neighbours.Add(candidate);
            }
        }
        if (node.posY - 1 >= 0)
        {
            Node candidate = nodeMap[node.posX, node.posY - 1];
            if (candidate.value != Node.Value.BLOCKED)
            {
                neighbours.Add(candidate);
            }
        }
        if (node.posY + 1 <= MAP_SIZE - 1)
        {
            Node candidate = nodeMap[node.posX, node.posY + 1];
            if (candidate.value != Node.Value.BLOCKED)
            {
                neighbours.Add(candidate);
            }
        }
        return neighbours;


    }
    //A simple estimate of distance
    //Uses the manhattan distance.
    private int CalculateHeuristicValue(Node node1, Node node2)
    {
        return Mathf.Abs(node1.posX - node2.posX) + Mathf.Abs(node1.posY - node2.posY);
    }

    private List<Node> BuildPath(Node node)
    {
        List<Node> path = new List<Node>() { node };
        while(node.parent != null)
        {
            node = node.parent;
            path.Add(node);
        }
        return path;
    }
}
