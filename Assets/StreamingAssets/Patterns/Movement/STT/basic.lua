function init(movement)

end

function update(movement, deltaTime)
	movement.Rotate(30 * movement.GetStageDeltaTime())
end