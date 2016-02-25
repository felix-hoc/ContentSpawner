# ContentSpawner
Scriptable content spawner for Unity3D.

# The media objects
... which you'd like to spawn have to be located in Resources folder. URL is path starting from there.

# ScreenPlay definition file
The ScreenPlay definition file has to be in the same folder as the binary or in top folder of your project. It needs to be valid Json and have the following structure:

<pre>
{
  "_screenPlayItems":[
		{
			"url":"bowling_noise",
			"delay":  {
				"seconds":2.0,
				"type":0
			},
			"type":1
		},
		{
			"url":"sunset_image",
			"delay":  {
				"seconds":1.0,
				"type":1
			},
			"type":2
		},
		...
	]
}
</pre>

### Item types:
0 .... unknown

1 .... Audio

2 .... Image


### Delay types:
0 .... absolute to play command

1 .... relative to previous item
