initialized = {}
fireIndex = {}
numSets = {}
currentAngle = {}
currentSpinSpeed = {}
maxSpinSpeed = {}
spinAcceleration = {}
reverseSpin = {}
reverseSpinSpeed = {}
spinningClockwise = {}
fireTimes = {}
spacePerSet = {}
a = 0

function init(pattern, id)
	fireIndex[id] = 0
	numSets[id] = 1
	spacePerSet[id] = 360 / numSets[id]

	currentAngle[id] = 0
	currentSpinSpeed[id] = 0
	maxSpinSpeed[id] = 3
	spinAcceleration[id] = maxSpinSpeed[id]
	reverseSpin[id] = true
	reverseSpinSpeed[id] = .99
	spinningClockwise[id] = false
	fireTimes[id] = pattern.GetFireTimes(17.066666, .6)
	a = pattern.StageWidth() * .6
	initialized[id] = true
end

function update(pattern, id, deltaTime)
	if(not initialized[id]) then
		init(pattern, id)
	end

	currentAngle[id] = currentAngle[id] + currentSpinSpeed[id] * pattern.GetRealDeltaTime()
	if (spinningClockwise) then
		currentSpinSpeed[id] = currentSpinSpeed[id] + spinAcceleration[id] * -1
	else
		currentSpinSpeed[id] = currentSpinSpeed[id] + spinAcceleration[id] * 1
	end

	if(currentSpinSpeed[id] < 0 ~= spinningClockwise[id]) then
		currentSpinSpeed[id] = currentSpinSpeed[id] * reverseSpinSpeed[id];
	end
	if(currentSpinSpeed[id] >= maxSpinSpeed[id]) then
		spinningClockwise[id] = true;
	elseif(currentSpinSpeed[id] <= -maxSpinSpeed[id]) then
		spinningClockwise[id] = false;
	end

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1
		for place = 0, 360, spacePerSet[id] do
			bullet = pattern.NewBullet()
			bullet.speed = 2
			bullet.destroyOnExitStage = false
			bullet.lifetime = 20
			bullet.angle = place + currentAngle[id]
			bullet.x = pattern.Math().Cos(-bullet.angle * pattern.Math().Deg2Rad) * a
			bullet.y = pattern.Math().Sin(-bullet.angle * pattern.Math().Deg2Rad) * a
			bullet.angle = -bullet.angle - 90
			bullet.material = "aqua"
			pattern.SpawnBullet(bullet)

			bullet = pattern.NewBullet()
			bullet.speed = 2
			bullet.destroyOnExitStage = false
			bullet.lifetime = 20
			bullet.angle = place + currentAngle[id] + 180
			bullet.x = pattern.Math().Cos((-bullet.angle) * pattern.Math().Deg2Rad) * a
			bullet.y = pattern.Math().Sin((-bullet.angle) * pattern.Math().Deg2Rad) * a
			bullet.angle = -bullet.angle - 90
			bullet.material = "orange"
			pattern.SpawnBullet(bullet)
		end
	end
end