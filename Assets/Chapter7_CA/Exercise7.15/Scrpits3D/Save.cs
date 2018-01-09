using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Save {
	public int size;
	public int Ysize;
	public int[,,] worldInt;
	public bool useTransparentsMats ;
	public bool use2D;
	public bool useTime;

	public bool threated;
	
	public int _w;
	public int _x;
	public int _y;
	public int _z;

	public int gen;

	public Save (bool s_threated, int s_size, int s_Ysize, int[,,] s_worldInt, bool s_useTransparentsMats, bool s_use2D, bool s_useTime, int s_w, int s_x, int s_y, int s_z, int s_gen) {
		threated = s_threated;
		size = s_size;
		Ysize = s_Ysize;
		worldInt = s_worldInt;
		useTransparentsMats = s_useTransparentsMats;
		use2D = s_use2D;
		useTime = s_useTime;
		_w = s_w;
		_x = s_x;
		_y = s_y;
		_z = s_z;
		gen = s_gen;
	}
}
