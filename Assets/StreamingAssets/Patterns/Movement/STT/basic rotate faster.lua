function init(movement)

end

function update(movement, deltaTime)
	movement.Rotate(180 * movement.GetStageDeltaTime())
end