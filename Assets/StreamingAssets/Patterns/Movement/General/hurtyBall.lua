function init(movement, id)
	movement.resetMoveOnUpdate = false
	movement.speed = 1.75;
	movement.speed = movement.Math().RandomRange(movement.speed, movement.speed + 2)
end

function update(movement, id, deltaTime)
	movement.SetMove(movement.speed)
end