initialized = {}
fireIndex = {}
numSets = {}
currentAngle = {}
currentSpinSpeed = {}
currentAngle2 = {}
currentSpinSpeed2 = {}
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

	currentAngle[id] = pattern.GetAngle()
	currentAngle2[id] = 0
	currentSpinSpeed[id] = 0
	currentSpinSpeed2[id] = 0
	maxSpinSpeed[id] = 20
	spinAcceleration[id] = .3
	reverseSpin[id] = false
	reverseSpinSpeed[id] = .99999
	spinningClockwise[id] = false
	fireTimes[id] = pattern.GetFireTimes(8.66666, .9)
	initialized[id] = true
end

function update(pattern, id, deltaTime)
	if(not initialized[id]) then
		init(pattern, id)
	end

	currentAngle[id] = currentAngle[id] + currentSpinSpeed[id] * deltaTime

	currentSpinSpeed[id] = 40

	currentAngle2[id] = currentAngle2[id] + currentSpinSpeed2[id] * deltaTime

	currentSpinSpeed2[id] = 0

	if(pattern.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1
		for place = 0, 360, spacePerSet[id] do
			bullet = pattern.NewBullet()
			bullet.speed = 3
			bullet.angle = place + currentAngle[id] + currentAngle2[id]
			bullet.material = "darkAqua"
			pattern.SpawnBullet(bullet)
		end
		for place = 0, 360, spacePerSet[id] do
			bullet = pattern.NewBullet()
			bullet.speed = 3
			bullet.angle = place - currentAngle[id] + currentAngle2[id]
			bullet.material = "aqua"
			pattern.SpawnBullet(bullet)
		end
	end
end