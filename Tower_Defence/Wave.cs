using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tower_Defence
{
	class Wave
	{
		public string Name { get; set; }
		public int SpawnNumber { get; set; }
		public List<Enemy> EnemySpawnQueue { get; set; }

		public Wave(string name, List<Enemy> enemySpawnQueue)
		{
			Name = name;
			SpawnNumber = 0;
			EnemySpawnQueue = enemySpawnQueue;
		}
	}
}
