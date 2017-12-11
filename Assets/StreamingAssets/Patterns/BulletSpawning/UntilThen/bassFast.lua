
initialized = {}

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
end

function init(pattern, id)
	initialized[id] = true
		for i = 0, 360, 9 do
			bullet = pattern.NewBullet()
			bullet.speed = 5
			bullet.speedMultiplier = .999
			bullet.type = "capsule"
			bullet.material = "red"
			bullet.angle = i
			bullet.scale = 1
			pattern.SpawnBullet(bullet)
		end
end