initialized = {}
fireIndex = {}
numSets = {}
spread = {}
spreadMax = {}
spreadMin = {}
spreadChangeSpeed = {}
goalSpread = {}
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
	numSets[id] = 2

	spread[id] = 90
	spreadMin[id] = 40
	spreadMax[id] = 180
	spreadChangeSpeed[id] = 1
	goalSpread[id] = spreadMax[id]

	currentAngle[id] = 0
	currentSpinSpeed[id] = 0
	maxSpinSpeed[id] = 100
	spinAcceleration[id] = 0
	reverseSpin[id] = true
	reverseSpinSpeed[id] = .99
	spinningClockwise[id] = false
	fireTimes[id] = pattern.GetFireTimes(4.666666, 1)
	initialized[id] = true
end

function updateSpread(id)
	if(spread[id] < goalSpread[id]) then
		spread[id] = spread[id] + spreadChangeSpeed[id]
	elseif spread[id] > goalSpread[id] then
		spread[id] = spread[id] - spreadChangeSpeed[id]
	end

	if(spread[id] >= spreadMax[id]) then
		spread[id] = spreadMax[id]
		goalSpread[id] = spreadMin[id]
	elseif spread[id] <= spreadMin[id] then
		spread[id] = spreadMin[id]
		goalSpread[id] = spreadMax[id]
	end

	spacePerSet[id] = spread[id] / numSets[id]
end

function update(pattern, id, deltaTime)
	if(not initialized[id]) then
		init(pattern, id)
	end
	updateSpread(id)

	currentAngle[id] = currentAngle[id] + currentSpinSpeed[id] * deltaTime
	if (spinningClockwise[id]) then
		currentSpinSpeed[id] = currentSpinSpeed[id] + spinAcceleration[id] * -1
	else
		currentSpinSpeed[id] = currentSpinSpeed[id] + spinAcceleration[id] * 1
	end

	if(currentSpinSpeed[id] < 0 ~= spinningClockwise[id]) then
		currentSpinSpeed[id] = currentSpinSpeed[id] * reverseSpinSpeed[id];
	end
	if(reverseSpin[id]) then
		if(currentSpinSpeed[id] >= maxSpinSpeed[id]) then
			spinningClockwise[id] = true;
		elseif(currentSpinSpeed[id] <= -maxSpinSpeed[id]) then
			spinningClockwise[id] = false;
		end
	end

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1
		for place = -spread[id] / 2 + spacePerSet[id]/2, spread[id] / 2 - spacePerSet[id]/2, spacePerSet[id] do
			bullet = pattern.NewBullet()
			bullet.speed = 4
			bullet.angle = place + currentAngle[id] - 90
			bullet.material = "lightRed"
			pattern.SpawnBullet(bullet)
		end
	end
end