fireIndex = 0

function update(pattern, deltaTime)
	if(fireTimes == nil) then
		fireTimes = pattern.GetFireTimes(1.16666, 1)
	end

	if(pattern.GetStageTime() >= fireTimes[fireIndex]) then
		fireIndex = fireIndex + 1

		bullet = pattern.NewBullet()
		bullet.movement = "General/homing"
		bullet.material = "orange"
		bullet.speed = 1.4
		bullet.lifetime = 4
		pattern.SpawnBullet(bullet)
	end
end

function init(pattern)
	
end