turnSpd = {}
initialized = {}
spd = {}

function init(movement, id)
	movement.resetMoveOnUpdate = false
	movement.ignoreAngle = true
	movement.targetType = "nearestEnemy"
	spd[id] = movement.Math().RandomValue() * 2 + 2
	movement.friction = .7
	spd[id] = 1
	turnSpd[id] = .3
end

function update(movement, id, deltaTime)
	movement.FindTarget()
	local speed = spd[id] * movement.speed
	if movement.TargetExists() then
		local dx = movement.GetTargetX() - movement.GetX()
		local dy = movement.GetTargetY() - movement.GetY()
		local a = movement.Math().Atan2(dy, dx);
		movement.SetRotation(movement.Math().LerpAngle(movement.GetAngle(), a * movement.Math().Rad2Deg, turnSpd[id]))
	end
	
	movement.SetMove(
		speed * movement.Math().Cos(movement.Math().Deg2Rad * movement.GetAngle()),
		speed * movement.Math().Sin(movement.Math().Deg2Rad * movement.GetAngle()))

	--turnSpd[id] = turnSpd[id] * 1.01
	spd[id] = spd[id] * .99
end