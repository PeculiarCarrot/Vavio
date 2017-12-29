fireIndex = {}
fireTimes = {}
initialized = {}
anglePer = {}
color = 0

function update(pattern, id, deltaTime)
	if(initialized[id] == nil) then
		init(pattern, id)
	end
end

function init(pattern, id)
	initialized[id] = true
	fireIndex[id] = 0
	anglePer[id] = 25
	numBullets = pattern.Math().RandomRangeInt(20, 40)

	turn = pattern.Math().RandomRange(-20, 20)
	color = color + 1

	if color > 3 then color = 0 end

	angle = pattern.Math().RandomRange(0, 360)
	for i = angle, angle + 360, (360 / numBullets) do
		bullet = pattern.NewBullet()
		bullet.speed = 4
		bullet.angle = i
		bullet.type = "circle"
		bullet.turn = turn
		if(color == 0) then
			bullet.material = "orange"
		elseif color == 1 then
			bullet.material = "darkRed"
		elseif color == 2 then
			bullet.material = "purple"
		elseif color == 3 then
			bullet.material = "aqua"
		end
		bullet.scale = 1
		pattern.SpawnBullet(bullet)
	end
end