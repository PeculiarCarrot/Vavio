function init(movement)
	movement.resetMoveOnUpdate = false
	movement.friction = .98
	movement.synced = true
end

function update(movement, deltaTime)
	movement.SetMove(7)
end