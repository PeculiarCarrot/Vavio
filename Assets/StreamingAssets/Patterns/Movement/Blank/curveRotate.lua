function init(movement, id)

end

function update(movement, id, deltaTime)
	movement.Rotate(120 * movement.GetStageDeltaTime())
end