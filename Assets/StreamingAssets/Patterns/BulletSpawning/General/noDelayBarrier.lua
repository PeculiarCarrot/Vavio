
initialized = {}
index = {}

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end

	index[id] = index[id] + 1

	if index[id] == 5 then
		index[id] = 0
	end

	if(index[id] == 0) then
		bullet = pattern.NewBullet()
		bullet.speed = 6
		bullet.material = "white"
		bullet.lifetime = 5
		bullet.scale = 2
		bullet.destroyOnExitStage = false
		pattern.SpawnBullet(bullet)
	end
end

function init(pattern, id)
	initialized[id] = true
	index[id] = 0
end