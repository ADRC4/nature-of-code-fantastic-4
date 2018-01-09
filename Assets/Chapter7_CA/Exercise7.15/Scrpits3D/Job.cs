using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Job : ThreadedJob
{
	public int[,,] worldInt;
	public bool[,,] world;
	public int size;
	public int Ysize;
	public bool useTime;
	public int _w = 2;
	public int _x = 3;
	public int _y = 3;
	public int _z = 3;
	public bool color;
	
	protected override void ThreadFunction()
	{
		// Do your threaded task. DON'T use the Unity API here
		Play();
	}

	protected override void OnFinished()
	{
		Debug.Log("Finished");
	}
	
	void SyncWorlds () {
		for(int x = 0; x < size; x++) {
			for(int y = 0; y < Ysize; y++) {
				for(int z = 0; z < size; z++) {
					if(worldInt[x,y,z] == 0)
						world[x,y,z] = false;
					else
						world[x,y,z] = true;
				}
			}
		}
	}
	
	void SyncWorldsTime () {
		for(int x = 0; x < size; x++) {
			for(int z = 0; z < size; z++) {
				if(worldInt[x,size-1,z] == 0)
					world[x,size-1,z] = false;
				else
					world[x,size-1,z] = true;
			}
		}
	}
	
	void Play () {
//		if(play || playOnce) {
//			gen ++;
//			genText.text = "Generation "+gen.ToString();
			if(useTime)
				PlayTime();
			else
				PlayNoTime();
//			ApplyColor();
//			playOnce = false;
//		}
	}
	
	void PlayTime () {
		GoDown();
		for(int x = 0; x < size; x++) {
			for(int z = 0; z < size; z++) {
				int neighbors = Neighbors(x,size-1,z);
				if(_y <= neighbors && neighbors <= _z) {
					if(!world[x,size-1,z])
						worldInt[x,size-1,z] = 4;
					else
						worldInt[x,size-1,z] = 3;
					
				} else if(_w <= neighbors && neighbors <= _x) {
					if(world[x,size-1,z])
						worldInt[x,size-1,z] = 3;
					else
						worldInt[x,size-1,z] = 0;
				} else
					worldInt[x,size-1,z] = 0;
			}
		}
		SyncWorldsTime();
		for(int x = 0; x < size; x++) {
			for(int z = 0; z < size; z++) {
				int neighbors = Neighbors(x,size-1,z);
				if(world[x,size-1,z]) {
					if(!((_y <= neighbors && neighbors <= _z) || (_w <= neighbors && neighbors <= _x)))
						worldInt[x,size-1,z] -= 2;
				}
			}
		}
	}
	
	void GoDown() {
		for(int x = 0; x < size; x++) {
			for(int y = 0; y < size-1; y++) {
				for(int z = 0; z < size; z++) {
					worldInt[x,y,z] = worldInt[x,y+1,z];
				}
			}
		}
	}
	
	void PlayNoTime () {
		for(int x = 0; x < size; x++) {
			for(int y = 0; y < Ysize; y++) {
				for(int z = 0; z < size; z++) {
					int neighbors = Neighbors(x,y,z);
					if(_y <= neighbors && neighbors <= _z) {
						if(!world[x,y,z])
							worldInt[x,y,z] = 4;
						else
							worldInt[x,y,z] = 3;
						
					} else if(_w <= neighbors && neighbors <= _x) {
						if(world[x,y,z])
							worldInt[x,y,z] = 3;
						else
							worldInt[x,y,z] = 0;
					} else
						worldInt[x,y,z] = 0;
				}
			}
		}
		SyncWorlds();
		if(color) {
			for(int x = 0; x < size; x++) {
				for(int y = 0; y < Ysize; y++) {
					for(int z = 0; z < size; z++) {
						int neighbors = Neighbors(x,y,z);
						if(world[x,y,z]) {
							if(!((_y <= neighbors && neighbors <= _z) || (_w <= neighbors && neighbors <= _x)))
								worldInt[x,y,z] -= 2;
						}
					}
				}
			}
		}
	}

	int Neighbors (int x, int y, int z) {
		int neighbors = 0;
		
		if(x != 0 && y != 0 && z != 0)
			neighbors += Convert.ToInt16(world[x-1, y-1,z-1]);
		if(x != 0 && y != 0)
			neighbors += Convert.ToInt16(world[x-1, y-1,z]);
		if(x != 0 && y != 0 && z != size-1)
			neighbors += Convert.ToInt16(world[x-1, y-1,z+1]);
		
		
		if(x != 0 && z != 0)
			neighbors += Convert.ToInt16(world[x-1, y,z-1]);
		if(x != 0)
			neighbors += Convert.ToInt16(world[x-1, y,z]);
		if(x != 0 && z != size-1)
			neighbors += Convert.ToInt16(world[x-1, y,z+1]);
		
		
		if(x != 0 && y != Ysize-1 && z != 0)
			neighbors += Convert.ToInt16(world[x-1, y+1,z-1]);
		if(x != 0 && y != Ysize-1)
			neighbors += Convert.ToInt16(world[x-1, y+1,z]);
		if(x != 0 && y != Ysize-1 && z != size-1)
			neighbors += Convert.ToInt16(world[x-1, y+1,z+1]);
		
		
		
		if(y != 0 && z != 0)
			neighbors += Convert.ToInt16(world[x, y-1,z-1]);
		if(y != 0)
			neighbors += Convert.ToInt16(world[x, y-1,z]);
		if(y != 0 && z != size-1)
			neighbors += Convert.ToInt16(world[x, y-1,z+1]);
		
		
		if(z != 0)
			neighbors += Convert.ToInt16(world[x, y,z-1]);
		//		if(x != 0)
		//			neighbors += Convert.ToInt16(world[x, y,z]);
		if(z != size-1)
			neighbors += Convert.ToInt16(world[x, y,z+1]);
		
		
		if(y != Ysize-1 && z != 0)
			neighbors += Convert.ToInt16(world[x, y+1,z-1]);
		if(y != Ysize-1)
			neighbors += Convert.ToInt16(world[x, y+1,z]);
		if(y != Ysize-1 && z != size-1)
			neighbors += Convert.ToInt16(world[x, y+1,z+1]);
		
		
		if(x != size-1 && y != 0 && z != 0)
			neighbors += Convert.ToInt16(world[x+1, y-1,z-1]);
		if(x != size-1 && y != 0)
			neighbors += Convert.ToInt16(world[x+1, y-1,z]);
		if(x != size-1 && y != 0 && z != size-1)
			neighbors += Convert.ToInt16(world[x+1, y-1,z+1]);
		
		
		if(x != size-1 && z != 0)
			neighbors += Convert.ToInt16(world[x+1, y,z-1]);
		if(x != size-1)
			neighbors += Convert.ToInt16(world[x+1, y,z]);
		if(x != size-1 && z != size-1)
			neighbors += Convert.ToInt16(world[x+1, y,z+1]);
		
		
		if(x != size-1 && y != Ysize-1 && z != 0)
			neighbors += Convert.ToInt16(world[x+1, y+1,z-1]);
		if(x != size-1 && y != Ysize-1)
			neighbors += Convert.ToInt16(world[x+1, y+1,z]);
		if(x != size-1 && y != Ysize-1 && z != size-1)
			neighbors += Convert.ToInt16(world[x+1, y+1,z+1]);
		return neighbors;
	}
}