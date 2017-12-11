function init(movement)
	movement.resetMoveOnUpdate = false
	movement.friction = .98
end

function update(movement, deltaTime)
	movement.SetMove(7)
end