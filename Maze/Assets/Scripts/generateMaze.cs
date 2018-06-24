using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class generateMaze : MonoBehaviour {
    static Vector2[] direction = new Vector2[4] { new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, 0), new Vector2(0, -1) };
    public class step{
        public Vector2 startPos;
        public Vector2 comeFrom;
        public step(Vector2 thisStep, Vector2 comeFromWhere) {
            startPos= thisStep;
            comeFrom = comeFromWhere;
        }
    }
    static float scaleOffset = 1.0f;
    static int mapWidth=22, mapHeight = 22;
    static float wallLen=0.25f;
    float halfWallLen = wallLen / 2.0f;
    Stack<step> myStack = new Stack<step>();
    short[,] map = new short[mapHeight, mapWidth];
    

    void generateWall(int dirIndex,step nowStep) {
        GameObject obj = Resources.Load("Cube") as GameObject;
        GameObject ChildObject = Instantiate(obj);
        ChildObject.transform.parent = transform;
        if (dirIndex == 0)
        {
            ChildObject.transform.localPosition = new Vector3(nowStep.startPos.x * wallLen - 2.5f + halfWallLen, 0, nowStep.startPos.y * wallLen - 2.5f);
            ChildObject.transform.localScale = new Vector3(0.05f, wallLen * scaleOffset, wallLen * scaleOffset);
        }
        else if (dirIndex == 1)
        {
            ChildObject.transform.localPosition = new Vector3(nowStep.startPos.x * wallLen - 2.5f, 0, nowStep.startPos.y * wallLen - 2.5f + halfWallLen);
            ChildObject.transform.localScale = new Vector3(wallLen * scaleOffset, wallLen * scaleOffset, 0.05f);
        }
        else if (dirIndex == 2)
        {
            ChildObject.transform.localPosition = new Vector3(nowStep.startPos.x * wallLen - 2.5f - halfWallLen, 0, nowStep.startPos.y * wallLen - 2.5f);
            ChildObject.transform.localScale = new Vector3(0.05f, wallLen * scaleOffset, wallLen * scaleOffset);
        }
        else if (dirIndex == 3)
        {
            ChildObject.transform.localPosition = new Vector3(nowStep.startPos.x * wallLen - 2.5f, 0, nowStep.startPos.y * wallLen - 2.5f - halfWallLen);
            ChildObject.transform.localScale = new Vector3(wallLen * scaleOffset, wallLen * scaleOffset, 0.05f);
        }
    }
    void forward(int dirIndex,step nowStep) {
        Vector2 newPos = nowStep.startPos+ direction[dirIndex];
        //Debug.Log("nowPos" + nowPos.ToString()+"+ dir"+ direction[dirIndex].ToString()+"=newPos" + newPos.ToString());
        if (map[(int)newPos.y, (int)newPos.x] == 0)
        {//新增到myStack
            step tmpStep=new step(newPos,nowStep.startPos);
            myStack.Push(tmpStep);
        }
        else if (map[(int)newPos.y, (int)newPos.x] == 1 && !Vector2.Equals(nowStep.comeFrom,newPos))
        {//產生牆壁
            generateWall(dirIndex, nowStep);
        }
        else if (map[(int)newPos.y, (int)newPos.x] == -1)
        {//產生牆壁
            generateWall(dirIndex, nowStep);
        }
    }
    void Start () {
        for (int i = 0; i < mapHeight; i++) {
            for (int j = 0; j < mapWidth; j++) {
                if (i == 0 || j == 0 || i == mapHeight-1 || j == mapWidth-1)
                {
                    map[i, j] = -1;
                }
                else {
                    map[i, j] = 0;
                }
            }
        }
        int tmpInt;
        ArrayList arrayRand = new ArrayList();
        System.Random random = new System.Random((int)DateTime.Now.Ticks);
        step tmpStep = new step(new Vector2(1, 1), new Vector2(1, 1));
        myStack.Push(tmpStep);
        while (myStack.Count > 0) {
            tmpStep = (step)myStack.Pop();
            if (map[(int)tmpStep.startPos.y, (int)tmpStep.startPos.x] == 1) {
                continue;
            } 
            map[(int)tmpStep.startPos.y, (int)tmpStep.startPos.x] = 1;
            arrayRand.Clear();
            while (arrayRand.Count < 4) {
                tmpInt = random.Next(0, 4);
                if (!arrayRand.Contains(tmpInt)) {
                    arrayRand.Add(tmpInt);
                }
            }
            foreach (int value in arrayRand) {
                forward(value, tmpStep);
            }
        }
    }
}
