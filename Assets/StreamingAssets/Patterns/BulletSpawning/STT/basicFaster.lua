fireIndex = 0

function update(pattern, deltaTime)
	if(fireTimes == nil) then
		fireTimes = pattern.GetFireTimes(4.666666, 1)
	end

	if(pattern.GetStageTime() >= fireTimes[fireIndex]) then
		fireIndex = fireIndex + 1

		bullet = pattern.NewBullet()
		bullet.movement = "General/basic"
		bullet.speed = 3
		pattern.SpawnBullet(bullet)

		bullet = pattern.NewBullet()
		bullet.movement = "General/basic"
		bullet.speed = 3
		bullet.angle = 180
		pattern.SpawnBullet(bullet)
	end
end

function init(pattern)
	
end