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

function init(pattern, id)
	fireIndex[id] = 0
	numSets[id] = 3
	spacePerSet[id] = 360 / numSets[id]

	currentAngle[id] = 0
	currentSpinSpeed[id] = 0
	maxSpinSpeed[id] = 200
	spinAcceleration[id] = .6
	reverseSpin[id] = true
	reverseSpinSpeed[id] = .9999
	spinningClockwise[id] = false
	fireTimes[id] = pattern.GetFireTimes(9.33333, .4)
	initialized[id] = true
end

function update(pattern, id, deltaTime)
	if(not initialized[id]) then
		init(pattern, id)
	end

	currentAngle[id] = currentAngle[id] + currentSpinSpeed[id] * deltaTime
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

	currentAngle[id] = pattern.Math().RandomRange(0, 360)

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1
		for place = 0, 360, spacePerSet[id] do
			bullet = pattern.NewBullet()
			bullet.speed = 3
			--bullet.z = -1
			bullet.angle = place + currentAngle[id]
			bullet.material = "darkAqua"
			pattern.SpawnBullet(bullet)
		end
	end
end