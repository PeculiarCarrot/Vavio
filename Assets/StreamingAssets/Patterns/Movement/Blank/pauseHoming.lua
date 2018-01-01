turnSpd = {}
initialized = {}
spd = {}
fireIndex = {}
fireTimes = {}
moving = {}

function init(movement, id)
	movement.resetMoveOnUpdate = true
	movement.ignoreAngle = true
	movement.targetType = "player"
	fireIndex[id] = 0
	moving[id] = true
	spd[id] = movement.Math().RandomValue() * 2 + 2
	movement.friction = .97
	spd[id] = 4
	turnSpd[id] = .005 + movement.Math().RandomValue() * .1
end

function update(movement, id, deltaTime)
	if(fireTimes[id] == nil) then
		fireTimes[id] = movement.GetFireTimes(0.857, 0, 60)
	end
	if(movement.GetStageTime() >= fireTimes[id][fireIndex[id]]) then
		fireIndex[id] = fireIndex[id] + 1
		moving[id] = not moving[id]
	end
	movement.FindTarget()
	local speed = spd[id] * movement.speed
	local dx = movement.GetTargetX() - movement.GetX()
	local dy = movement.GetTargetY() - movement.GetY()
	local a = movement.Math().Atan2(dy, dx);
	
	if(moving[id]) then
		movement.SetMove(
			speed * movement.Math().Cos(movement.Math().Deg2Rad * movement.GetAngle()),
			speed * movement.Math().Sin(movement.Math().Deg2Rad * movement.GetAngle()))
	else
		movement.SetRotation(movement.Math().LerpAngle(movement.GetAngle(), a * movement.Math().Rad2Deg, turnSpd[id]))
	end
end