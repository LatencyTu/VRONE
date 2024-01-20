using UnityEngine;
using System.Collections;
using System.Text;
using System;
using System.Runtime.InteropServices;
using Templete;

public class GetMemory: MonoBehaviour
{
	float maxUsedMemory = 0.0f;
	void Update()
	{
		//获取当前系统
		//SystemInfo.operatingSystem;
		float totalMemory = SystemInfo.systemMemorySize;
		float reservedMemory = UnityEngine.Profiling.Profiler.GetTotalReservedMemoryLong() / 1024.0f / 1024.0f;
		float allocatedMemory = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong() / 1024.0f / 1024.0f;
		float usedMemory = reservedMemory + allocatedMemory;
		if (maxUsedMemory < usedMemory) maxUsedMemory = usedMemory;
		//totalMemory:{totalMemory}M\n
		//TestDebug.Log($"maxUsedMemory:{maxUsedMemory}M\nreservedMemory:{reservedMemory}M\nallocatedMemory:{allocatedMemory}M\nusedMemory:{usedMemory }M");
	}

}