
initialized = {}

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
end

function init(pattern, id)
	initialized[id] = true
	local goalX = 0;
	local goalY = 0;
	goalX = goalX - pattern.GetX()
	goalY = goalY - pattern.GetY()
	dist = pattern.StageWidth() / 2
	dist2 = dist + 2
	dist3 = dist + 4
	bullets = 25

		for i = 0, 360, (360 / bullets) do
			bullet = pattern.NewBullet()
			bullet.speed = 5
			bullet.speedMultiplier = 1
			bullet.type = "circle"
			bullet.material = "red"
			bullet.x = pattern.Math().Cos(i * pattern.Math().Deg2Rad) * dist + goalX
			bullet.y = pattern.Math().Sin(i * pattern.Math().Deg2Rad) * dist + goalY
			bullet.angle = i - 180
			bullet.scale = .5
			bullet.destroyOnExitStage = false;
			pattern.SpawnBullet(bullet)
		end
		off = (360 / bullets) / 2
		for i = off, 360 + off, (360 / bullets) do
			bullet = pattern.NewBullet()
			bullet.speed = 5
			bullet.speedMultiplier = 1
			bullet.type = "circle"
			bullet.material = "red"
			bullet.x = pattern.Math().Cos(i * pattern.Math().Deg2Rad) * dist2 + goalX
			bullet.y = pattern.Math().Sin(i * pattern.Math().Deg2Rad) * dist2 + goalY
			bullet.angle = i - 180
			bullet.scale = .5
			bullet.destroyOnExitStage = false;
			pattern.SpawnBullet(bullet)
		end
		for i = 0, 360, (360 / bullets) do
			bullet = pattern.NewBullet()
			bullet.speed = 5
			bullet.speedMultiplier = 1
			bullet.type = "circle"
			bullet.material = "red"
			bullet.x = pattern.Math().Cos(i * pattern.Math().Deg2Rad) * dist3 + goalX
			bullet.y = pattern.Math().Sin(i * pattern.Math().Deg2Rad) * dist3 + goalY
			bullet.angle = i - 180
			bullet.scale = .5
			bullet.destroyOnExitStage = false;
			pattern.SpawnBullet(bullet)
		end
end