function init(pattern, id)
	fireIndex = 0
	numSets = 4

	spread = 90
	spreadMin = 40
	spreadMax = 180
	spreadChangeSpeed = 1
	goalSpread = spreadMax

	currentAngle = 0
	currentSpinSpeed = 0
	maxSpinSpeed = 100
	spinAcceleration = 0
	reverseSpin = true
	reverseSpinSpeed = .99
	spinningClockwise = false
	fireTimes = pattern.GetFireTimes(4.666666, 1)
end

function updateSpread()
	if(spread < goalSpread) then
		spread = spread + spreadChangeSpeed
	elseif spread > goalSpread then
		spread = spread - spreadChangeSpeed
	end

	if(spread >= spreadMax) then
		spread = spreadMax
		goalSpread = spreadMin
	elseif spread <= spreadMin then
		spread = spreadMin
		goalSpread = spreadMax
	end

	spacePerSet = spread / numSets
end

function update(pattern, deltaTime)
	updateSpread()

	currentAngle = currentAngle + currentSpinSpeed * deltaTime
	if (spinningClockwise) then
		currentSpinSpeed = currentSpinSpeed + spinAcceleration * -1
	else
		currentSpinSpeed = currentSpinSpeed + spinAcceleration * 1
	end

	if(currentSpinSpeed < 0 ~= spinningClockwise) then
		currentSpinSpeed = currentSpinSpeed * reverseSpinSpeed;
	end
	if(currentSpinSpeed >= maxSpinSpeed) then
		spinningClockwise = true;
	elseif(currentSpinSpeed <= -maxSpinSpeed) then
		spinningClockwise = false;
	end

	if(pattern.GetStageTime() >= fireTimes[fireIndex]) then
		fireIndex = fireIndex + 1
		for place = -spread / 2 + spacePerSet/2, spread / 2 - spacePerSet/2, spacePerSet do
			bullet = pattern.NewBullet()
			bullet.speed = 4
			bullet.angle = place + currentAngle - 90
			bullet.material = "lightRed"
			pattern.SpawnBullet(bullet)
		end
	end
end