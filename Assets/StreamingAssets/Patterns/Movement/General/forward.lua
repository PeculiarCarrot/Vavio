function init(movement)
	movement.resetMoveOnUpdate = false
	movement.friction = .98
end

function update(movement, deltaTime)
	movement.SetMove(movement.speed)
end