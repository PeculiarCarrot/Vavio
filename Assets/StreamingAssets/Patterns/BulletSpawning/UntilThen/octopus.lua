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
fireTimer = {}
spacePerSet = {}
startTime = {}

function init(pattern, id)
	fireIndex[id] = 0
	numSets[id] = 7
	spacePerSet[id] = 360 / numSets[id]

	currentAngle[id] = 40
	currentSpinSpeed[id] = 0
	maxSpinSpeed[id] = 80
	spinAcceleration[id] = 20
	reverseSpin[id] = true
	reverseSpinSpeed[id] = .9999999
	spinningClockwise[id] = false
	fireTimer[id] = 0
	initialized[id] = true
	startTime[id] = pattern.GetStageTime()
end

function update(pattern, id, deltaTime)
	if(not initialized[id]) then
		init(pattern, id)
	end

	if(startTime[id] > pattern.GetStageTime()) then
		return
	end

	currentAngle[id] = currentAngle[id] + currentSpinSpeed[id] * pattern.GetRealDeltaTime()
	if (spinningClockwise[id]) then
		currentSpinSpeed[id] = currentSpinSpeed[id] + spinAcceleration[id] * -1 * pattern.GetRealDeltaTime()
	else
		currentSpinSpeed[id] = currentSpinSpeed[id] + spinAcceleration[id] * 1 * pattern.GetRealDeltaTime()
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

	fireTimer[id] = fireTimer[id] + pattern.GetRealDeltaTime()

	if(fireTimer[id] >= .1) then
		fireTimer[id] = 0
		for place = 0, 360, spacePerSet[id] do
			bullet = pattern.NewBullet()
			bullet.speed = 4
			bullet.synced = true
			bullet.speedMultiplier = .99999
			bullet.angle = place + currentAngle[id]
			bullet.material = "orange"
			pattern.SpawnBullet(bullet)
		end
	end
end