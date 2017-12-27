
initialized = {}

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
end

function init(pattern, id)
	initialized[id] = true
		for i = 0, 360, (360 / 20) do
			bullet = pattern.NewBullet()
			bullet.speed = 2
			bullet.speedMultiplier = .9995
			bullet.type = "capsule"
			bullet.material = "darkAqua"
			bullet.angle = i
			bullet.scale = 1
			pattern.SpawnBullet(bullet)
		end
end