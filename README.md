# ContentSpawner
Scriptable content spawner for Unity3D.

# The spawned media objects
... have to be located in Resources folder. URL is path starting from ther.e

# ScreenPlay definition file
The ScreenPlay definition file has to be in the same folder as the binary or in top folder of your project. It needs to be valid Json and have the following structure:

<pre>
{
  "_screenPlayItems":[
		{
			"url":"bowling_noise",
			"delay":2.0,
			"type":1
		},
		{
			"url":"sunset_image",
			"delay":1.0,
			"type":2
		},
		...
	]
}
</pre>

Types:

1. Audio
1. Image
